using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player GameObject in the Inspector
    public Vector3 offset; // Adjust this to control the camera's position relative to the player

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
