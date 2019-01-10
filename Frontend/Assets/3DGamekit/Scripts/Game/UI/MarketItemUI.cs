using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamekit3D;
using FrontEnd.Item;
using UnityEngine.EventSystems;
using Common;
public class MarketItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ItemName;
    public GameObject ItemCost;
    public GameObject ItemType;
    private FItem item;
    public AllItemInfoUI allItemInfoUI;
    private CostConf costConf;
    bool infoActive = false;
    int myTag;
    public void Set(string name, int cost, CostType costType)
    {
        ItemName.GetComponent<Text>().text = name;
        ItemCost.GetComponent<Text>().text = cost.ToString();
        if (ItemType != null)   
            ItemType.GetComponent<Image>().color = new Color(1, 1, costType == CostType.Gold ? 0 : 1);
        costConf = new CostConf();
        costConf.cost = cost;
        costConf.costType = costType;
    }

    public void SetItem(FItem _item, int i)
    {
        item = _item;
        myTag = i;
    }
    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (infoActive)
        {

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoActive = true;
        allItemInfoUI.SetItemInfo(
            item.health_value.ToString(),
            item.damage_value.ToString(),
            item.defence_value.ToString(),
            item.intelligence_value.ToString(),
            item.speed_value.ToString(),
            gameObject.GetComponentInChildren<UnityEngine.UI.Button>().image.sprite,
            item.name,
            item.item_type.ToString(),
            "",
            costConf.costType == CostType.Gold,
            costConf.cost.ToString(),
            eventData.position,
            "market" + myTag.ToString());
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
