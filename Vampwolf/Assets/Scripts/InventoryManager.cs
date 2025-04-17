using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf
{
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int coins = 200;
    public List<string> ownedItems = new List<string>();
    public int healthPotions = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public bool TryPurchase(string itemName, int cost)
    {
        if (coins >= cost)
        {
            coins -= cost;

            if (itemName == "Health Potion")
            {
                healthPotions++;
            }
            else
            {
                ownedItems.Add(itemName);
            }
            return true;
        }
        return false;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }
}
}
