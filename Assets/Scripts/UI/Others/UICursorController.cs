using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICursorController : MonoBehaviour
{
    [Header("Cursor Sprites")]
    public Sprite defaultCursor;
    public Sprite attackCursor;
    public Sprite chatCursor;

    [Header("Cursor UI")]
    [SerializeField] private RectTransform cursorTransform;

    private Image cursorImage;
    private Sprite currentCursor;

    void Start()
    {
        cursorImage = cursorTransform.GetComponent<Image>();
        ForceDefaultCursor();
        HideSystemCursor();
    }

    void Update()
    {
        // Always hide system cursor no matter what
        HideSystemCursor();

        cursorTransform.position = Input.mousePosition;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            ForceDefaultCursor(); // Optional: don't change while over UI
            return;
        }

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject target = hit.collider.gameObject;

            if (target.CompareTag("Enemy"))
            {
                SetCursor(attackCursor, new Vector2(0f, 1f));
            }
            else if (target.CompareTag("NPC"))
            {
                SetCursor(chatCursor, new Vector2(0f, 1f));
            }
            else
            {
                ForceDefaultCursor();
            }
        }
        else
        {
            ForceDefaultCursor();
        }
    }

    public void SetCursor(Sprite newSprite, Vector2 hotspot)
    {
        if (newSprite == null || currentCursor == newSprite)
            return;

        cursorImage.sprite = newSprite;
        cursorTransform.pivot = new Vector2(
            hotspot.x / newSprite.rect.width,
            1f - (hotspot.y / newSprite.rect.height)
        );
        currentCursor = newSprite;
    }

    public void ForceDefaultCursor()
    {
        SetCursor(defaultCursor, new Vector2(0f, 1f));
    }

    private void HideSystemCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None; // Set to Locked if needed
    }
}
