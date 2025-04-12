using UnityEngine;

public class MapRevealer : MonoBehaviour
{
    public FogOfWarTexture fog;
    public float revealRadiusNormalized = 0.04f; // value from 0–1 (based on world size)
    public Camera mapCamera;

    public float updateInterval = 0.2f;
    private float timer = 0f;

    void Start()
    {
        Vector3 viewportPos = mapCamera.WorldToViewportPoint(transform.position);
        fog.RevealArea(new Vector2(viewportPos.x, viewportPos.y), revealRadiusNormalized);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;

            Vector3 viewportPos = mapCamera.WorldToViewportPoint(transform.position);
            fog.RevealArea(new Vector2(viewportPos.x, viewportPos.y), revealRadiusNormalized);
        }
    }
}