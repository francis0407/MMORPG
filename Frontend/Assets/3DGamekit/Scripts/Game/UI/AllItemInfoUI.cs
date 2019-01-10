using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AllItemInfoUI : MonoBehaviour
{
    public Image Item_img;
    public Text health_value;
    public Text damage_value;
    public Text defence_value;
    public Text intelligence_value;
    public Text speed_value;

    public Text item_name;
    public Text item_type;
    public Text other_info;

    public bool isGold = false;
    public Image price_img;
    public Text price_value;

    public Sprite GoldImg;
    public Sprite SilverImg;

    public float UIWidth = 0;
    public float UIHeight = 0;

    DateTime activeTime;
    string myTag;

    private void Awake()
    {
            
    }

    public void OnEnable()
    {
        int count = transform.parent.childCount;
        transform.SetSiblingIndex(count - 1);
        activeTime = DateTime.Now;
    }

    public void SetItemInfo(
        string _health_value,
        string _damage_value,
        string _defence_value,
        string _intelligence_value,
        string _speed_value,
        Sprite _item_img,
        string _item_name,
        string _item_type,
        string _other_info,
        bool _isGold,
        string _price_value,
        Vector2 _mouse_pos,
        string tag)
    {
        //gameObject.SetActive(false);
        if (tag == myTag)
            return;
        else
            myTag = tag;
        health_value.text = _health_value;
        damage_value.text = _damage_value;
        defence_value.text = _defence_value;
        intelligence_value.text = _intelligence_value;
        speed_value.text = _speed_value;
        Item_img.sprite = _item_img;
        item_name.text = _item_name;
        item_type.text = _item_type;
        other_info.text = _other_info;
        price_img.color = new Color(1, 1, _isGold ? 0 : 1);
        price_value.text = _price_value;
        gameObject.transform.position = new Vector3(_mouse_pos.x - UIWidth / 2, _mouse_pos.y - UIHeight / 2, 0);
        gameObject.SetActive(true);
    }

    public void ResetPosition(Vector2 _mouse_pos)
    {
        gameObject.transform.position = new Vector3(_mouse_pos.x - UIWidth / 2, _mouse_pos.y - UIHeight / 2, 0);
    }

    public void ResetActive(Vector2 _mouse_pos)
    {
        if (activeTime.AddMilliseconds(100) > DateTime.Now)
            return;
        if (_mouse_pos.x >= transform.position.x - UIWidth / 2 && _mouse_pos.x <= transform.position.x + UIWidth / 2
            && _mouse_pos.y >= transform.position.y - UIHeight / 2 && _mouse_pos.y <= transform.position.y + UIHeight / 2)
        {
            return;
        }
        var _pos = Input.mousePosition;
        var rect_trans = this.GetComponentInParent<RectTransform>();
        if (rect_trans != null)
        {
            if (rect_trans.rect.Contains(_mouse_pos) || rect_trans.rect.Contains(new Vector2(_pos.x, _pos.y)))
                return;
        }
        Debug.Log(string.Format("{0} {1}", _mouse_pos.x, _mouse_pos.y));
        gameObject.SetActive(false);
        myTag = "";
    }
}

