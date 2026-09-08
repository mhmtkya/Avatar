using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game Data/Upgrade Data", order = 0)]
    public class UpgradeData : ScriptableObject
    {
        public string upgradeName;
        [TextArea] public string description;
        public Sprite upgradeIcon;
        
        [Header("Stat Effect")]
        public StatType statToUpgrade;
        public float increaseAmount;

    }
}