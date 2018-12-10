using System;
using System.Collections.Generic;
namespace Common
{
    [Serializable]
    public class CGetTmallItems : Message
    {
        public CGetTmallItems() : base(Command.C_GET_TMALL_ITEMS) { }
    }

    [Serializable]
    public class TmallItem
    {
        public ItemConf itemConf;
        public CostConf costConf;
        public int count;
    }

    [Serializable]
    public class CBuyTmallItems : Message
    {
        public CBuyTmallItems() : base(Command.C_BUY_TMALL_ITEMS) { }
        public TmallItem[] tmallItems;
        public int luck;
    }
}
