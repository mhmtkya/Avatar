using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsTabs : MonoBehaviour
{
    [Serializable]
    public class Tab
    {
        public string tabName;
        public Button button;
        public GameObject content;
        [HideInInspector] public Animator animator;
        [HideInInspector] public RectTransform rectTransform;
    }

    [SerializeField] private List<Tab> tabs = new List<Tab>();

    private void Awake()
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            int index = i;
            tabs[i].animator = tabs[i].button.GetComponent<Animator>();
            tabs[i].rectTransform = tabs[i].button.GetComponent<RectTransform>();

            // Butona tıklandığında ilgili sekmeyi aç
            tabs[i].button.onClick.AddListener(() => SelectTab(index));
        }
    }

    private void OnEnable()
    {
        // Ayarlar her açıldığında varsayılan olarak 0. sekmeyi (Ses) aç
        SelectTab(0);
    }

    public void SelectTab(int targetIndex)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            bool isSelected = (i == targetIndex);

            // 1. İlgili içerik sayfasını aç / kapat
            if (tabs[i].content != null)
                tabs[i].content.SetActive(isSelected);

            // 2. Buton animasyonu / basılı kalma efekti
            if (tabs[i].animator != null)
            {
                // Animator kullanıyorsan "IsSelected" bool'unu tetikle
                tabs[i].animator.SetBool("IsSelected", isSelected);
            }

            // Seçili olan butona tekrar basılmasını engelle
            tabs[i].button.interactable = !isSelected;
        }
    }
}