using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

    private const KeyCode DEFAULT_SHOOT = KeyCode.Mouse0;
    private const KeyCode DEFAULT_RUN = KeyCode.LeftShift;
    private const KeyCode DEFAULT_INTERACT = KeyCode.E;
    private const KeyCode DEFAULT_PAUSE = KeyCode.Escape;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadKeybinds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadKeybinds()
    {
        keybinds["Shoot"] = (KeyCode)PlayerPrefs.GetInt("ShootKey", (int)DEFAULT_SHOOT);
        keybinds["Run"] = (KeyCode)PlayerPrefs.GetInt("RunKey", (int)DEFAULT_RUN);
        keybinds["Interact"] = (KeyCode)PlayerPrefs.GetInt("InteractKey", (int)DEFAULT_INTERACT);
        keybinds["Pause"] = (KeyCode)PlayerPrefs.GetInt("PauseKey", (int)DEFAULT_PAUSE);
    }

    public KeyCode GetKeybind(string actionName)
    {
        if (keybinds.ContainsKey(actionName))
        {
            return keybinds[actionName];
        }
        Debug.LogWarning($"Keybind for action '{actionName}' not found. Using default.");
        return KeyCode.None;
    }

    public void SetKeybind(string actionName, KeyCode newKey)
    {
        if (keybinds.ContainsKey(actionName))
        {
            keybinds[actionName] = newKey;
            PlayerPrefs.SetInt(actionName + "Key", (int)newKey);
            PlayerPrefs.Save();
            Debug.Log($"Keybind for '{actionName}' changed to '{newKey}'");
        }
        else
        {
            Debug.LogWarning($"Attempted to set keybind for unknown action: '{actionName}'");
        }
    }

    public void StartRebinding(string actionName, System.Action<KeyCode> onKeyRebound)
    {
        Debug.Log($"Press a new key for {actionName}...");
        StartCoroutine(DetectKeypress(actionName, onKeyRebound));
    }

    private System.Collections.IEnumerator DetectKeypress(string actionName, System.Action<KeyCode> onKeyRebound)
    {
        yield return null;

        while (true)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    SetKeybind(actionName, keyCode);
                    onKeyRebound?.Invoke(keyCode);
                    yield break;
                }
            }
            if (Input.GetMouseButtonDown(0)) // Left click
            {
                SetKeybind(actionName, KeyCode.Mouse0);
                onKeyRebound?.Invoke(KeyCode.Mouse0);
                yield break;
            }
            if (Input.GetMouseButtonDown(1)) // Right click
            {
                SetKeybind(actionName, KeyCode.Mouse1);
                onKeyRebound?.Invoke(KeyCode.Mouse1);
                yield break;
            }
            if (Input.GetMouseButtonDown(2)) // Middle click
            {
                SetKeybind(actionName, KeyCode.Mouse2);
                onKeyRebound?.Invoke(KeyCode.Mouse2);
                yield break;
            }

            yield return null;
        }
    }
}