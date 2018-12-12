using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamekit3D;
using FrontEnd;
using Common;
public class MarketSellViewUI : MonoBehaviour
{

    public GameObject SellShelfItem;
    public GameObject SellButton;
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
    public GameObject ItemPriceButton;
    public GameObject ItemInfo;
    private int ItemId = -1;
    private CostType priceType = CostType.Silver;
    private void Awake()
    {
        ItemInfo.SetActive(false);
        SellShelfItem.SetActive(false);
        SellButton.GetComponent<Button>().onClick.AddListener(delegate() {
            int price = System.Int32.Parse(ItemPriceValue.GetComponent<Text>().text);
            Debug.Log(string.Format("Sell {0} {1} {2}", this.ItemId, this.priceType, price));

            var inventory = World.Instance.fPlayer.inventory;
            if (!inventory.ContainsKey(ItemId))
                return;
            MarketItem item = new MarketItem();
            item.ditem = inventory[ItemId].ToDItem();
            item.costConf.costType = priceType;
            item.costConf.cost = price;
            item.owner_id = 0;
            CSellMarketItem msg = new CSellMarketItem();
            msg.item = item;
            Gamekit3D.Network.Client.Instance.Send(msg);

            ItemId = -1;
            ItemInfo.SetActive(false);
        });
        ItemPriceButton.GetComponent<Button>().onClick.AddListener(delegate() {
            if (this.priceType == CostType.Silver)
            {
                this.priceType = CostType.Gold;
                ItemPriceButton.GetComponent<Image>().color = new Color(1, 1, 0);
            }
            else
            {
                this.priceType = CostType.Silver;
                ItemPriceButton.GetComponent<Image>().color = new Color(1, 1, 1);
            }
        });

    }

    public void RefreshItems()
    {
        int cellCount = ShelfGridContent.transform.childCount;
        foreach (Transform transform in ShelfGridContent.transform)
        {
            Destroy(transform.gameObject);
        }

        var inventory = World.Instance.fPlayer.inventory;
        foreach (var kv in inventory)
        {
            GameObject cloned = GameObject.Instantiate(SellShelfItem);
            Button button = cloned.GetComponentInChildren<Button>();
            Sprite icon = GetAllIcons.icons[kv.Value.icon_name];
            button.image.sprite = icon;
            cloned.GetComponentInChildren<Text>().text = kv.Value.name;
            button.onClick.AddListener(delegate ()
            {
                ItemInfo.SetActive(true);
                ItemId = kv.Key;
                ItemImg.GetComponent<Image>().sprite = icon;
                ItemNameValue.GetComponent<Text>().text = kv.Value.name;
                ItemTypeValue.GetComponent<Text>().text = kv.Value.item_type.ToString();
                ItemHealthValue.GetComponent<Text>().text = kv.Value.health_value.ToString();
                ItemSpeedValue.GetComponent<Text>().text = kv.Value.speed_value.ToString();
                ItemIntelligenceValue.GetComponent<Text>().text = kv.Value.intelligence_value.ToString();
                ItemDamageValue.GetComponent<Text>().text = kv.Value.damage_value.ToString();
                ItemDefenceValue.GetComponent<Text>().text = kv.Value.defence_value.ToString();
                ItemPriceButton.GetComponent<Image>().color = new Color(1, 1, 1);
            });
            cloned.SetActive(true);
            cloned.transform.SetParent(ShelfGridContent.transform, false);
        }
    }

    private void OnEnable()
    {
        RefreshItems();
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
