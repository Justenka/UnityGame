using UnityEngine;
using UnityEngine.UI;

public class PermadeathToggle : MonoBehaviour
{
    public GameObject confirmationMenu;

    public Toggle permadeathToggle;
    public GameObject playMenu;

    void Start()
    {
    //    permadeathToggle = GetComponent<Toggle>();
        if (confirmationMenu != null)
        {
            confirmationMenu.SetActive(false);
        }
        permadeathToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnToggleValueChanged(bool isOn)
    {
        if (isOn && confirmationMenu != null)
        {
            confirmationMenu.SetActive(true);
            playMenu.SetActive(false);
        }
    }
    public void ConfirmPermadeath()
    {
        GameSettings.isPermadeathEnabled = true;
        confirmationMenu.SetActive(false);
        permadeathToggle.isOn = true;
        playMenu.SetActive(true);
    }

    public void CancelPermadeath()
    {
        GameSettings.isPermadeathEnabled = false;
        confirmationMenu.SetActive(false);
        permadeathToggle.isOn = false;
        playMenu.SetActive(true);
    }
}