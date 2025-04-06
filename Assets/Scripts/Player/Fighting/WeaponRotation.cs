using System;
using System.Collections;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer weaponSpriteRenderer, characterSpriteRenderer;

    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;

    public float xOffset = 0.01f;
    public float yOffset = 0.15f;

    void Start()
    {
        mainCamera = Camera.main;
        weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterSpriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = mousePosition - transform.parent.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (direction.x < 0)
        {
            weaponSpriteRenderer.flipY = true;
        }
        else
        {
            weaponSpriteRenderer.flipY = false;
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

    public void Attack()
    {
        if(attackBlocked) return;

        Debug.Log("Triggering Attack animation");
        animator.SetTrigger("Attack");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }
}