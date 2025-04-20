using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf
{
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int coins = 200;
    public int healthPotions = 0; //health potions is the only thing the player can have multiple of
    public bool hasVampItem = false;
    public bool hasWolfItem = false;

    void Awake()
    {
        //Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasItem(string itemName)
    {
        return itemName switch
        {
            "VampItem" => hasVampItem,
            "WolfItem" => hasWolfItem,
            _ => false
        };
    }

    public void AddItem(string itemName)
    {
        switch (itemName)
        {
            case "VampItem": hasVampItem = true; break;
            case "WolfItem": hasWolfItem = true; break;
            case "HealthPotion": healthPotions++; break;
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }
}
}
