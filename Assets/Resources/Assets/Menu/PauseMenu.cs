using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public void GoToMainMenu() {
        SceneManager.UnloadSceneAsync("PauseMenu");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }
}
