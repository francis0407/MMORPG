using System;

namespace Common
{
    [Serializable]
    public class SPlayerBeatMessage : Message
    {
        public SPlayerBeatMessage() : base(Command.S_PLAYER_BEAT) { }
        public bool win;
        public int award;
    }
}
