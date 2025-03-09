using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea;
    private SpriteRenderer attackSprite;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    public float attackDistance = 0f;
    private float angle = 0f;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        attackSprite = attackArea.GetComponent<SpriteRenderer>();

        // Make it slightly visible by default
        attackSprite.color = new Color(1, 0, 0, 0.2f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Attack();

        if (attacking)
        {
            timer += Time.deltaTime;
            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
                attackSprite.color = new Color(1, 0, 0, 0.2f);
            }
        }

        RotateAttackAroundPlayer();
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
        attackSprite.color = new Color(1, 0, 0, 1f);
    }

    private void RotateAttackAroundPlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Get direction from player to mouse
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Calculate angle in degrees
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set attack area's rotation so the point faces outward
        attackArea.transform.rotation = Quaternion.Euler(0, 0, angle);

    }
}
