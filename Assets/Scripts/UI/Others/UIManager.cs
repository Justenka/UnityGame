using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiDimmer;

    public static UIManager Instance;

    public GameObject pauseMenu;

    public GameObject coreInventory;
    public GameObject storageInventory;
    public TooltipUI tooltipUI;

    public List<GameObject> openMenus = new List<GameObject>();
    public bool IsAnyMenuOpen => openMenus.Count > 0;
    void Start()
    {
        if (tooltipUI != null)
        {
            InventoryItem.tooltip = tooltipUI;
        }
        else
        {
            Debug.LogError("TooltipUI is not assigned in UIManager. Inventory tooltips may not work.");
        }
    }

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.GetKeybind("Pause")))
        {
            HandleEscape();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void HandleEscape()
    {
        if (openMenus.Count > 0)
        {
            GameObject topMenu = openMenus[openMenus.Count - 1];

            if (topMenu == pauseMenu)
            {
                PauseMenuUI pauseMenuScript = pauseMenu.GetComponent<PauseMenuUI>();
                if (pauseMenuScript != null && pauseMenuScript.settingsPanel != null && pauseMenuScript.settingsPanel.activeSelf)
                {
                    pauseMenuScript.CloseSettings();
                    return;
                }
            }

            if (openMenus.Count > 0)
            {
                GameObject menuToClose = openMenus[openMenus.Count - 1];
                UnregisterMenu(menuToClose);

                if (openMenus.Count == 0)
                {
                    Time.timeScale = 1;
                }
            }
        }
        else
        {
            if (Time.timeScale != 0)
            {
                PauseMenuUI pauseMenuScript = pauseMenu.GetComponent<PauseMenuUI>();
                if (pauseMenuScript != null)
                {
                    pauseMenuScript.OpenPauseMenu();
                }
                else
                {
                    Debug.LogError("PauseMenuUI script not found on pauseMenu GameObject!");
                    // Fallback: Directly activate and register if script is missing
                    pauseMenu.SetActive(true);
                    RegisterOpenMenu(pauseMenu);
                    Time.timeScale = 0;
                }
            }
        }
    }

    public void RegisterOpenMenu(GameObject menu)
    {
        if (menu != null && !openMenus.Contains(menu))
        {
            openMenus.Add(menu);
            if (uiDimmer != null)
            {
                uiDimmer.SetActive(true);
            }
            Time.timeScale = 0;
            Debug.Log($"Registered menu: {menu.name}. Total open menus: {openMenus.Count}");
        }
    }

    public void UnregisterMenu(GameObject menu)
    {
        if (menu != null && openMenus.Contains(menu))
        {
            openMenus.Remove(menu);
            menu.SetActive(false);

            if (openMenus.Count == 0)
            {
                if (uiDimmer != null)
                {
                    uiDimmer.SetActive(false);
                }
                Time.timeScale = 1;
                Debug.Log($"Unregistered menu: {menu.name}. All menus closed. Resuming game.");
            }
            else
            {
                Debug.Log($"Unregistered menu: {menu.name}. Remaining open menus: {openMenus.Count}");
            }
        }
    }

    public void CloseAllMenus()
    {
        while (openMenus.Count > 0)
        {
            GameObject menuToClose = openMenus[openMenus.Count - 1];
            UnregisterMenu(menuToClose);
            if (menuToClose != null && menuToClose.TryGetComponent(out UpgradeUI upgradeUI))
            {
                upgradeUI.ReturnItemsToInventory();
            }
        }

        if (coreInventory != null && coreInventory.activeSelf)
        {
            coreInventory.SetActive(false);
        }
        if (storageInventory != null && storageInventory.activeSelf)
        {
            storageInventory.SetActive(false);
        }

        if (uiDimmer != null)
        {
            uiDimmer.SetActive(false);
        }

        if (tooltipUI != null)
        {
            tooltipUI.Hide();
        }

        Time.timeScale = 1;
        Debug.Log("Closed all menus through UIManager. Time resumed.");
    }

    public void ToggleInventory()
    {
        if (coreInventory.activeSelf)
        {
            UnregisterMenu(coreInventory);
            UnregisterMenu(storageInventory);
            tooltipUI.Hide();
        }
        else
        {
            coreInventory.SetActive(true);
            storageInventory.SetActive(true);
            RegisterOpenMenu(coreInventory);
            RegisterOpenMenu(storageInventory);
        }
    }
}