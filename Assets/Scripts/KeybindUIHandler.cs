using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeybindUIHandler : MonoBehaviour
{
    public string actionName;
    public TextMeshProUGUI currentKeyText;
    public Button rebindButton;

    void Start()
    {
        UpdateKeyText();
        rebindButton.onClick.AddListener(OnRebindButtonClicked);
    }

    void UpdateKeyText()
    {
        if (InputManager.Instance != null)
        {
            currentKeyText.text = InputManager.Instance.GetKeybind(actionName).ToString();
        }
    }

    void OnRebindButtonClicked()
    {
        currentKeyText.text = "Press a key...";
        InputManager.Instance.StartRebinding(actionName, OnKeyRebound);
    }

    void OnKeyRebound(KeyCode newKey)
    {
        UpdateKeyText();
    }
}