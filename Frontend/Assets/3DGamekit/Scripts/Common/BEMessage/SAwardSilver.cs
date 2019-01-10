using System;

namespace Common
{
    [Serializable]
    public class SAwardSilver : Message
    {
        public SAwardSilver() : base(Command.S_AWARD_SILVER) { }
        public int count;
    }
}
