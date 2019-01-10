using System;

namespace Common
{
    [Serializable]
    public partial class STrigerStatus : Message
    {
        public STrigerStatus() : base(Command.S_TRIGER_STATUS) { }
        public Door[] doors;
        public PressurePad[] pressurePads;
        public SwitchCrystal[] switchCrystals;
        public HealthBox[] healthBoxes;
    }

    [Serializable]
    public partial class STrigerOnEnter : Message
    {
        public STrigerOnEnter() : base(Command.S_TRIGER_ON_ENTER) { }
        public PressurePad pressurePad;
        public SwitchCrystal switchCrystal;
        public HealthBox healthBox;
    }
}
