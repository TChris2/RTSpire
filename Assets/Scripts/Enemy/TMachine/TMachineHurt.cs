using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// How TMachines are hit and effects of different on them by different player attacks
public class TMachineHurt : MonoBehaviour
{
    // Gets attack info from the player attack
    AttackInfo atkInfo;
    // Gets health for entity
    TMachineEntity TMEntity;
    // Controls when player can attack the enemy again
    public bool isHit;
    void Start()
    {
        // Gets the enemy's components
        TMEntity = GetComponent<TMachineEntity>();
        isHit = false;
    }

    // When the TMachine enters the collider of one of the player's attacks
    private void OnTriggerEnter(Collider other)
    {
        // Checks to see if the enemy has already been hit
        if (!isHit)
        {
            // Checks to see which attack the enemy is being attacked by
            // If RT attack
            if (other.CompareTag("RT Attack") && other.gameObject.name == "Attack Hitbox")
            {
                // Sets to true to prevent the enemy being hit multiple times while hit
                isHit = true;
                // Gets attack info
                atkInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                TMEntity.Health -= atkInfo.dmg;

                // Starts hit cooldown
                StartCoroutine(HitCooldown());
            }
            // If Cupcake
            else if (other.CompareTag("Cupcake") && other.gameObject.name == "Cupcake Orientate")
            {
                // Sets to true to prevent the enemy being hit multiple times while hit
                isHit = true;
                // Gets attack info
                atkInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                TMEntity.Health -= atkInfo.dmg;

                // Starts hit cooldown
                StartCoroutine(HitCooldown());
            }
        }
    }
    
    // Cooldown for when the TMachine can be attack again
    IEnumerator HitCooldown() {
        yield return new WaitForSeconds(.1f);
        isHit = false;
    }
}
