using System;
using System.Collections.Generic;
namespace Common
{
    [Serializable]
    public class SGetMarketItems : Message
    {
        public SGetMarketItems() : base(Command.S_GET_MARKET_ITEMS) { }
        public MarketItem[] items;
    }


    [Serializable]
    public class SBuyMarketItem : Message
    {
        public SBuyMarketItem() : base(Command.S_BUY_MARKET_ITEM) { }
        public MarketItem item;
    }

    [Serializable]
    public class SSellMarketItem : Message
    {
        public SSellMarketItem() : base(Command.S_SELL_MARKET_ITEM) { }
        public MarketItem item;
    }
}
