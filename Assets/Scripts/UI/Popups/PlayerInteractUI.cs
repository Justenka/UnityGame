using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containergameObject;
    [SerializeField] private PlayerInteract playerInteract;

    private void Update()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    public void Show()
    {
        containergameObject.SetActive(true);
    }
    public void Hide()
    {
        containergameObject.SetActive(false);
    }
}
