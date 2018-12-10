using System;
using UnityEngine;
using UnityEngine.UI;
using FrontEnd.Item;
using FrontEnd;
public class ItemInfoUI : MonoBehaviour
{
    static public ItemInfoUI itemInfoUI;
    public GameObject itemName;
    public GameObject healthValue;
    public GameObject speedValue;
    public GameObject damageValue;
    public GameObject intelligenceValue;
    public GameObject defenceValue;
    public GameObject silverValue;

    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    public GameObject self;

    public int item_id;
    void Awake()
    {
        self = GameObject.Find("ItemInfo");
        itemInfoUI = this;
    }

    public void ChangeItem(FItem item, bool equip)
    {
        item_id = item.item_id;
        itemName.GetComponent<Text>().text = item.name;
        healthValue.GetComponent<Text>().text = "+" + item.health_value.ToString();
        speedValue.GetComponent<Text>().text = "+" + item.speed_value.ToString();
        damageValue.GetComponent<Text>().text =  "+" + item.damage_value.ToString();
        intelligenceValue.GetComponent<Text>().text = "+" + item.intelligence_value.ToString();
        defenceValue.GetComponent<Text>().text = "+" + item.defence_value.ToString();
        silverValue.GetComponent<Text>().text = item.silver_value.ToString();
        if (equip)
        {
            equipButton.SetActive(true);
            equipButton.GetComponentInChildren<Text>().text = item.item_type == Common.ItemType.Elixir ? "使用" : "装备";
            equipButton.GetComponent<Button>().onClick.RemoveAllListeners();
            equipButton.GetComponent<Button>().onClick.AddListener(delegate() {
                // equip item
                if (item.item_type == Common.ItemType.Elixir)
                    World.Instance.fPlayer.SendUseItem(item_id);
                else
                    World.Instance.fPlayer.SendEquipItem(item_id);
                // GameObject.FindObjectOfType<RoleUI>().RefreshAttr();
                self.SetActive(false);
            });


            dropButton.SetActive(true);
            dropButton.GetComponent<Button>().onClick.RemoveAllListeners();
            dropButton.GetComponent<Button>().onClick.AddListener(delegate () {
                // drop item
                World.Instance.fPlayer.SendDropItem(item_id);
                self.SetActive(false);
                //GameObject.FindObjectOfType<RoleUI>().RefreshAttr();
            });
            unEquipButton.SetActive(false);
        }
        else
        {
            equipButton.SetActive(false);
            dropButton.SetActive(false);
            unEquipButton.SetActive(true);
            unEquipButton.GetComponent<Button>().onClick.RemoveAllListeners();
            unEquipButton.GetComponent<Button>().onClick.AddListener(delegate () {
                // Unequip item
                World.Instance.fPlayer.SendUnEquipItem(item_id);
                self.SetActive(false);
                //GameObject.FindObjectOfType<RoleUI>().RefreshAttr();
            });
        }

        Debug.Log(string.Format("Focus on item {0}", item.name));
        self.SetActive(true);
        //this.a
    }
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }



}
