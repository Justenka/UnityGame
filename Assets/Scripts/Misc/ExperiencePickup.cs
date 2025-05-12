using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class ExperiencePickup : MonoBehaviour
{
    public int expValue;

    public float moveSpeed = 3;

    public float timeBetweenChecks = 0.2f;
    private float checkCounter;

    private GameObject player;
    public float pickupRange = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2.Distance(transform.position, player.transform.position) < pickupRange))
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExperienceLevelController.instance.GetExp(expValue);

            Destroy(gameObject);
        }
    }
}
