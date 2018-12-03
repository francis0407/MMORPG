using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontEnd
{

public class WorldPlayers:Singleton<WorldPlayers>
{
    public Dictionary<string, int> players = new Dictionary<string, int>();
    public int selfId;
}

public class ChatEntry
{
    public ChatEntry(bool _self, string _message) { self = _self; message = _message; }
    public bool self;
    public string message;
}

public class ChatHistory:Singleton<ChatHistory>
{
    public Dictionary<int, List<ChatEntry>> history = new Dictionary<int, List<ChatEntry>>();
    
}

}

