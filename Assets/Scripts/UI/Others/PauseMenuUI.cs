using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject pauseMenuPanel;

    private bool isPauseMenuOpen = false;
    private bool isSettingsOpen = false;

    void Awake()
    {
        if (pauseMenuPanel == null)
        {
            pauseMenuPanel = gameObject;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.GetKeybind("Pause")))
        {
            HandlePauseInput();
        }
    }

    private void HandlePauseInput()
    {
        if (isSettingsOpen)
        {
            CloseSettings();
        }
        else if (isPauseMenuOpen)
        {
            ResumeGame();
        }
        else
        {
            if (Time.timeScale != 0)
            {
                OpenPauseMenu();
            }
        }
    }

    public void OpenPauseMenu()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0;
        isPauseMenuOpen = true;
        isSettingsOpen = false;
        UIManager.Instance.RegisterOpenMenu(pauseMenuPanel);
    }

    public void ResumeGame()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        isPauseMenuOpen = false;
        isSettingsOpen = false; 
        UIManager.Instance.UnregisterMenu(pauseMenuPanel);
    }
    public void OpenSettings()
    {
        Debug.Log("Settings button pressed - Opening Settings Panel.");

        pauseMenuPanel.SetActive(false);
        isPauseMenuOpen = false;

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            isSettingsOpen = true;
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
            isSettingsOpen = false;
        }

        pauseMenuPanel.SetActive(true);
        isPauseMenuOpen = true;
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