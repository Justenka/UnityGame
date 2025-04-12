using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public void ResumeGame()
    {
        UIManager.Instance.UnregisterMenu(gameObject); // Remove from openMenus
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenSettings()
    {
        Debug.Log("Settings button pressed");
        // Show your settings menu here
    }

    public void ExitGame()
    {
        Debug.Log("Exit button pressed");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}