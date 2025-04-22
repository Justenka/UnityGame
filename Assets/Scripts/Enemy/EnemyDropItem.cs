using UnityEngine;

[System.Serializable]
public class DropData
{
    public PickupType type;
    public int minAmount = 1;
    public int maxAmount = 1;
    public ScriptableObject itemData;
    public float dropChance = 1f;
}

public class EnemyDropItem : MonoBehaviour
{
    public GameObject pickupPrefab;
    public DropData[] dropTable;

    public void DropAll()
    {
        foreach (var drop in dropTable)
        {
            if (Random.value <= drop.dropChance)
            {
                int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);

                GameObject pickup = Instantiate(pickupPrefab, transform.position + (Vector3)Random.insideUnitCircle * 0.5f, Quaternion.identity);
                PickupItem pickupItem = pickup.GetComponent<PickupItem>();
                pickupItem.type = drop.type;
                pickupItem.amount = amount;
                pickupItem.itemData = drop.itemData;

                Rigidbody2D rb = pickup.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(1f, 3f)), ForceMode2D.Impulse);
                }
            }
        }
    }
}
