using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea;
    private SpriteRenderer attackSprite;
    private Animator animator;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private float angle = 0f;
    private AttackArea attackAreaScript;
    private WeaponRotation weaponRotation;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        attackSprite = attackArea.GetComponent<SpriteRenderer>();
        attackSprite.color = new Color(1, 0, 0, 0.2f);

        attackAreaScript = attackArea.GetComponent<AttackArea>();
        animator = GetComponentInChildren<Animator>();
        Transform weaponParent = transform.Find("WeaponParent");
        if (weaponParent != null)
        {
            weaponRotation = weaponParent.GetComponent<WeaponRotation>();
        }
    }

    void Update()
    {
        if (attacking)
        {
            timer += Time.deltaTime;
            if (timer >= timeToAttack)
            {
                timer = 0f;
                attacking = false;
                attackArea.SetActive(false);
                attackSprite.color = new Color(1, 0, 0, 0.2f);
                attackAreaScript.EndAttack();
            }
        }

        RotateAttackAroundPlayer();
    }

    public void DoAttack()
    {
        attacking = true;
        attackArea.SetActive(true);
        attackSprite.color = new Color(1, 0, 0, 1f);
        attackAreaScript.StartAttack();

        if (weaponRotation != null && weaponRotation.animator != null)
        {
            weaponRotation.animator.SetTrigger("Attack");
        }
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
