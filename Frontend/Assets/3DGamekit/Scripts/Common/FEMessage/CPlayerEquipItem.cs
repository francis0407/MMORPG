using System;
namespace Common
{
    [Serializable]
    public class CPlayerEquipItem : Message
    {
        public CPlayerEquipItem() : base(Command.C_PLAYER_EQUIP_ITEM) { }
        public int item_id;
    }

    [Serializable]
    public class CPlayerUnEquipItem : Message
    {
        public CPlayerUnEquipItem() : base(Command.C_PLAYER_UNEQUIP_ITEM) { }
        public int item_id;
    }

    [Serializable]
    public class CPlayerDropItem : Message
    {
        public CPlayerDropItem() : base(Command.C_PLAYER_DROP_ITEM) { }
        public int item_id;
    }

    [Serializable]
    public class CPlayerChangeItem : Message
    {
        public CPlayerChangeItem() : base(Command.C_PLAYER_CHANGE_ITEM) { }
        public int old_id;
        public int new_id;
    }
}
