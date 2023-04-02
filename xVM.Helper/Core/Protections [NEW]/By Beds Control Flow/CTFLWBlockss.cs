using System.Linq;
using System.Collections.Generic;

using xVM.Helper.Core.Protections.Generator.Context;

namespace xVM.Helper.Core.Protections
{
    internal class CTFLWBlockss
    {
        public List<CTFLWBlock> blocks = new List<CTFLWBlock>();
        private xVM.Helper.Core.Protections.Generator.Context.Generator generator = new xVM.Helper.Core.Protections.Generator.Context.Generator();
        public CTFLWBlock getBlock(int id)
        {
            return blocks.Single(block => block.ID == id);
        }

        public void Scramble(out CTFLWBlockss incGroups)
        {
            var groups = new CTFLWBlockss();
            foreach (var group in blocks)
                groups.blocks.Insert(generator.Generate<int>(GeneratorType.Integer, groups.blocks.Count), group);
            incGroups = groups;
        }
    }
}
