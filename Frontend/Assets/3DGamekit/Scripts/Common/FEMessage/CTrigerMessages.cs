using System;

namespace Common
{
    [Serializable]
    public class CTrigerOnEnter : Message
    {
        public CTrigerOnEnter() : base(Command.C_TRIGER_ON_ENTER) { }
        public SwitchCrystal switchCrystal;
        public PressurePad pressurePad;
        public HealthBox healthBox;
    }
}
