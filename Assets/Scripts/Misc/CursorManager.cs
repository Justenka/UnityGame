using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Textures")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D attackCursor;
    [SerializeField] private Texture2D chatCursor;

    private Texture2D currentCursor;
    private Vector2 hotspot = Vector2.zero;

    void Start()
    {
        SetCursor(defaultCursor);
    }

    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject target = hit.collider.gameObject;

            if (target.CompareTag("Enemy"))
            {
                SetCursor(attackCursor);
            }
            else if (target.CompareTag("NPC"))
            {
                SetCursor(chatCursor);
            }
            else
            {
                SetCursor(defaultCursor);
            }
        }
        else
        {
            SetCursor(defaultCursor);
        }
    }

    void SetCursor(Texture2D cursor)
    {
        if (cursor == null || currentCursor == cursor) return;

        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
        currentCursor = cursor;
    }
}
