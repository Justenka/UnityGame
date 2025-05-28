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
        gameObject.SetActive(false);

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            UIManager.Instance.RegisterOpenMenu(settingsPanel);
        }

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