using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Vampwolf
{
public class ShopUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Update()
    {
        coinText.text = $"Coins: {InventoryManager.Instance.coins}";
    }
}
}
