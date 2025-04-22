using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vampwolf
{
    public class MapLevelHandler : MonoBehaviour
    {
        GameObject castleButton;
        GameObject villageButton;
        
        void Start()
        {
            castleButton = transform.GetChild(1).gameObject;
            villageButton = transform.GetChild(2).gameObject;

            if (!ProgressTracker.Instance.level1Complete) LockLevel2();
            if (!ProgressTracker.Instance.level2Complete) LockLevel3();
        }

        void LockLevel2()
        {
            castleButton.GetComponent<Image>().color = Color.gray;
            castleButton.GetComponent<Button>().enabled = false;
        }

        void LockLevel3()
        {
            villageButton.GetComponent<Image>().color = Color.gray;
            villageButton.GetComponent<Button>().enabled = false;
        }
    }
}
