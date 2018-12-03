using System;

namespace Common
{
    [Serializable]
    public class SChatMessage : Message
    {
        public SChatMessage() : base(Command.S_CHAT_MESSAGE) { }
        public string message;
        public int from;
        public int to;
    }
}
