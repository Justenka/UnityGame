using System.Collections;
using UnityEngine;

public class Armagedon : MonoBehaviour
{
    public float damageAmount = 2000f;
    public bool killPlayer = false;
    public KeyCode abilityKey = KeyCode.F;
    PlayerAudioManager audioManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioManager>();
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
        audioManager.PlayExplosion(audioManager.explosion);
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
        if (player != null && killPlayer)
        {
            player.TakeDamage(damageAmount);
        }
        player.UseMana(1000);

        player.UseStamina(1000);

        player.UseHealth(player.stats[StatType.Health].currentValue-1);
    }
    //public void TriggerArmageddon(GameObject user)
    //{
    //    // Damage all entities in the scene
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    foreach (var enemy in enemies)
    //    {
    //        enemy.GetComponent<Enemy>().TakeDamage(2000); // Armageddon damage
    //    }

    //    Player player = user.GetComponent<Player>();
    //    if (player != null)
    //    {
    //        player.TakeDamage(2000); // Armageddon also damages the player
    //    }
    //}
}
