using System.Collections;
using UnityEngine;

public class SummonCompanion : MonoBehaviour
{
    public GameObject companionPrefab;
    public float summonDuration = 60f;
    public bool limitToOne = true;
    public KeyCode abilityKey = KeyCode.C;
    public float ManaCost = 90f;

    private Transform summonPoint;
    private GameObject activeCompanion;
    PlayerAudioManager audioManager;
    Player player;
    void Start()
    {
        Vector2 summonPoint = transform.position;
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioManager>();
        player = GetComponent<Player>();
    }
    void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            TrySummon();
        }
    }

    public void TrySummon()
    {
        if (limitToOne && activeCompanion != null)
        {
            Debug.Log("Companion already summoned!");
            return;
        }
        player.UseMana(ManaCost);

        Vector3 spawnPosition = summonPoint != null ? summonPoint.position : transform.position;

        GameObject companion = Instantiate(companionPrefab, spawnPosition, Quaternion.identity);
        CompanionAI ai = companion.GetComponent<CompanionAI>();
        ai.player = transform;

        activeCompanion = companion;
        audioManager.PlaySound(audioManager.summon);

        StartCoroutine(DestroyAfterDelay(companion, summonDuration));
    }

    IEnumerator DestroyAfterDelay(GameObject companion, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (companion != null)
        {
            Destroy(companion);
            if (companion == activeCompanion)
                activeCompanion = null;
        }
    }
    public void Summon(GameObject user)
    {
        if (limitToOne && activeCompanion != null)
        {
            Debug.Log("Companion already summoned!");
            return;
        }
        player.UseMana(ManaCost);
        GameObject companion = Instantiate(companionPrefab, user.transform.position, Quaternion.identity);
        companion.GetComponent<CompanionAI>().player = user.transform;
        CompanionAI ai = companion.GetComponent<CompanionAI>();
        ai.player = transform;
        activeCompanion = companion;
        audioManager.PlaySound(audioManager.summon);
        Destroy(companion, summonDuration);
    }
}
