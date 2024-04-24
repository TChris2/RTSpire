using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Transform player;
    private bool canAttack = true;
    TMPro.TMP_Text healthtext;


    void Start()
    {
        // gets player object
        player = GameObject.Find("Player").GetComponent<Transform>();
        // gets health text
        healthtext = GameObject.Find("Health").GetComponent<TMPro.TMP_Text>();
        // for attacking the player
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true)
        {
            // can only attack the player while there is no cooldown and the player has positive amount of health
            if (canAttack /*&& LevelReload.health > 0*/)
            {
                // checks the distance between the enemy and the player
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // if the distance is less than or equal to 2 the enemy can attack
                if (distanceToPlayer <= 2f)
                {
                    // health is decreased and the text ui is updated
                    /*LevelReload.health -= 10;
                    healthtext.text = $"Health: {LevelReload.health}";*/
                    // attack cooldown activates
                    canAttack = false;
                    yield return new WaitForSeconds(1f);
                    canAttack = true;
                }
            }
            yield return null;
        }
    }
}
