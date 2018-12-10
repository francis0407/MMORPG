using System;
using UnityEngine;
using UnityEngine.UI;
using Common;
public class CartItemUI : MonoBehaviour
{
    public Button button;
    public Text textCost;
    public InputField inputCount;
    public GameObject CostImg;
    int count = 0;
    string itemName;

    public ItemConf item;
    public CostConf cost;
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int Count
    {
        get { return count;}
    }
    public void Init(string name)
    {
        Sprite sprite;
        if (button == null || textCost == null || textCost == null)
        {
            return;
        }
        if (!GetAllIcons.icons.TryGetValue(name, out sprite))
        {
            return;
        }
        itemName = name;
        count++;
        button.image.sprite = sprite;
        inputCount.text = System.Convert.ToString(count);
        textCost.text = "$5";
    }

    public void Init(ItemConf itemConf, CostConf itemCost)
    {
        item = itemConf;
        cost = itemCost;
        itemName = itemConf.name;
        button.image.sprite = GetAllIcons.icons[item.icon];
        CostImg.GetComponent<Image>().color = new Color(1, 1, cost.costType == CostType.Silver ? 1 : 0);
        Increase();
    }

    public void Increase()
    {
        count++;
        inputCount.text = System.Convert.ToString(count);
        //textCost.text = "$5";
        textCost.text = (count * cost.cost).ToString();
    }

    public void Decrease()
    {
        count--;
        if (count == 0)
        {
            if (transform.parent == null)
            {
                return;
            }
            CartGridUI gridHandler = transform.parent.GetComponent<CartGridUI>();
            if (gridHandler != null)
            {
                gridHandler.RemoveFromCart(itemName);
            }
        }
        else
        {
            inputCount.text = System.Convert.ToString(count);
            //textCost.text = "$5";
            textCost.text = (count * cost.cost).ToString();
        }
    }

}
