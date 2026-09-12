using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("GameScene")]
    [SerializeField] private string gameSceneName = "GameScene"; // Oynanacak sahnenin adı

    public void OnPlayClicked()
    {
    
    }

    public void OnSettingsClicked()
    {
        // Ana menü butonlarını gizleyip ayarlar panelini aç
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnCloseSettingsClicked()
    {
        // Ayarları kapatıp ana menüye dön
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnQuitClicked()
    {
        // Oyunu kapat
        Debug.Log("Oyundan çıkıldı!");
        Application.Quit();
    }
}