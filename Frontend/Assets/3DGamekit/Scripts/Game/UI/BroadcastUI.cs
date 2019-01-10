using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BroadcastUI : MonoBehaviour
{
    public Queue<string> messageQueue;

    Time lastTime;

    public void AddMessage(string _msg)
    {
        messageQueue.Enqueue(_msg);
    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }
}

