using UnityEngine;
using UnityEngine.UI;

public class FogOfWarTexture : MonoBehaviour
{
    [Header("Fog Settings")]
    public RawImage fogImage;
    public int resolution = 256;
    public FilterMode filterMode = FilterMode.Bilinear;

    private Texture2D fogTexture;
    private Color32[] fogPixels;

    public bool IsReady => fogTexture != null && fogPixels != null;

    private void OnEnable()
    {
        if (!IsReady)
            InitializeFog();
    }

    public void InitializeFog()
    {
        if (IsReady) return;

        fogTexture = new Texture2D(resolution, resolution, TextureFormat.Alpha8, false);
        fogTexture.wrapMode = TextureWrapMode.Clamp;
        fogTexture.filterMode = filterMode;

        fogPixels = new Color32[resolution * resolution];
        for (int i = 0; i < fogPixels.Length; i++)
            fogPixels[i] = new Color32(0, 0, 0, 255);

        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();

        if (fogImage != null)
            fogImage.texture = fogTexture;
    }

    public void RevealArea(Vector2 normalizedPosition, float normalizedRadius)
    {
        if (!IsReady) InitializeFog();

        int centerX = (int)(normalizedPosition.x * resolution);
        int centerY = (int)(normalizedPosition.y * resolution);
        int radius = Mathf.CeilToInt(normalizedRadius * resolution);

        bool updated = false;

        for (int y = -radius; y <= radius; y++)
        {
            int py = centerY + y;
            if (py < 0 || py >= resolution) continue;

            for (int x = -radius; x <= radius; x++)
            {
                int px = centerX + x;
                if (px < 0 || px >= resolution) continue;

                if (x * x + y * y <= radius * radius)
                {
                    int index = py * resolution + px;
                    if (fogPixels[index].a > 0)
                    {
                        fogPixels[index].a = 0;
                        updated = true;
                    }
                }
            }
        }

        if (updated)
        {
            fogTexture.SetPixels32(fogPixels);
            fogTexture.Apply();
        }
    }
}
