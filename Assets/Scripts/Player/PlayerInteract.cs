using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    private Collider2D playerCollider;

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 center = playerCollider.bounds.center;
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(center, interactRange);

            foreach (Collider2D collider in colliderArray)
            {
                if (collider.TryGetComponent(out NPCBase npc))
                {
                    npc.Interact();
                    UIController uiController = Object.FindFirstObjectByType<UIController>();
                    if (uiController != null)
                    {
                        uiController.SetActiveNPC(npc);
                        break; // Only interact with one NPC at a time
                    }
                }
            }
        }
    }

    public NPCBase GetInteractableObject()
    {
        Vector2 center = playerCollider.bounds.center;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(center, interactRange);

        foreach (Collider2D collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPCBase npc))
            {
                return npc;
            }
        }

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCollider == null) playerCollider = GetComponent<Collider2D>();
        if (playerCollider == null) return;

        Vector2 center = playerCollider.bounds.center;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, interactRange);
    }
}
