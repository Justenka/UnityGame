using UnityEngine;

public class MapToggle : MonoBehaviour
{
    public GameObject mapUI;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    public void ToggleMap()
    {
        isOpen = !isOpen;
        mapUI.SetActive(isOpen);

        if (isOpen)
            UIManager.Instance.RegisterOpenMenu(mapUI);
        else
            UIManager.Instance.UnregisterMenu(mapUI);
    }
}