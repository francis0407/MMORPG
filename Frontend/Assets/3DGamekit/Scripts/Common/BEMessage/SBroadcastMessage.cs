using System;

namespace Common
{
    [Serializable]
    public class SBroadcastMessage : Message
    {
        public SBroadcastMessage() : base(Command.S_BROADCAST_MESSAGE) { }
        public string message;
    }
}
