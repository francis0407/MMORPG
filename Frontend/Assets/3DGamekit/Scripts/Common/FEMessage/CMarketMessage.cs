using System;
using System.Collections.Generic;
namespace Common
{
    [Serializable]
    public class CGetMarketItems : Message
    {
        public CGetMarketItems() : base(Command.C_GET_MARKET_ITEMS) { }
    }


    [Serializable]
    public class CBuyMarketItem : Message
    {
        public CBuyMarketItem() : base(Command.C_BUY_MARKET_ITEM) { }
        public MarketItem item;
    }

    [Serializable]
    public class CSellMarketItem : Message
    {
        public CSellMarketItem() : base(Command.C_SELL_MARKET_ITEM) { }
        public MarketItem item;
    }
}
