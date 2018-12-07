using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gamekit3D;
using FrontEnd;
using Common;
public class RoleUI : MonoBehaviour
{

    public TextMeshProUGUI HPValue;
    public TextMeshProUGUI InteligenceValue;
    public TextMeshProUGUI SpeedValue;
    public TextMeshProUGUI LevelValue;
    public TextMeshProUGUI AttackValue;
    public TextMeshProUGUI DefenseValue;

    public GameObject Helmet;
    public GameObject Armour;
    public GameObject LeftWeapon;
    public GameObject RightWeapon;
    public GameObject Shoes;
    public GameObject Legging;

    public Sprite IconBackGround;

    private Damageable m_damageable;
    private PlayerController m_controller;

    public Dictionary<ItemType, GameObject> WearingObjects;
    public void RefreshAttr()
    {
        var player = World.Instance.fPlayer;
        player.refreshAttr();
        HPValue.text = player.health.ToString();
        InteligenceValue.text = player.intelligence.ToString();
        SpeedValue.text = player.speed.ToString();
        AttackValue.text = player.damage.ToString();
        DefenseValue.text = player.defence.ToString();
    }

    public void RefreshAll()
    {
        RefreshAttr();
        var player = World.Instance.fPlayer;
        foreach(var item in WearingObjects)
        {
            item.Value.GetComponent<Button>().onClick.RemoveAllListeners();
            item.Value.GetComponent<Image>().sprite = IconBackGround;
        }
        foreach(var item in player.wearing)
        {
            var icon = item.Value.icon_name;
            var obj = WearingObjects[item.Value.item_type];
            obj.GetComponent<Image>().sprite = GetAllIcons.icons[icon];
            obj.GetComponent<Button>().onClick.RemoveAllListeners();
            obj.GetComponent<Button>().onClick.AddListener(delegate(){
                //var itemInfoUI = GameObject.FindObjectOfType<ItemInfoUI>();
                var obj2 = GameObject.Find("ItemInfo");
                obj2.SetActive(true);
                ItemInfoUI.itemInfoUI.ChangeItem(item.Value, false);
            });
        }
    }
    private void Awake()
    {
        WearingObjects = new Dictionary<ItemType, GameObject>()
        {
            {ItemType.Armour, Armour},
            {ItemType.Helmet, Helmet},
            {ItemType.Leftweapon, LeftWeapon},
            {ItemType.Rightweapon, RightWeapon},
            {ItemType.Shoes, Shoes},
            {ItemType.Legging, Legging}
        };
        Debug.Log("Wake roleUI");
    }
    // Use this for initialization
    void Start()
    {

    }

    private void OnEnable()
    {
        PlayerMyController.Instance.EnabledWindowCount++;
        if (m_controller == null || m_damageable == null)
        {
            m_controller = PlayerController.Mine;
            m_damageable = PlayerController.Mine.GetComponent<Damageable>();
        }
        string hp = string.Format("{0}/{1}", m_damageable.currentHitPoints, m_damageable.maxHitPoints);
        HPValue.SetText(hp, true);
        RefreshAll();
    }

    private void OnDisable()
    {
        PlayerMyController.Instance.EnabledWindowCount--;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Test()
    {
        HPValue.text = "100";
        InteligenceValue.text = "100";
    }

    public void InitUI(PlayerController controller)
    {
        m_damageable = controller.GetComponent<Damageable>();
        m_controller = controller;
    }
}
