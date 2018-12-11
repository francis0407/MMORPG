using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamekit3D;
using FrontEnd;
public class MarketViewUI : MonoBehaviour
{
    public GameObject GoldText;
    public GameObject SilverText;
    public GameObject SwitchButton;
    public GameObject SwitchText;

    public GameObject BuyView;
    public GameObject SellView;

    public bool buying = false;
    private void Awake()
    {
    }

    private void OnEnable()
    {
        PlayerMyController.Instance.EnabledWindowCount++;
    }

    private void OnDisable()
    {
        PlayerMyController.Instance.EnabledWindowCount--;
    }

    // Use this for initialization
    void Start()
    {
        var self = this;
        SwitchButton.GetComponent<Button>().onClick.AddListener(delegate() {
            if (self.buying)
            {
                SellView.SetActive(true);
                BuyView.SetActive(false);
                self.buying = false;
                SwitchText.GetComponent<Text>().text = "咸鱼市场";
            }
            else
            {
                SellView.SetActive(false);
                BuyView.SetActive(true);
                self.buying = true;
                SwitchText.GetComponent<Text>().text = "我的背包";
            }

        });
        SellView.SetActive(false);
        BuyView.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        GoldText.GetComponent<Text>().text = World.Instance.fPlayer.gold.ToString();
        SilverText.GetComponent<Text>().text = World.Instance.fPlayer.silver.ToString();
    }
}
