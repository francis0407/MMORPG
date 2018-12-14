using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [Serializable]
    public class CHeartBeat : Message
    {
        public CHeartBeat() : base(Command.C_HEART_BEAT) { }
    }
}
