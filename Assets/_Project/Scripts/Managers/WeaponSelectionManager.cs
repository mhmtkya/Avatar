using System;
using System.Collections.Generic;
using _Project.Scripts.UI;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class WeaponSelectionManager : MonoBehaviour
    {
        public static WeaponSelectionManager Instance;

        [Header("UI Elements")] 
        public GameObject weaponSelectionCanvas;
        public LevelUpCard[] weaponCards;
        
        [Header("Database")]
        public List<WeaponData> allWeaponsInGame;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            weaponSelectionCanvas.SetActive(false);
        }

        public void ShowWeaponMenu()
        {
            Time.timeScale = 0f;
            weaponSelectionCanvas.SetActive(true);

            List<WeaponData> availablePool = new List<WeaponData>();

            if (WeaponInventoryManager.Instance.HasEmptySlot())
            {
                availablePool.AddRange(allWeaponsInGame);
                
            }
            else
            {
                availablePool.AddRange(WeaponInventoryManager.Instance.equippedWeapons);
            }

            foreach (LevelUpCard card in weaponCards)
            {
                if (availablePool.Count == 0)
                {
                    card.gameObject.SetActive(false);
                    continue;
                }
                
                card.gameObject.SetActive(true);
                
                int randomIndex = UnityEngine.Random.Range(0, availablePool.Count);
                WeaponData selectedWeapon = availablePool[randomIndex];
                
                card.SetupWeaponCard(selectedWeapon);
                availablePool.RemoveAt(randomIndex);
            }
        }

        public void SelectWeapon(WeaponData chosenWeapon)
        {
            WeaponInventoryManager.Instance.AddWeapon(chosenWeapon);
            weaponSelectionCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}