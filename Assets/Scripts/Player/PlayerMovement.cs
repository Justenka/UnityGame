using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public Player player;
    public float moveSpeed;
    public Rigidbody2D rigidBody;
    private Vector2 moveDirection;
    public bool running = false;
    public float runCost;

    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    public float dashCost = 20f;

    void Update()
    {
        if (player.currentStamina > 0)
        {
            if (Input.GetKeyDown("left shift"))
            {
                running = true;
            }
            else if (Input.GetKeyUp("left shift"))
            {
                running = false;
            }
        }
        else running = false;

        ProcessInput();
        if (player.currentStamina >= 20)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canDash)
            {
                StartCoroutine(Dash());
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        if (running)
        {
            rigidBody.linearVelocity = new Vector2(moveDirection.x * moveSpeed * 2, moveDirection.y * moveSpeed * 2);
            player.UseStamina(runCost * Time.deltaTime);
        }
        else
        {
            rigidBody.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        //float originalGravity = rigidBody.gravityScale;
        //rigidBody.gravityScale = 0f;
        rigidBody.linearVelocity = moveDirection * dashingPower;
        player.UseStamina(dashCost);
        yield return new WaitForSeconds(dashingTime);
        //rigidBody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}