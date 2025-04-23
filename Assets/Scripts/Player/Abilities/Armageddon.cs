using System.Collections;
using UnityEngine;

public class Armagedon : MonoBehaviour
{
    public float damageAmount = 2000f;
    public bool killPlayer = false;
    public KeyCode abilityKey = KeyCode.F;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            Activate();
        }
    }
    public void Activate()
    {
        // Damage all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damageAmount);
            }
        }

        // Damage player
        Player player = GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
}
