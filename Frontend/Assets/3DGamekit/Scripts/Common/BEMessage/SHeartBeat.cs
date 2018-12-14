using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [Serializable]
    public class SHeartBeat : Message
    {
        public SHeartBeat() : base(Command.SBEGIN) { }
    }
}
