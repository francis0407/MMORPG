using System;

namespace Common
{
    [Serializable]
    public class CCreateItem : Message
    {
        public CCreateItem() : base(Command.C_CREATE_ITEM) { }
        public int luck;
    }
}
