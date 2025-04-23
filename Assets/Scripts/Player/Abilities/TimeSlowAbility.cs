using UnityEngine;
using System.Collections;

public class TimeSlowAbility : MonoBehaviour
{
    [Header("Time Slow Settings")]
    public float slowFactor = 0.1f;// 0 = frozen, 1 = normal speed
    public float slowDuration = 2f;
    public float cooldownTime = 5f;
    private float nextUseTime = 0f;
    public KeyCode abilityKey = KeyCode.E;

    private bool isSlowingTime = false;

    private PlayerMovement playerMovement;
    private float originalSpeedMultiplier;
    PlayerAudioManager audioManager;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(abilityKey) && Time.time >= nextUseTime && !isSlowingTime)
        {
            audioManager.PlaySound(audioManager.slowtime);
            nextUseTime = Time.time + cooldownTime;
            StartCoroutine(SlowTime());
        }
    }

    public IEnumerator SlowTime()
    {
        isSlowingTime = true;

        // Slow down time
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        originalSpeedMultiplier = playerMovement.GetCurrentSpeedMultiplier();

        playerMovement.ModifySpeedMultiplier(originalSpeedMultiplier / slowFactor);
        Debug.Log("Slow");
        yield return new WaitForSecondsRealtime(slowDuration);

        // Restore time
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        playerMovement.ModifySpeedMultiplier(originalSpeedMultiplier);

        isSlowingTime = false;
    }
    public void TimeSlow(GameObject user)
    {
        if (!isSlowingTime)
        {
            audioManager.PlaySound(audioManager.slowtime);
            StartCoroutine(SlowTime());
        }
    }
}
