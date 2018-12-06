using System;
using FrontEnd.Item;
namespace Common
{
    [Serializable]
    public partial class SGetItem : Message
    {
        public SGetItem() : base(Command.S_GET_ITEM) { }
        public bool success;
        public DItem dItem;
    }
}
