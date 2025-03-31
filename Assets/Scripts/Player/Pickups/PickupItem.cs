using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public PickupType type;
    public int amount = 1;
    public ScriptableObject itemData;

    [Header("Visuals")]
    public Sprite goldSprite;
    public Sprite potionSprite;
    public Sprite defaultSprite;

    private void Start()
{
    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    if (sr == null) return;

    switch (type)
    {
        case PickupType.Currency:
            sr.sprite = goldSprite;
            break;
        case PickupType.Potion:
            sr.sprite = potionSprite;
            break;
        //case PickupType.Item:
        //    if (itemData is ItemSO item)
        //        sr.sprite = item.icon;
        //    else
        //        sr.sprite = defaultSprite;
        //    break;
        default:
            sr.sprite = defaultSprite;
            break;
    }
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var handler = other.GetComponent<PlayerPickupHandler>();
            if (handler != null)
            {
                handler.HandlePickup(this);
                Destroy(gameObject);
            }
        }
    }
}
