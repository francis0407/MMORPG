using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [Serializable]
    public class CPlayerExit : Message
    {
        public CPlayerExit() : base(Command.C_PLAYER_EXIT) { }
    }
}
