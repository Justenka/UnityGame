using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public float panSpeed = 50f;
    public float zoomSpeed = 10f;
    public float minZoom = 20f;
    public float maxZoom = 200f;

    private Camera cam;
    private bool isMapOpen => mapUI != null && mapUI.activeSelf;

    [SerializeField] private GameObject mapUI; // assign your Map UI Canvas

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (!isMapOpen) return;

        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandlePan()
    {
        Vector3 move = Vector3.zero;

        // Mouse drag (middle or right button)
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            move.x -= Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
            move.y -= Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime;
        }

        // Optional: WASD or arrow keys
        //move.x += Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime;
        //move.y += Input.GetAxis("Vertical") * panSpeed * Time.deltaTime;

        transform.position += move;
    }
}
