using System;
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
        public Animator animator;
        
        private UpgradeData myUpgrade;

        private void Start()
        {
            nameText.gameObject.SetActive(false);
            descText.gameObject.SetActive(false);
            iconImage.gameObject.SetActive(false);
            animator = GetComponent<Animator>();
        }

        private void OnDisable()
        {
            nameText.gameObject.SetActive(false);
            descText.gameObject.SetActive(false);
            iconImage.gameObject.SetActive(false);
        }

        public void SetupCard(UpgradeData upgradeData)
        {
            animator.Play("button");
            myUpgrade = upgradeData;
        }

        public void OnComplete()
        {
            nameText.gameObject.SetActive(true);
            descText.gameObject.SetActive(true);
            iconImage.gameObject.SetActive(true);
            SetupCardAfterAnim();
            
        }
        
        public void SetupCardAfterAnim()
        {
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