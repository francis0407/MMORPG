using System;
using System.Collections.Generic;
using FrontEnd.Item;
namespace Common
{

    [Serializable]
    public class SGetTmallItems : Message
    {
        public SGetTmallItems() : base(Command.S_GET_TMALL_ITEMS) { }
        public KeyValuePair<ItemConf, CostConf>[] TmallItems;
    }

    [Serializable]
    public class SBuyTmallItems : Message
    {
        public SBuyTmallItems() : base(Command.S_BUY_TMALL_ITEMS) { }
        public DItem[] dItems;
        public int silver;
        public int gold;
    }
}
