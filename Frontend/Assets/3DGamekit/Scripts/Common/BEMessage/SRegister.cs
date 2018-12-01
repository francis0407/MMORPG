using System;

namespace Common
{
    [Serializable]
    public class SRegister : Message
    {
        public SRegister() : base(Command.S_PLAYER_REGISTER) { }
        public enum Status
        {
            Success,
            Fail,
            Error
        }
        public Status status;

    }
}
