using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamekit3D;
using Common;
using FrontEnd;
using FrontEnd.Item;
using Gamekit3D.Network;
public class CartGridUI : MonoBehaviour
{
    public GameObject CartItem;

    private Dictionary<string, GameObject> m_items = new Dictionary<string, GameObject>();

    private void Awake()
    {
        CartItem.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToCart(string name)
    {
        Sprite sprite;
        GameObject item;
        if (!GetAllIcons.icons.TryGetValue(name, out sprite))
        {
            return;
        }
        bool exists = m_items.TryGetValue(name, out item);
        if (!exists)
        {
            item = GameObject.Instantiate(CartItem);
            if (item == null)
            {
                return;
            }
            item.transform.SetParent(transform, false);
            item.SetActive(true);
            m_items.Add(name, item);
        }
        CartItemUI handler = item.GetComponent<CartItemUI>();
        if (handler == null)
        {
            return;
        }

        if (exists)
        {
            handler.Increase();
        }
        else
        {
            handler.Init(name);
        }
    }

    public void AddToCart(ItemConf itemConf, CostConf itemCost)
    {
        Sprite sprite;
        GameObject item;
        
        if (!GetAllIcons.icons.TryGetValue(itemConf.icon, out sprite))
            return;
        bool exists = m_items.TryGetValue(itemConf.name, out item);
        if (!exists)
        {
            // new cart item
            item = GameObject.Instantiate(CartItem);
            if (item == null)
            {
                return;
            }
            item.transform.SetParent(transform, false);
            item.SetActive(true);
            m_items.Add(itemConf.name, item);
        }
        CartItemUI handler = item.GetComponent<CartItemUI>();
        if (exists)
            handler.Increase();
        else
            handler.Init(itemConf, itemCost);
    }

    public void RemoveFromCart(string name)
    {
        GameObject item;
        if (m_items.TryGetValue(name, out item))
        {
            m_items.Remove(name);
            Destroy(item);
        }
    }

    public void OnBuyButtonClicked()
    {
        CBuyTmallItems msg = new CBuyTmallItems();
        msg.luck = World.Instance.fPlayer.intelligence;
        List<TmallItem> items = new List<TmallItem>();
        int gold_cost = 0;
        int silver_cost = 0;
        int item_count = 0;
        foreach (var kv in m_items)
        {
            var cartItem = kv.Value.GetComponent<CartItemUI>();
            //cartItem.item
            Debug.Log(string.Format("Buy {0} * {1}, Cost {2} {3}", cartItem.item.name, cartItem.Count, cartItem.cost.cost * cartItem.Count, cartItem.cost.costType.ToString()));
            TmallItem item = new TmallItem();
            item.itemConf = cartItem.item;
            item.costConf = cartItem.cost;
            item.count = cartItem.Count;
            items.Add(item);
            Destroy(kv.Value);

            if (item.costConf.costType == CostType.Silver)
                silver_cost += item.count * item.costConf.cost;
            else
                gold_cost += item.count * item.costConf.cost;
            if (item.itemConf.type != ItemType.Others)
                item_count += item.count;
        }
        m_items.Clear();
        msg.tmallItems = items.ToArray();
        if (!(gold_cost <= World.Instance.fPlayer.gold && silver_cost <= World.Instance.fPlayer.silver))
        {
            MessageBox.Show("Can't Afford that!");
            return;
        }
        if (World.Instance.fPlayer.inventory.Count + item_count <= 40)
        {
            MessageBox.Show("Inventory is FULL!");
            return;
        }
        Client.Instance.Send(msg);
        
    }

}
