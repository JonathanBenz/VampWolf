using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampwolf
{
public class ShopItem : MonoBehaviour
{
    public string itemName; // e.g. "VampItem"
    public int itemCost = 25;
    public Button button;

    void Start()
    {
        if (InventoryManager.Instance.HasItem(itemName))
        {
            button.gameObject.SetActive(false);
        }
    }

    public void BuyItem()
    {
        var inv = InventoryManager.Instance;
        if (inv.coins >= itemCost && !inv.HasItem(itemName))
        {
            inv.coins -= itemCost;
            inv.AddItem(itemName);
            button.gameObject.SetActive(false);
            Debug.Log("Bought " + itemName);
        }
        else
        {
            Debug.Log("Not enough coins or already owned.");
        }
    }
}
}
