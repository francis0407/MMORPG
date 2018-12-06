using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [Serializable]
    public class SPlayerEquipItem : Message
    {
        public SPlayerEquipItem() : base(Command.S_PLAYER_EQUIP_ITEM) { }
        public int item_id;
    }

    [Serializable]
    public class SPlayerUnEquipItem : Message
    {
        public SPlayerUnEquipItem() : base(Command.S_PLAYER_UNEQUIP_ITEM) { }
        public int item_id;
    }

    [Serializable]
    public class SPlayerDropItem : Message
    {
        public SPlayerDropItem() : base(Command.S_PLAYER_DROP_ITEM) { }
        public int item_id;
    }

    [Serializable]
    public class SPlayerChangeItem : Message
    {
        public SPlayerChangeItem() : base(Command.S_PLAYER_CHANGE_ITEM) { }
        public int old_id;
        public int new_id;
    }
}
