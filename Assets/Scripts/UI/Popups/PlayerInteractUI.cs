using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;

    private void Update()
    {
        NPCBase interactable = playerInteract.GetInteractableObject();

        if (interactable != null)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        if (!containerGameObject.activeSelf)
        {
            containerGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        if (containerGameObject.activeSelf)
        {
            containerGameObject.SetActive(false);
        }
    }
}
