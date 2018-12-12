using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamekit3D;
using FrontEnd;
using Common;
public class MarketBuyViewUI : MonoBehaviour
{

    public GameObject BuyShelfItem;
    public GameObject BuyButton;
    public GameObject ShelfGridContent;
    public GameObject ItemImg;
    public GameObject ItemNameValue;
    public GameObject ItemTypeValue;
    public GameObject ItemHealthValue;
    public GameObject ItemSpeedValue;
    public GameObject ItemIntelligenceValue;
    public GameObject ItemDamageValue;
    public GameObject ItemDefenceValue;
    public GameObject ItemPriceValue;
    public GameObject ItemPriceImg;
    public GameObject ItemInfo;
    private int ItemId = -1;
    private CostType priceType = CostType.Silver;
    private void Awake()
    {
        ItemInfo.SetActive(false);
        BuyShelfItem.SetActive(false);
        BuyButton.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (!World.Instance.MarketItems.ContainsKey(ItemId))
                return;
            var item = World.Instance.MarketItems[ItemId];
            var gold = World.Instance.fPlayer.gold;
            var silver = World.Instance.fPlayer.silver;
            if (item.costConf.costType == CostType.Gold)
            {
                if (gold < item.costConf.cost)
                {
                    MessageBox.Show("Can't Afford that.");
                    return;
                }
            }
            else
            {
                if (silver < item.costConf.cost)
                {
                    MessageBox.Show("Can't Afford that.");
                    return;
                }
            }
            CBuyMarketItem msg = new CBuyMarketItem();
            msg.item = item;
            Gamekit3D.Network.Client.Instance.Send(msg);

            ItemId = -1;
            ItemInfo.SetActive(false);
        });
    }

    public void RefreshItems()
    {
        int cellCount = ShelfGridContent.transform.childCount;
        foreach (Transform transform in ShelfGridContent.transform)
            Destroy(transform.gameObject);
        var marketItems = World.Instance.MarketItems;
        if (marketItems == null)
            return;
        foreach (var kv in marketItems)
        {
            GameObject cloned = GameObject.Instantiate(BuyShelfItem);
            Button button = cloned.GetComponentInChildren<Button>();
            Sprite icon = GetAllIcons.icons[kv.Value.ditem.icon_name];
            button.image.sprite = icon;
            cloned.GetComponent<MarketItemUI>().Set(kv.Value.ditem.name, kv.Value.costConf.cost, kv.Value.costConf.costType);
            button.onClick.AddListener(delegate () {
                ItemInfo.SetActive(true);
                ItemId = kv.Key;
                ItemImg.GetComponent<Image>().sprite = icon;
                ItemNameValue.GetComponent<Text>().text = kv.Value.ditem.name;
                ItemTypeValue.GetComponent<Text>().text = kv.Value.ditem.item_type.ToString();
                ItemHealthValue.GetComponent<Text>().text = kv.Value.ditem.health_value.ToString();
                ItemSpeedValue.GetComponent<Text>().text = kv.Value.ditem.speed_value.ToString();
                ItemIntelligenceValue.GetComponent<Text>().text = kv.Value.ditem.intelligence_value.ToString();
                ItemDamageValue.GetComponent<Text>().text = kv.Value.ditem.damage_value.ToString();
                ItemDefenceValue.GetComponent<Text>().text = kv.Value.ditem.defence_value.ToString();
                ItemPriceImg.GetComponent<Image>().color = new Color(1, 1, kv.Value.costConf.costType==CostType.Silver? 1:0);
                priceType = kv.Value.costConf.costType;
                ItemPriceValue.GetComponent<Text>().text = kv.Value.costConf.cost.ToString();
            });
            cloned.SetActive(true);
            cloned.transform.SetParent(ShelfGridContent.transform, false);
        }
    }

    private void OnEnable()
    {
        foreach (Transform transform in ShelfGridContent.transform)
            Destroy(transform.gameObject);
        CGetMarketItems msg = new CGetMarketItems();
        Gamekit3D.Network.Client.Instance.Send(msg);
    }

    private void OnDisable()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
