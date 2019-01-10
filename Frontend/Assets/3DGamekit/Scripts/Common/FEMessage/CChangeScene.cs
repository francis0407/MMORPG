using System;

namespace Common
{
    [Serializable]
    public class CChangeScene : Message
    {
        public CChangeScene() : base(Command.C_CHANGE_SCENE) { }
        public int player_id;
        public string level;
    }
}
