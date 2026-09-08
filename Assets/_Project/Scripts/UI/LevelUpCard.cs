using _Project.Scripts.Data;
using _Project.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class LevelUpCard : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descText;
        public Image iconImage;
        public Button cardButton;

        private UpgradeData myUpgrade;
        private WeaponData myWeapon;

        public void SetupWeaponCard(WeaponData weaponData)
        {
            myWeapon = weaponData;

            nameText.text = myWeapon.weaponName;
            descText.text = "Hasar: " + myWeapon.baseDamage;

            if (myWeapon.weaponIcon != null)
                iconImage.sprite = myWeapon.weaponIcon;
            
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(OnWeaponCardClicked);

        }

        private void OnWeaponCardClicked()
        {
            WeaponSelectionManager.Instance.SelectWeapon(myWeapon);
        }
        
        public void SetupCard(UpgradeData upgrade)
        {
            myUpgrade = upgrade;
            nameText.text = myUpgrade.upgradeName;
            descText.text = myUpgrade.description;

            if (myUpgrade.upgradeIcon != null)
                iconImage.sprite = myUpgrade.upgradeIcon;
            
            
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(OnCardClicked);
        }

        private void OnCardClicked()
        {
            LevelUpManager.Instance.ApplyUpgrade(myUpgrade);
        }
    }
}