using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf
{
public class HubManager : MonoBehaviour
{
    public GameObject hubCanvas;
    public GameObject shopCanvas;
    public GameObject inventoryCanvas;

    void Start()
    {
        //Start on the Hub Screen
        hubCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
    }
    public void OpenShop()
    {
        hubCanvas.SetActive(false);
        shopCanvas.SetActive(true);
        inventoryCanvas.SetActive(false);
    }

    public void CloseShop()
    {
        hubCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
    }

    public void OpenInventory()
    {
        hubCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        inventoryCanvas.SetActive(true);
    }

    public void CloseInventory()
    {
        hubCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
    }
}
}
