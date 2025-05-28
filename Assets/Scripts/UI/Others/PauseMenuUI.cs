using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;

    void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.GetKeybind("Pause")))
        {
            HandlePauseInput();
        }
    }

    private void HandlePauseInput() 
    {
        if (Time.timeScale == 0)
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            else
            {
                ResumeGame();
            }
        }
        else 
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }

            gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }

        UIManager.Instance.UnregisterMenu(gameObject);

        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenSettings()
    {
        Debug.Log("Settings button pressed - Opening Settings Panel.");

        gameObject.SetActive(false);

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Settings Panel is not assigned in PauseMenuUI. Please assign it in the Inspector.");
        }
    }
    public void CloseSettings()
    {
        Debug.Log("Closing Settings Panel - Returning to Pause Menu.");

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        gameObject.SetActive(true);
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