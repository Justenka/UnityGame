using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float maxStamina = 100;
    public float currentStamina;
    public float moveSpeed;
    public Rigidbody2D rigidBody;

    private Vector2 moveDirection;
    public bool running = false;
    public float runCost;
    public float ChargeRate;
    public StaminaBar staminaBar;

    private Coroutine recharge;

    private void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }
    void Update()
    {
        if (Input.GetKeyDown("left shift"))
        {
            running = true;
        }
        else if (Input.GetKeyUp("left shift"))
        {
            running = false;
        }
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
            UseStamina(runCost * Time.deltaTime);
        }
        else
        {
            rigidBody.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
    }
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0) currentStamina = 0;
        staminaBar.SetStamina(currentStamina);
        if (recharge != null) StopCoroutine(recharge);
        recharge = StartCoroutine(RechargeStamina());
    }
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (currentStamina < maxStamina)
        {
            currentStamina += ChargeRate / 10f;
            staminaBar.SetStamina(currentStamina);
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            yield return new WaitForSeconds(.1f);

        }
    }
}
