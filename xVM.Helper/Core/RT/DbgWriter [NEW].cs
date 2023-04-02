using System.Collections.Generic;

using xVM.Helper.Core.AST.IL;
using xVM.Helper.Core.Helpers;

namespace xVM.Helper.Core.RT
{
    internal class DbgWriter
    {
        private readonly HashSet<string> documents = new HashSet<string>();

        private readonly Dictionary<ILBlock, List<DbgEntry>> entries = new Dictionary<ILBlock, List<DbgEntry>>();

        public void AddSequencePoint(ILBlock block, uint offset, uint len, string document, uint lineNum)
        {
            List<DbgEntry> entryList;
            if(!entries.TryGetValue(block, out entryList))
                entryList = entries[block] = new List<DbgEntry>();

            entryList.Add(new DbgEntry
            {
                offset = offset,
                len = len,
                document = document,
                lineNum = lineNum
            });
            documents.Add(document);
        }

        private struct DbgEntry
        {
            public uint offset;
            public uint len;

            public string document;
            public uint lineNum;
        }
    }
}