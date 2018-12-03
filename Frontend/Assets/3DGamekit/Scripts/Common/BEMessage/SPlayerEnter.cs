using System;

namespace Common
{
    [Serializable]
    public class SPlayerEnter : Message
    {
        public SPlayerEnter() : base(Command.S_PLAYER_ENTER) { }
        public string user;
        public string token;
        public int id;
        public string scene;
        public enum Status
        {
            Fail,
            Success,
            Error
        }
        public Status status;
        
    }
}
