using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Services;
using xVM.Helper.Core.Helpers.System;

namespace xVM.Helper.Core
{
    internal class Scanner
    {
        private readonly HashSet<MethodDef> Exclude = new HashSet<MethodDef>();
        private readonly HashSet<MethodDef> Export = new HashSet<MethodDef>();
        internal static HashSet<MethodDef> MethodsDF = new HashSet<MethodDef>();
        private readonly ModuleDef ModuleDF;
        private readonly HashSet<Tuple<MethodDef, bool>> Results = new HashSet<Tuple<MethodDef, bool>>();

        public Scanner(ModuleDef module, CompressionService compression)
        {
            ModuleDF = module;
            MethodsDF = new HashSet<MethodDef>(FullNameToMethod_Class.FullNamesToMethods(module, XMLUtils.Methods_FullName));          

            #region Remove <Module> Ctor Method (For Anti Tamper)
            ///////////////////////////////////////////////////////////////////////////////////////////
            if (XMLUtils.Methods_FullName.Contains(module.GlobalType.FindOrCreateStaticConstructor().FullName))
            {
                XMLUtils.Methods_FullName.Remove(module.GlobalType.FindOrCreateStaticConstructor().FullName);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            compression.TryGetRuntimeDecompressor(ModuleDF, def =>
            {
                if (def is MethodDef)
                    MethodsDF.Remove((MethodDef)def);
            });
        }
       
        public IEnumerable<Tuple<MethodDef, bool>> Scan()
        {
            ScanMethods(FindExclusion);
            ScanMethods(ScanExport);
            ScanMethods(PopulateResult);
            return Results;
        }

        private void ScanMethods(Action<MethodDef> scanFunc)
        {
            foreach(var type in ModuleDF.GetTypes())
            foreach(var method in type.Methods)
                scanFunc(method);
        }

        private void FindExclusion(MethodDef method)
        {
            if(!method.HasBody || MethodsDF != null && !MethodsDF.Contains(method))
                Exclude.Add(method);
            else if(method.HasGenericParameters)
                foreach(var instr in method.Body.Instructions)
                {
                    var target = instr.Operand as IMethod;
                    if(target != null && target.IsMethod &&
                       (target = target.ResolveMethodDef()) != null &&
                       (MethodsDF == null || MethodsDF.Contains((MethodDef) target)))
                        Export.Add((MethodDef) target);
                }
        }

        private void ScanExport(MethodDef method)
        {
            if(!method.HasBody)
                return;

            var shouldExport = false;
            shouldExport |= method.IsPublic;
            shouldExport |= method.SemanticsAttributes != 0;
            shouldExport |= method.IsConstructor;
            shouldExport |= method.IsVirtual;
            shouldExport |= method.Module.EntryPoint == method;
            if(shouldExport)
                Export.Add(method);

            var excluded = Exclude.Contains(method) || method.DeclaringType.HasGenericParameters;
            foreach(var instr in method.Body.Instructions)
                if(instr.OpCode == OpCodes.Callvirt ||
                   instr.Operand is IMethod && excluded)
                {
                    var target = ((IMethod) instr.Operand).ResolveMethodDef();
                    if(target != null && (MethodsDF == null || MethodsDF.Contains(target)))
                        Export.Add(target);
                }
        }

        private void PopulateResult(MethodDef method)
        {
            if(Exclude.Contains(method) || method.DeclaringType.HasGenericParameters)
                return;
            Results.Add(Tuple.Create(method, Export.Contains(method)));
        }
    }
}