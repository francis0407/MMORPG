using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
class BroadcastTextUI : MonoBehaviour
{
    public Queue<KeyValuePair<DateTime, string>> textQueue = new Queue<KeyValuePair<DateTime, string>>();
    KeyValuePair<DateTime, string> currentText;
    DateTime lastTime;

    public Text messageText;
    public Text timeText;
    public void AddNewMessage(string msg)
    {
        textQueue.Enqueue(new KeyValuePair<DateTime, string>(DateTime.Now, msg));
    }

    private void Awake()
    {
        lastTime = DateTime.Now;
        SetMessage("");
    }

    void SetMessage(string msg)
    {
        timeText.text = "";
        messageText.text = msg;
    }

    void SetMessage(DateTime time, string msg)
    {
        timeText.text = time.ToLongTimeString();
        messageText.text = msg;
    }

    private void Update()
    {
        if (DateTime.Now > lastTime.AddSeconds(3))
        {
            if (textQueue.Count == 0)
            {
                SetMessage("");
            }
            else
            {
                var kv = textQueue.Dequeue();
                lastTime = DateTime.Now;
                SetMessage(kv.Key, kv.Value);
            }
        }
    }
}

