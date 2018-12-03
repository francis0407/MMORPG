using System;

namespace Common
{
    [Serializable]
    public class CChatMessage : Message
    {
        public CChatMessage() : base(Command.C_CHAT_MESSAGE) { }
        public string message;
        public int from;
        public int to;
    }
}
