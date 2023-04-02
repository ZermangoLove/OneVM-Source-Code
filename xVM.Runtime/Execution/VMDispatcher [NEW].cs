using System;
using System.Runtime.CompilerServices;
using System.Text;
using xVM.Runtime.Data;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Services;
using xVM.Runtime.Execution.Internal;

namespace xVM.Runtime.Execution
{
    internal unsafe static class VMDispatcher
    {
        public static ExecutionState Invoke(VMContext ctx)
        {
            var state = ExecutionState.Next;
            var isAbnormal = true;
            do
            {
                try
                {
                    state = RunInternal(ctx);
                    switch (state)
                    {
                        case ExecutionState.Throw:
                            {
                                var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
                                var ex = ctx.Stack[sp--];
                                ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
                                DoThrow(ctx, ex.O);
                                break;
                            }
                        case ExecutionState.Rethrow:
                            {
                                var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
                                var ex = ctx.Stack[sp--];
                                ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
                                HandleRethrow(ctx, ex.O);
                                return state;
                            }
                    }
                    isAbnormal = false;
                }
                catch (Exception ex)
                {
                    // Patched to catch object
                    SetupEHState(ctx, ex);
                    isAbnormal = false;
                }
                finally
                {
                    if (isAbnormal)
                    {
                        HandleAbnormalExit(ctx);
                        state = ExecutionState.Exit;
                    }
                    else if (ctx.EHStates.Count > 0)
                    {
                        do
                        {
                            HandleEH(ctx, ref state);
                        } while (state == ExecutionState.Rethrow);
                    }
                }
            } while (state != ExecutionState.Exit);
            return state;
        }

        private static ExecutionState RunInternal(VMContext ctx)
        {
            ExecutionState state;
            while (true)
            {
                var op = ctx.ReadByte();
                var p = ctx.ReadByte(); // For key fixup
                OpCodeMap.Lookup(op).Run(ctx, out state);

                if (ctx.Registers[Constants.OPCODELIST[18]].U8 == 1)
                    state = ExecutionState.Exit;

                if (state != ExecutionState.Next)
                    return state;
            }
        }

        private static void SetupEHState(VMContext ctx, object ex)
        {
            EHState ehState;
            if (ctx.EHStates.Count != 0)
            {
                ehState = ctx.EHStates[ctx.EHStates.Count - 1];
                if (ehState.CurrentFrame != null)
                {
                    if (ehState.CurrentProcess == EHState.EHProcess.Searching) ctx.Registers[Constants.OPCODELIST[2]].U1 = 0;
                    else if (ehState.CurrentProcess == EHState.EHProcess.Unwinding) ehState.ExceptionObj = ex;
                    return;
                }
            }
            ehState = new EHState
            {
                OldBP = ctx.Registers[Constants.OPCODELIST[14]],
                OldSP = ctx.Registers[Constants.OPCODELIST[16]],
                ExceptionObj = ex,
                CurrentProcess = EHState.EHProcess.Searching,
                CurrentFrame = null,
                HandlerFrame = null
            };
            ctx.EHStates.Add(ehState);
        }

