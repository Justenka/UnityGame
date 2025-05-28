using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public bool resetKeybindsOnLaunch = false;

    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

    private Dictionary<string, KeyCode> defaultKeybinds = new Dictionary<string, KeyCode>()
    {
        { "Shoot", KeyCode.Mouse0 },
        { "Run", KeyCode.LeftShift },
        { "Interact", KeyCode.E },
        { "Pause", KeyCode.Escape },
        { "Inventory", KeyCode.I }
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Check the public boolean to decide whether to reset
            if (resetKeybindsOnLaunch)
            {
                Debug.Log("Resetting keybinds to default due to 'resetKeybindsOnLaunch' being true.");
                ResetToDefaults();
            }
            else
            {
                LoadKeybinds();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetToDefaults()
    {
        Debug.Log("Resetting all keybinds to default values.");
        keybinds.Clear();

        foreach (var entry in defaultKeybinds)
        {
            keybinds[entry.Key] = entry.Value;
            PlayerPrefs.SetInt(entry.Key + "Key", (int)entry.Value);
        }
        PlayerPrefs.Save();
        Debug.Log("Keybinds reset and saved.");
    }

    void LoadKeybinds()
    {
        foreach (var entry in defaultKeybinds)
        {
            keybinds[entry.Key] = (KeyCode)PlayerPrefs.GetInt(entry.Key + "Key", (int)entry.Value);
        }
        Debug.Log("Keybinds loaded from PlayerPrefs.");
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