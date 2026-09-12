using System;
using _Project.Scripts.Data;
using _Project.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class WeaponLevelCard : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descText;
        public TextMeshProUGUI lvlText;
        public Image iconImage;
        public Button cardButton;
        public Animator animator;
        
        private UpgradeData myUpgrade;
        private WeaponData myWeapon;

        private void Start()
        {
            nameText.gameObject.SetActive(false);
            descText.gameObject.SetActive(false);
            iconImage.gameObject.SetActive(false);
            lvlText.gameObject.SetActive(false);
            animator = GetComponent<Animator>();
        }

        private void OnDisable()
        {
            nameText.gameObject.SetActive(false);
            descText.gameObject.SetActive(false);
            iconImage.gameObject.SetActive(false);
            lvlText.gameObject.SetActive(false);
        }

        public void SetupWeaponCard(WeaponData weaponData)
        {
            animator.Play("button");
            myWeapon = weaponData;
        }

        public void OnComplete()
        {
            nameText.gameObject.SetActive(true);
            descText.gameObject.SetActive(true);
            iconImage.gameObject.SetActive(true);
            lvlText.gameObject.SetActive(true);
            SetupWeaponCardAfterAnim();
        }
        
        private void SetupWeaponCardAfterAnim()
        {

            int currentLevel = WeaponInventoryManager.Instance.GetCurrentWeaponLevel(myWeapon);
            int nextLevel = currentLevel + 1;

            if (currentLevel == 0)
            {
                nameText.text = myWeapon.weaponName;
                descText.text = myWeapon.statsPerLevel[currentLevel].levelDescription;
                lvlText.text = "New";
            }
            else
            {
                nameText.text = myWeapon.weaponName;
                descText.text = "Geliştir: \n" + myWeapon.statsPerLevel[nextLevel - 1].levelDescription;
                lvlText.text = currentLevel + " --> "  + nextLevel; 
            }
            
            if (myWeapon.weaponIcon != null)  iconImage.sprite = myWeapon.weaponIcon; iconImage.SetNativeSize();
                   
            
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(OnWeaponCardClicked);

        }

        private void OnWeaponCardClicked()
        {
            WeaponSelectionManager.Instance.SelectWeapon(myWeapon);
        }
        
        
    }
}