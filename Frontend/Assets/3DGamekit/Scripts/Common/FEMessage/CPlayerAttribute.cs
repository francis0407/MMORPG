using System;

namespace Common
{
    // when a player finishing enter a scene
    [Serializable]
    public class CPlayerAttribute : Message
    {
        public CPlayerAttribute() : base(Command.C_PLAYER_ATTRIBUTE) { }

    }
}
