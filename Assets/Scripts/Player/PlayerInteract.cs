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
                if(collider.TryGetComponent(out NPCInteractable npcInteractable))
                {
                    npcInteractable.Interact();
                }
            }
        }
    }

    public NPCInteractable GetInteractableObject()
    {
        Vector2 center = playerCollider.bounds.center;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(center, interactRange);
        foreach (Collider2D collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                return npcInteractable;
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
