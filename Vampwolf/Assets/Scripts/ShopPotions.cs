using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vampwolf
{
    public class ShopPotions : MonoBehaviour
    {
        public int potionCost = 10;
        public Button button;

        public void BuyPotion()
        {
            var inv = InventoryManager.Instance;
            if (inv.coins >= potionCost)
            {
                inv.coins -= potionCost;
                inv.AddItem("HealthPotion");
                Debug.Log("Bought health potion. Total: " + inv.healthPotions);
            }
            else
            {
                Debug.Log("Not enough coins to buy a potion.");
            }
        }
    }
}
