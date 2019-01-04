using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using FrontEnd.Item;
class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private FItem item;
    public AllItemInfoUI allItemInfoUI;

    public void SetItem(FItem _item)
    {
        item = _item;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        allItemInfoUI.SetItemInfo(
            item.health_value.ToString(),
            item.damage_value.ToString(),
            item.defence_value.ToString(),
            item.intelligence_value.ToString(),
            item.speed_value.ToString(),
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite,
            item.name,
            item.item_type.ToString(),
            "",
            false,
            item.silver_value.ToString(),
            eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var rect_trans = GetComponentInParent<RectTransform>();
        if (rect_trans.rect.Contains(eventData.position))
            return;
        allItemInfoUI.ResetActive(eventData.position);
    }
}

