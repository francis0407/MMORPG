using System;

namespace Common
{
    [Serializable]
    public class SOtherPlayerExit : Message
    {
        public SOtherPlayerExit() : base(Command.S_OTHER_PLAYER_EXIT) { }
        public string user;
        public int id;
        public string scene;

    }
}
