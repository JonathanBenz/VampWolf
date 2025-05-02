using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf
{
    [RequireComponent(typeof(Animator))]
    public class PlayAnimation : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<Animator>().Play("Base Layer.bloodSplatter", -1, 0f);
            Invoke("DisableAfterPlaying", 0.5f);
        }

        void DisableAfterPlaying()
        {
            this.gameObject.SetActive(false);
        }
    }
}
