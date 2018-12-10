using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
public class ShelfItemUI : MonoBehaviour
{
    public string itemName;
    public GameObject cartContent;
    public GameObject CostImg;
    public Button button;
    public Text textName;
    public Text textCost;
    CartGridUI handler;

    public ItemType itemType;
    public string icon_name;

    public ItemConf itemConf;
    public CostConf itemCost;
    public void SetInfo(string name, string icon, Common.ItemType type, int cost)
    {
        textName.text = name;
        textCost.text = cost.ToString();
        itemType = type;
        icon_name = icon;
        button.image.sprite = GetAllIcons.icons[icon];

    }

    public void SetInfo(ItemConf item, CostConf cost)
    {
        itemConf = item;
        itemCost = cost;
        CostImg.GetComponent<Image>().color = new Color(1, 1, cost.costType == CostType.Silver ? 1 : 0);

        SetInfo(item.name, item.icon, item.type, cost.cost);
    }

    private void Awake()
    {
        if (cartContent != null)
        {
            handler = cartContent.GetComponent<CartGridUI>();
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(string name)
    {
        itemName = name;
        Sprite sprite;
        if (button == null || textName == null || textCost == null)
        {
            return;
        }
        if (!GetAllIcons.icons.TryGetValue(name, out sprite))
        {
            return;
        }
        button.image.sprite = sprite;
        textName.text = name;
        textCost.text = "$5";
    }

    public void AddToCart()
    {
        //if (handler != null)
        //    handler.AddToCart(icon_name);
        if (handler != null)
            handler.AddToCart(itemConf, itemCost);
    }
}
