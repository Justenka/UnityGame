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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
