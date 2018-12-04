using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontEnd
{
    public class ChatEntry
    {
        public ChatEntry(bool _self, string _message) { self = _self; message = _message; }
        public bool self;
        public string message;
    }

    // singleton class to store player information
    class World : Singleton<World>
    {
        public Dictionary<string, int> players = new Dictionary<string, int>();
        public int selfId;
        public Dictionary<int, List<ChatEntry>> chatHistory = new Dictionary<int, List<ChatEntry>>();
        public FPlayer fPlayer = new FPlayer();


    }
}
