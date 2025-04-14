using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInput;

namespace Vampwolf.Input
{
    public interface IInputReader
    {
        void EnablePlayerActions();
        void DisablePlayerActions();
    }

    [CreateAssetMenu(fileName = "New Input Reader", menuName = "Player Input")]
    public class InputReader : ScriptableObject, IPlayerActions, IInputReader
    {
        public event UnityAction<Vector2> Look = delegate { };
        public event UnityAction<bool> Select = delegate { };
        public event UnityAction<bool> UseAbility1 = delegate { };
        public event UnityAction<bool> UseAbility2 = delegate { };
        public event UnityAction<bool> UseAbility3 = delegate { };

        public PlayerInput inputActions;

        public Vector2 MousePos => inputActions.Player.Look.ReadValue<Vector2>();

        private bool ability1Status;
        private bool ability2Status;
        private bool ability3Status;

        public void EnablePlayerActions()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerInput();
                inputActions.Player.SetCallbacks(this);
            }

            inputActions.Enable();
        }

        public void DisablePlayerActions()
        {
            inputActions.Disable();

            ability1Status = false;
            ability2Status = false;
            ability3Status = false;
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Started:
                    Select.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Select.Invoke(false);
                    break;
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>());
        }

        public void OnUseAbility1(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                // If button was already pressed before and is pressed again, cancel out
                if (ability1Status) ability1Status = false;
            
                // Else, ability1Status is true and all other ability status' are false
                else
                {
                    ability1Status = true;
                    ability2Status = false;
                    ability3Status = false;
                }
                UseAbility1.Invoke(ability1Status);
            }
        }

        public void OnUseAbility2(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                // If button was already pressed before and is pressed again, cancel out
                if (ability2Status) ability2Status = false;

                // Else, ability2Status is true and all other ability status' are false
                else
                {
                    ability1Status = false;
                    ability2Status = true;
                    ability3Status = false;
                }
                UseAbility2.Invoke(ability2Status);
            }
        }

        public void OnUseAbility3(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                // If button was already pressed before and is pressed again, cancel out
                if (ability3Status) ability3Status = false;

                // Else, ability3Status is true and all other ability status' are false
                else
                {
                    ability1Status = false;
                    ability2Status = false;
                    ability3Status = true;
                }
                UseAbility3.Invoke(ability3Status);
            }
        }
    }
}
