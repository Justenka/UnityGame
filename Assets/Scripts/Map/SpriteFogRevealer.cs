using UnityEngine;

public class SpriteFogRevealer : MonoBehaviour
{
    public SpriteRenderer fogRenderer;
    public float revealRadius = 5f;

    private Texture2D fogTexture;
    private Color32[] fogPixels;
    private int textureSize;

    void Start()
    {
        // Clone texture to make it writable
        fogTexture = Instantiate(fogRenderer.sprite.texture);
        textureSize = fogTexture.width;
        fogPixels = fogTexture.GetPixels32();

        // Reassign cloned texture
        fogRenderer.sprite = Sprite.Create(
            fogTexture,
            new Rect(0, 0, fogTexture.width, fogTexture.height),
            new Vector2(0.5f, 0.5f),
            fogRenderer.sprite.pixelsPerUnit
        );
    }

    void Update()
    {
        RevealDirectly();
    }
    void RevealDirectly()
    {
        Vector2 localPos = fogRenderer.transform.InverseTransformPoint(transform.position);

        // Convert localPos (-0.5 to +0.5 range) to texture pixels
        Vector2 spriteSize = fogRenderer.sprite.bounds.size;
        int cx = Mathf.FloorToInt(((localPos.x / spriteSize.x) + 0.5f) * textureSize);
        int cy = Mathf.FloorToInt(((localPos.y / spriteSize.y) + 0.5f) * textureSize);

        int radius = Mathf.CeilToInt(revealRadius * textureSize / spriteSize.x);

        bool changed = false;

        for (int y = -radius; y <= radius; y++)
        {
            int py = cy + y;
            if (py < 0 || py >= textureSize) continue;

            for (int x = -radius; x <= radius; x++)
            {
                int px = cx + x;
                if (px < 0 || px >= textureSize) continue;

                if (x * x + y * y <= radius * radius)
                {
                    int index = py * textureSize + px;
                    if (fogPixels[index].a > 0)
                    {
                        fogPixels[index].a = 0;
                        changed = true;
                    }
                }
            }
        }

        if (changed)
        {
            fogTexture.SetPixels32(fogPixels);
            fogTexture.Apply();
        }
    }
}
