using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public void StartGame()
    {
        string username = usernameInputField.text;
        PlayerPrefs.SetString("PlayerUsername", username);
        SceneManager.LoadScene("Scenes/Tutorial");
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void SetDifficultyToEasy()
    {
        GameSettings.gameDifficulty = 0.5f;
    }
    public void SetDifficultyToNormal()
    {
        GameSettings.gameDifficulty = 1f;
    }
    public void SetDifficultyToHard()
    {
        GameSettings.gameDifficulty = 2f;
    }
}
