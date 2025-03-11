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

    void Update()
    {
        if(player.currentStamina > 0)
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
    }

    void FixedUpdate()
    {
        Move();
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
}
