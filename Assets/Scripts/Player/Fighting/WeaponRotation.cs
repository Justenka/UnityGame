using System;
using System.Collections;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer weaponSpriteRenderer, characterSpriteRenderer;

    public Animator animator;
    public float delay = 0.12f;
    private bool attackBlocked;

    public float xOffset = 0.01f;
    public float yOffset = 0.15f;

    private Quaternion cachedRotation;
    private Vector3 cachedScale;

    private Player player;

    public Transform circleOrigin;
    public float radius;

    void Start()
    {
        mainCamera = Camera.main;
        weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterSpriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (!attackBlocked)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector3 direction = mousePosition - transform.parent.position;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (direction.x < 0)
            {
                transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (characterSpriteRenderer.flipX)
            {
                transform.localPosition = new Vector3(-xOffset, yOffset, 0);
            }
            else
            {
                transform.localPosition = new Vector3(xOffset, yOffset, 0);
            }

            if (transform.eulerAngles.z > 15 && transform.eulerAngles.z < 165)
            {
                weaponSpriteRenderer.sortingOrder = characterSpriteRenderer.sortingOrder - 1;
            }
            else
            {
                weaponSpriteRenderer.sortingOrder = characterSpriteRenderer.sortingOrder + 1;
            }
        }
        else
        {
            transform.rotation = cachedRotation;
            transform.localScale = cachedScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!attackBlocked) return;

        Attack();
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            WeaponItem weapon = player.GetComponent<EquipmentManager>().equippedWeapon;
            if (weapon != null)
            {
                float damage = player.stats[StatType.Attack].Total;
                float knockBack = 5f;
                bool doesKnockBack = true;
                enemy.TakeDamage(damage, transform.position, knockBack, doesKnockBack);
            }
        }
    }

    public bool Attack()
    {
        if (attackBlocked)
            return false;

        Debug.Log("Triggering Attack animation");

        // Cache current rotation and scale
        cachedRotation = transform.rotation;
        cachedScale = transform.localScale;

        animator.SetTrigger("Attack");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
        return true;
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }
    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                WeaponItem weapon = player.GetComponent<EquipmentManager>().equippedWeapon;
                if (weapon != null)
                {
                    float damage = player.stats[StatType.Attack].Total;
                    float knockBack = 5f;
                    bool doesKnockBack = true;
                    enemy.TakeDamage(damage, transform.position, knockBack, doesKnockBack);
                }
            }
        }
    }

}