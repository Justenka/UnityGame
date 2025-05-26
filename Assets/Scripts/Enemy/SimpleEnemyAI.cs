using UnityEngine;

public class SimpleEnemyAI : MonoBehaviour
{
    public Player player;
    public float speedModifier;
    private float distance;
    public float speed;
    private float minSpeed = 1;
    //public float despawnDistance = 80f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        player = playerGameObject.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player script not found on the GameObject with tag 'Player' in " + gameObject.name);
            enabled = false; // Disable this script to prevent further errors
        }
        if (speedModifier != 0)
        {
            speed = Random.Range(0.5f, 4.5f) * speedModifier;
        }
        else
        {
            speed = minSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);

            //if (distance >= despawnDistance)
            //{
            //    Destroy(gameObject);
            //}

            Vector2 direction = player.transform.position - transform.position;
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
