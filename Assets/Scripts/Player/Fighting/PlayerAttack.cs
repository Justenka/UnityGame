using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea;
    private SpriteRenderer attackSprite;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private float angle = 0f;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        attackSprite = attackArea.GetComponent<SpriteRenderer>();
        attackSprite.color = new Color(1, 0, 0, 0.2f);
    }

    void Update()
    {
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

    public void DoAttack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
        attackSprite.color = new Color(1, 0, 0, 1f);
    }

    private void RotateAttackAroundPlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackArea.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}