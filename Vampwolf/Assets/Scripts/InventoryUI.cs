using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Only if you're using TextMeshPro

namespace Vampwolf
{
    public class InventoryUI : MonoBehaviour
    {
        public GameObject inventoryPanel;
        public TextMeshProUGUI coinsText;
        public TextMeshProUGUI potionsText;
        public TextMeshProUGUI itemsText;

        void OnEnable()
        {
            UpdateInventoryUI();
        }

        public void UpdateInventoryUI()
        {
            var inv = InventoryManager.Instance;

            coinsText.text = "Coins: " + inv.coins;
            potionsText.text = "Health Potions: " + inv.healthPotions;

            string items = "";
            if (inv.HasItem("VampItem")) items += "Vampire Item\n";
            if (inv.HasItem("WolfItem")) items += "Werewolf Item\n";
            if (items == "") items = "No special items";

            itemsText.text = "Items:\n" + items;
        }

        public void OpenInventory()
        {
            UpdateInventoryUI();
        }
    }
}
