using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject pauseMenu;
    public List<GameObject> openMenus = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            HandleEscape();
        }
    }

    public void HandleEscape()
    {
        if (openMenus.Count > 0)
        {
            // Close top-most menu
            GameObject last = openMenus[openMenus.Count - 1];
            last.SetActive(false);
            openMenus.RemoveAt(openMenus.Count - 1);
        }
        else
        {
            // Toggle pause menu
            bool isOpen = pauseMenu.activeSelf;
            pauseMenu.SetActive(!isOpen);
            if (!isOpen) openMenus.Add(pauseMenu);
        }
    }

    public void RegisterOpenMenu(GameObject menu)
    {
        if (!openMenus.Contains(menu))
            openMenus.Add(menu);
    }

    public void UnregisterMenu(GameObject menu)
    {
        openMenus.Remove(menu);
    }

    public void CloseAllMenus()
    {
        foreach (var menu in openMenus)
            menu.SetActive(false);

        openMenus.Clear();
    }
}
