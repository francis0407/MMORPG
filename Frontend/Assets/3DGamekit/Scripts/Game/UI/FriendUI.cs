using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit3D;
using FrontEnd;
public class FriendUI : MonoBehaviour
{
    public GameObject FriendInfo;
    private List<GameObject> OnlinePlayers = new List<GameObject>();
    //private Dictionary<string, GameObject> OnlinePlayers = new Dictionary<string, GameObject>();

    //public void AddPlayer(string name)
    //{
    //    GameObject cloned = GameObject.Instantiate(FriendInfo);
    //    cloned.transform.SetParent(transform, false);
    //    cloned.SetActive(true);
    //    var text = cloned.GetComponentInChildren<UnityEngine.UI.Text>();
    //    text.text = name;
    //    OnlinePlayers.Add(name, cloned);
    //}

    //public void RemovePlayer(string name)
    //{
    //    OnlinePlayers.Remove(name);
    //}
    
    private void Awake()
    {
        FriendInfo.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
       // Debug.Log(this.name);
        Debug.Log("Start FriendUI");
       // Test();
    }

    private void OnEnable()
    {
        //x++;
        //for (int i = 0; i < 5; i++)
        //{
        //    GameObject cloned = GameObject.Instantiate(FriendInfo);
        //    cloned.transform.SetParent(transform, false);
        //    cloned.SetActive(true);
        //    var text = cloned.GetComponentInChildren<UnityEngine.UI.Text>();
        //    text.text = i.ToString();
        //    //OnlinePlayers.Add(cloned);
        //}
        var players = WorldPlayers.Instance.players;
        foreach(var player in players)
        {
            GameObject cloned = GameObject.Instantiate(FriendInfo);
            cloned.transform.SetParent(transform, false);
            cloned.SetActive(true);
            var text = cloned.GetComponentInChildren<UnityEngine.UI.Text>();
            text.text = player.Key;
            OnlinePlayers.Add(cloned);
        }
        Debug.Log("Enable FriendUI");
        PlayerMyController.Instance.EnabledWindowCount++;
    }

    private void OnDisable()
    {
        foreach (var obj in OnlinePlayers)
            obj.SetActive(false);
        OnlinePlayers.Clear();
        Debug.Log("Disable FriendUI");
        PlayerMyController.Instance.EnabledWindowCount--;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update");
    }

    void Test()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject cloned = GameObject.Instantiate(FriendInfo);
            cloned.transform.SetParent(transform, false);
            cloned.SetActive(true);
            cloned.name = i.ToString();
        }
    }
}
