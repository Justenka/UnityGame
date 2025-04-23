using System.Collections;
using UnityEngine;

public class SummonCompanion : MonoBehaviour
{
    public GameObject companionPrefab;
    public float summonDuration = 60f;
    public bool limitToOne = true;
    public KeyCode abilityKey = KeyCode.C;

    private Transform summonPoint;
    private GameObject activeCompanion;
    void Start()
    {
        Vector2 summonPoint = transform.position;
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

        Vector3 spawnPosition = summonPoint != null ? summonPoint.position : transform.position;

        GameObject companion = Instantiate(companionPrefab, spawnPosition, Quaternion.identity);
        CompanionAI ai = companion.GetComponent<CompanionAI>();
        ai.player = transform;

        activeCompanion = companion;

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
}
