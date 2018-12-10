using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamekit3D.Network;
using Common;
public class ShelfGridUI : MonoBehaviour
{
    public GameObject ShelfItem;
    public GameObject GoldText;
    public GameObject SilverText;
    public void GetAllItems()
    {
        var items = FrontEnd.World.Instance.TmallItems;

        foreach (var kv in items)
        {
            var cloned = GameObject.Instantiate(ShelfItem);
            cloned.transform.SetParent(this.transform, false);
            ShelfItemUI handler = cloned.GetComponent<ShelfItemUI>();
            //handler.SetInfo(kv.Key.name, kv.Key.icon, kv.Key.type, kv.Value);
            handler.SetInfo(kv.Key, kv.Value);
        }
        ShelfItem.SetActive(false);
    }
    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
        CGetTmallItems msg = new CGetTmallItems();
        Client.Instance.Send(msg);
        GoldText.GetComponent<Text>().text = FrontEnd.World.Instance.fPlayer.gold.ToString();
        //foreach (KeyValuePair<string, Sprite> kv in GetAllIcons.icons)
        //{
        //    string key = kv.Key;
        //    GameObject cloned = GameObject.Instantiate(ShelfItem);
        //    if (cloned == null)
        //    {
        //        continue;
        //    }
        //    cloned.SetActive(true);
        //    cloned.transform.SetParent(this.transform, false);
        //    ShelfItemUI handler = cloned.GetComponent<ShelfItemUI>();
        //    if (handler == null)
        //    {
        //        continue;
        //    }
        //    handler.Init(key);
        //}
        //ShelfItem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GoldText.GetComponent<Text>().text = FrontEnd.World.Instance.fPlayer.gold.ToString();
        SilverText.GetComponent<Text>().text = FrontEnd.World.Instance.fPlayer.silver.ToString();
    }
}