        private static void HandleRethrow(VMContext ctx, object ex)
        {
            if (ctx.EHStates.Count > 0)
                SetupEHState(ctx, ex);
            else
                DoThrow(ctx, ex);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void DoThrow(VMContext ctx, object ex)
        {
            if (ex is Exception)
            {
                EHHelper.Rethrow((Exception)ex, GetIP(ctx));
            }
            throw Throw(ex);
        }

        private static unsafe string GetIP(VMContext ctx)
        {
            var ip = (uint)(ctx.Registers[Constants.OPCODELIST[18]].U8 - (ulong)Interpreter.__ILVDATA);
            ulong key = (uint)(new object().GetHashCode() + Environment.TickCount) | 1;
            return ((ip * key) << 32 | (key & (~1UL))).ToString("x16");
        }

        private static Exception Throw(object obj)
        {
            return null;
        }

        private static void HandleEH(VMContext ctx, ref ExecutionState state)
        {
            var ehState = ctx.EHStates[ctx.EHStates.Count - 1];
            switch (ehState.CurrentProcess)
            {
                case EHState.EHProcess.Searching:
                    {
                        if (ehState.CurrentFrame != null)
                        {
                            // Return from filter
                            if (ctx.Registers[Constants.OPCODELIST[2]].U1 != 0)
                            {
                                ehState.CurrentProcess = EHState.EHProcess.Unwinding;
                                ehState.HandlerFrame = ehState.CurrentFrame;
                                ehState.CurrentFrame = ctx.EHStack.Count;
                                state = ExecutionState.Next;
                                goto case EHState.EHProcess.Unwinding;
                            }
                            ehState.CurrentFrame--;
                        }
                        else
                        {
                            ehState.CurrentFrame = ctx.EHStack.Count - 1;
                        }

                        var exType = ehState.ExceptionObj.GetType();
                        for (; ehState.CurrentFrame >= 0 && ehState.HandlerFrame == null; ehState.CurrentFrame--)
                        {
                            var frame = ctx.EHStack[ehState.CurrentFrame.Value];
                            if (frame.EHType == Constants.OPCODELIST[228])
                            {
                                // Run filter
                                var sp = ehState.OldSP.U4;
                                ctx.Stack.SetTopPosition(++sp);
                                ctx.Stack[sp] = new VMSlot { O = ehState.ExceptionObj };
                                ctx.Registers[Constants.OPCODELIST[22]].U1 = 0;
                                ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
                                ctx.Registers[Constants.OPCODELIST[14]] = frame.BP;
                                ctx.Registers[Constants.OPCODELIST[18]].U8 = frame.FilterAddr;
                                break;
                            }
                            if (frame.EHType == Constants.OPCODELIST[226])
                                if (frame.CatchType.IsAssignableFrom(exType))
                                {
                                    ehState.CurrentProcess = EHState.EHProcess.Unwinding;
                                    ehState.HandlerFrame = ehState.CurrentFrame;
                                    ehState.CurrentFrame = ctx.EHStack.Count;
                                    goto case EHState.EHProcess.Unwinding;
                                }
                        }
                        if (ehState.CurrentFrame == -1 && ehState.HandlerFrame == null)
                        {
                            ctx.EHStates.RemoveAt(ctx.EHStates.Count - 1);
                            state = ExecutionState.Rethrow;
                            if (ctx.EHStates.Count == 0)
                                HandleRethrow(ctx, ehState.ExceptionObj);
                        }
                        else
                        {
                            state = ExecutionState.Next;
                        }
                        break;
                    }
                case EHState.EHProcess.Unwinding:
                    {
                        ehState.CurrentFrame--;
                        int i;
                        for (i = ehState.CurrentFrame.Value; i > ehState.HandlerFrame.Value; i--)
                        {
                            var frame = ctx.EHStack[i];
                            ctx.EHStack.RemoveAt(i);
                            if (frame.EHType == Constants.OPCODELIST[230] || frame.EHType == Constants.OPCODELIST[232])
                            {
                                // Run finally
                                SetupFinallyFrame(ctx, frame);
                                break;
                            }
                        }
                        ehState.CurrentFrame = i;

                        if (ehState.CurrentFrame == ehState.HandlerFrame)
                        {
                            var frame = ctx.EHStack[ehState.HandlerFrame.Value];
                            ctx.EHStack.RemoveAt(ehState.HandlerFrame.Value);
                            // Run handler
                            frame.SP.U4++;
                            ctx.Stack.SetTopPosition(frame.SP.U4);
                            ctx.Stack[frame.SP.U4] = new VMSlot { O = ehState.ExceptionObj };

                            ctx.Registers[Constants.OPCODELIST[22]].U1 = 0;
                            ctx.Registers[Constants.OPCODELIST[16]] = frame.SP;
                            ctx.Registers[Constants.OPCODELIST[14]] = frame.BP;
                            ctx.Registers[Constants.OPCODELIST[18]].U8 = frame.HandlerAddr;

                            ctx.EHStates.RemoveAt(ctx.EHStates.Count - 1);
                        }
                        state = ExecutionState.Next;
                        break;
                    }
                default:
                    throw new ExecutionEngineException();
            }
        }

        private static void HandleAbnormalExit(VMContext ctx)
        {
            var oldBP = ctx.Registers[Constants.OPCODELIST[14]];
            var oldSP = ctx.Registers[Constants.OPCODELIST[16]];

            for (var i = ctx.EHStack.Count - 1; i >= 0; i--)
            {
                var frame = ctx.EHStack[i];
                if (frame.EHType == Constants.OPCODELIST[230] || frame.EHType == Constants.OPCODELIST[232])
                {
                    SetupFinallyFrame(ctx, frame);
                    Invoke(ctx);
                }
            }
            ctx.EHStack.Clear();
        }

        private static void SetupFinallyFrame(VMContext ctx, EHFrame frame)
        {
            frame.SP.U4++;
            ctx.Registers[Constants.OPCODELIST[22]].U1 = 0;
            ctx.Registers[Constants.OPCODELIST[16]] = frame.SP;
            ctx.Registers[Constants.OPCODELIST[14]] = frame.BP;
            ctx.Registers[Constants.OPCODELIST[18]].U8 = frame.HandlerAddr;

            ctx.Stack[frame.SP.U4] = new VMSlot { U8 = 1 };
        }
    }
}