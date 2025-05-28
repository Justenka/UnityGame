using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportTarget;

    private bool hasTeleported = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTeleported) return;

        if (other.CompareTag("Player") && teleportTarget != null)
        {
            other.transform.position = teleportTarget.position;
            hasTeleported = true;
            Debug.Log("Player teleported to target location.");
        }
        else if (teleportTarget == null)
        {
            Debug.LogWarning("Teleport target not set on portal!");
        }
    }
}
