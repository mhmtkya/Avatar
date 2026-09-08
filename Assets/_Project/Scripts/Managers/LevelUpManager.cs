using _Project.Scripts.Data;
using _Project.Scripts.UI;
using UnityEngine;
using System.Collections.Generic;

namespace _Project.Scripts.Managers
{
    public class LevelUpManager : MonoBehaviour
    {
        public static LevelUpManager Instance;
        
        [Header("UI References")]
        public GameObject levelUpCanvas;
        public LevelUpCard[] cards;
        
        [Header("Database")]
        public List<UpgradeData> allAvailableUpgrades;
        
        private PlayerStats playerStats;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            levelUpCanvas.SetActive(false);
        }

        public void ShowLevelUpMenu(PlayerStats stats)
        {
            playerStats = stats;
            
            Time.timeScale = 0f;
            levelUpCanvas.SetActive(true);
            
            List<UpgradeData> tempUpgrades = new List<UpgradeData>(allAvailableUpgrades);
            
            foreach (LevelUpCard card in cards)
            {
                if (tempUpgrades.Count == 0)
                {
                    card.gameObject.SetActive(false);
                    continue;
                }
                
                card.gameObject.SetActive(true);
                
                int randomIndex = Random.Range(0, tempUpgrades.Count);
                UpgradeData selectedUpgrade = tempUpgrades[randomIndex];
                
                card.SetupCard(selectedUpgrade);
                
                tempUpgrades.RemoveAt(randomIndex);
            }
        }

        public void ApplyUpgrade(UpgradeData upgrade)
        {
            playerStats.AddStat(upgrade.statToUpgrade,  upgrade.increaseAmount);
            
            levelUpCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}