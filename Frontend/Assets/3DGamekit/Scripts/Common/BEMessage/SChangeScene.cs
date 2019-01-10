using System;

namespace Common
{
    [Serializable]
    public class SChangeScene : Message
    {
        public SChangeScene() : base(Command.S_CHANGE_SCENE) { }
        public string level;
    }
}
