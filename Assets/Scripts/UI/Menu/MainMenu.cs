using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI & Sahne Ayarları")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private string gameSceneName = "MainScene";

    public void PlayGame()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("[MainMenu] gameSceneName boş! Lütfen Inspector'da geçilecek sahne adını girin.");
        }
    }

    public void OpenOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

