using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    public Player player;
    public Rigidbody2D rigidBody;
    public Animator animator;

    private Vector2 moveDirection;
    public bool running = false;

    private bool canDash = true;
    private bool isDashing;

    public float dashCost = 20f;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    public static event Action OnPlayerMoved;
    public float speedMultiplier = 1f;
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player.stats[StatType.Stamina].currentValue > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                running = true;
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                running = false;
        }
        else running = false;

        ProcessInput();
        HandleMovementSounds();

        if (player.stats[StatType.Stamina].currentValue >= dashCost)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canDash)
            {
                StartCoroutine(Dash());
            }
        }

        FlipCharacter();
    }

    void FixedUpdate()
    {
        if (!isDashing)
            Move();

        animator.SetFloat("xVelocity", rigidBody.linearVelocity.magnitude);
    }

    void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        float speed = player.stats[StatType.Speed].Total * speedMultiplier;
        float moveMultiplier = running ? 2f : 1f;
        OnPlayerMoved?.Invoke();
        rigidBody.linearVelocity = moveDirection * speed * moveMultiplier;

        if (running & moveDirection.magnitude > 0)
        {
            float runCostPerSecond = 10f;
            player.UseStamina(runCostPerSecond * Time.deltaTime);
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        rigidBody.linearVelocity = moveDirection * dashingPower;
        player.UseStamina(dashCost);

        yield return new WaitForSeconds(dashingTime);

        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;
    }

    void FlipCharacter()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = (mouseWorldPosition.x < transform.position.x) ? Vector3.right : Vector3.left;
    }
    public void ModifySpeedMultiplier(float newMultiplier)
    {
        speedMultiplier = newMultiplier;
    }
    public float GetCurrentSpeedMultiplier()
    {
        return speedMultiplier;
    }



    void HandleMovementSounds()
    {
        bool isMoving = moveDirection.magnitude > 0f && !isDashing;

        PlayerAudioManager audioManager = player.GetComponent<PlayerAudioManager>();
        if (audioManager != null)
        {
            if (isMoving)
            {
                if (running)
                {
                    audioManager.PlayLoop(audioManager.runningAudioSource);
                }
                else
                {
                    audioManager.PlayLoop(audioManager.walkingAudioSource);
                }
            }
            else
            {
                audioManager.StopLoop();
            }
        }
    }
}
