using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;

    private void Start()
    {
        // Try to find PlayerInteract in case it's missing due to scene load
        if (playerInteract == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerInteract = player.GetComponent<PlayerInteract>();
            }
        }
    }

    private void Update()
    {
        if (playerInteract == null) return;

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
