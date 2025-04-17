using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf
{
public class HubManager : MonoBehaviour
{
    public GameObject hubCanvas;
    public GameObject shopCanvas;

    void Start()
    {
        //Start on the Hub Screen
        hubCanvas.SetActive(true);
        shopCanvas.SetActive(false);
    }
    public void OpenShop()
    {
        hubCanvas.SetActive(false);
        shopCanvas.SetActive(true);
    }

    public void CloseShop()
    {
        hubCanvas.SetActive(true);
        shopCanvas.SetActive(false);
    }
}
}
