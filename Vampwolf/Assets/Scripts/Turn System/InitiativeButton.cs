using UnityEngine;
using UnityEngine.UI;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Spells;

namespace Vampwolf
{
    public class InitiativeButton : MonoBehaviour
    {
        [SerializeField] private CharacterType characterType;

        private void Awake()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            EventBus<ShowSpells>.Raise(new ShowSpells()
            {
                CharacterType = characterType
            });
        }
    }
}
