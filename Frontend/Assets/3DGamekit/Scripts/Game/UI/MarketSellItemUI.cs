using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using FrontEnd.Item;
class MarketSellItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private FItem item;
    public AllItemInfoUI allItemInfoUI;
    int myTag;
    public void SetItem(FItem _item, int i)
    {
        item = _item;
        myTag = i;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
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
            false,
            item.silver_value.ToString(),
            eventData.position,
            "Sell"+myTag.ToString());
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

