using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamekit3D;
using FrontEnd;
using Common;
public class MarketItemUI : MonoBehaviour
{
    public GameObject ItemName;
    public GameObject ItemCost;
    public GameObject ItemType;
  
    public void Set(string name, int cost, CostType costType)
    {
        ItemName.GetComponent<Text>().text = name;
        ItemCost.GetComponent<Text>().text = cost.ToString();
        if (ItemType != null)   
            ItemType.GetComponent<Image>().color = new Color(1, 1, costType == CostType.Gold ? 0 : 1);
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
        
    }
}
