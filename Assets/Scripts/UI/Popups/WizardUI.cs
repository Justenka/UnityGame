using UnityEngine;
using UnityEngine.SceneManagement;

public class WizardUI : MonoBehaviour
{

    [Header("Dungeon Scene Names")]
    public string firstDungeonSceneName = "Scenes/FirstDungeon";
    public string TutorialScene = "Scenes/Tutorial";
    public string HUB = "Scenes/HUB";

    public void GoToFirstDungeon()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.CloseAllMenus();
            Time.timeScale = 1f;
        }

        SceneManager.LoadScene(firstDungeonSceneName);
    }
    public void GoToTutorial()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.CloseAllMenus();
            Time.timeScale = 1f;
        }

        SceneManager.LoadScene(TutorialScene);
    }
    public void GoToHUB()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.CloseAllMenus();
            Time.timeScale = 1f;
        }

        SceneManager.LoadScene(HUB);
    }
}
