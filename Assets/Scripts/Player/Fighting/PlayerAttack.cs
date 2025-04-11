using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private float angle = 0f;

    private WeaponRotation weaponRotation;

    void Start()
    {
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
            }
        }

        RotateWeaponAroundPlayer();
    }

    public void DoAttack()
    {
        attacking = true;

        if (weaponRotation != null)
        {
            weaponRotation.Attack();
        }
    }

    private void RotateWeaponAroundPlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
