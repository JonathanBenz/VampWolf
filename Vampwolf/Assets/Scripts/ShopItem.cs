using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampwolf
{
public class ShopItem : MonoBehaviour
{
    public string itemName;
    public int cost;

    public TextMeshProUGUI itemLabel;
    public Button buyButton;

    private void Start()
    {
        itemLabel.text = $"{itemName} - {cost} Coins";
        buyButton.onClick.AddListener(BuyItem);
    }

    void BuyItem()
    {
        if (InventoryManager.Instance.TryPurchase(itemName, cost))
        {
            Debug.Log($"Purchased {itemName}");
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }
}
}
