using System;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Items
{
    public class WeaponAltar : MonoBehaviour
    {
        public InputActionReference interactAction;
        private bool isPlayerInRange = false;
        private bool isUsed = false;

        private void OnEnable()
        {
            if (interactAction != null)
            {
                interactAction.action.Enable();
                interactAction.action.performed += OnInteractPressed;
            }
        }

        private void OnDisable()
        {
            if (interactAction != null)
                interactAction.action.performed -= OnInteractPressed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                isPlayerInRange = false;
        }

        private void OnInteractPressed(InputAction.CallbackContext context)
        {
            if (!isUsed && isPlayerInRange)
            {
                isUsed = true;
                WeaponSelectionManager.Instance.ShowWeaponMenu();
                
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}