using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    public static bool isPaused = false;

    private void Start()
    {
        // Ensure panels are hidden when scene starts
        Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsPanel != null && optionsPanel.activeSelf)
            {
                // ESC pressed while in Options -> Go back to Pause Menu
                CloseOptions();
            }
            else if (isPaused)
            {
                // ESC pressed while in Pause Menu -> Resume Game
                Resume();
            }
            else
            {
                // ESC pressed during gameplay -> Pause Game
                Pause();
            }
        }
    }

    public void Pause()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
        else
            Debug.LogWarning("[PauseMenu] pauseMenuPanel is not assigned in Inspector!");

        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenOptions()
    {
        Debug.Log("[PauseMenu] OpenOptions() called.");

        if (optionsPanel == null)
        {
            Debug.LogError("[PauseMenu] ERROR: optionsPanel reference is NULL in Inspector! Please drag your Options Panel into the PauseMenu script.");
            return;
        }

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        Debug.Log("[PauseMenu] CloseOptions() called.");

        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}


