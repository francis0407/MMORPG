using System;

namespace Common
{
    [Serializable]
    public class SOnlinePlayers : Message
    {
        public SOnlinePlayers() : base(Command.S_ONLINE_PLAYERS) { }
        public string[] users;
        public int[] ids;
        public string[] scenes;

    }
}
