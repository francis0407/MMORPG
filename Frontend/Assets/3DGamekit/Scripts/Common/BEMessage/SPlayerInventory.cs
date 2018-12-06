using System;
using FrontEnd.Item;
namespace Common
{
    [Serializable]
    public partial class SPlayerInventory : Message
    {
        public SPlayerInventory() : base(Command.S_PLAYER_INVENTORY) { }       
        public DItem[] dItems;
    }
}
