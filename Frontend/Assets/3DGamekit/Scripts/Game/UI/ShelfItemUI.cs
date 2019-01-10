using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using UnityEngine.EventSystems;

public class ShelfItemUI : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
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

    public AllItemInfoUI allItemInfoUI;

    private Dictionary<string, string> itemInfoString = null;
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
    public void OnMouseEnter()
    {
     //   allItemInfoUI.SetItemInfo("", "", "", "", "", button.image.sprite, itemConf.name, itemConf.type.ToString(), "", false, "5", gameObject.transform.position);
    }

    private void OnMouseOver()
    {

    }

    private void OnMouseExit()
    {
        
    }
    private void Awake()
    {
        if (cartContent != null)
        {
            handler = cartContent.GetComponent<CartGridUI>();
        }
       // allItemInfoUI = GameObject.FindObjectOfType<AllItemInfoUI>();
    }
    // Use this for initialization
    void Start()
    {
     //   allItemInfoUI = GameObject.FindObjectOfType<AllItemInfoUI>();
        
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
        itemName = name;
        textCost.text = "$5";
        allItemInfoUI = GameObject.FindObjectOfType<AllItemInfoUI>();
    }

    public void AddToCart()
    {
        //if (handler != null)
        //    handler.AddToCart(icon_name);
        if (handler != null)
            handler.AddToCart(itemConf, itemCost);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (itemInfoString == null)
        {
            itemInfoString = FrontEnd.Item.FItem.SpecificItemString(itemType, FrontEnd.World.Instance.fPlayer.intelligence);
            itemInfoString["info"] = "";
            if (itemType == ItemType.Others || textName.text == "HealthElixir")
            {
                itemInfoString["health"] = "";
                itemInfoString["speed"] = "";
                itemInfoString["intelligence"] = "";
                itemInfoString["defence"] = "";
                itemInfoString["damage"] = "";
            }
            if (itemType == ItemType.Others)
                itemInfoString["info"] = "* 50";
            if (textName.text == "HealthElixir")
                itemInfoString["info"] = "恢复所有HP";
        }

        allItemInfoUI.SetItemInfo(
            itemInfoString["health"],
            itemInfoString["damage"],
            itemInfoString["defence"],
            itemInfoString["intelligence"],
            itemInfoString["speed"],
            button.image.sprite,
            textName.text,
            itemType.ToString(),
            itemInfoString["info"],
            itemCost.costType == CostType.Gold,
            itemCost.cost.ToString(),
            eventData.position,
            "mall" + textName.text);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var rect_trans = GetComponentInParent<RectTransform>();
        var _pos = Input.mousePosition;
        if (rect_trans.rect.Contains(eventData.position) || rect_trans.rect.Contains(new Vector2(_pos.x, _pos.y)))
            return;
        allItemInfoUI.ResetActive(eventData.position);
    }
}
