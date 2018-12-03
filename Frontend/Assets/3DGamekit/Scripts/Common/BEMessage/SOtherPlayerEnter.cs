using System;

namespace Common
{
    [Serializable]
    public class SOtherPlayerEnter : Message
    {
        public SOtherPlayerEnter() : base(Command.S_OTHER_PLAYER_ENTER) { }
        public string user;
        public int id;
        public string scene;

    }
}
