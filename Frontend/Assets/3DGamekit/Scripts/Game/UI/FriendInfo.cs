using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gamekit3D;

public class FriendInfo : MonoBehaviour
{
    //public Component friendButton;
    public void OnClickFunc()
    {
        string friendName = this.GetComponentInChildren<Text>().text;
        var chatWindow = GameObject.FindObjectOfType<ChatUI>();
        Debug.Log("Click FrindInfo");
        if (chatWindow == null)
            Debug.Log("Can't find chatWindow");
        chatWindow.setFriendName(friendName);
        
    }
    public void Start()
    {
        Debug.Log(this.name);
        //var friendButton = this.GetComponent<Button>();
        //Debug.Log(friendButton.name);
        //friendButton.onClick.AddListener(delegate() {
        //    Debug.Log("FUCKKKKKKKKK");
        //    var chatWindow = GameObject.Find("MessageContent");
        //    chatWindow.SetActive(true);
        //    OnClickFunc();
        //});
        //Debug.Log(friendButton.name);
    }
}