using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles how damage is dealt to TMachines
public class TMachineHurt : MonoBehaviour
{
    // Gets attack info from the player attack
    AttackInfo atkInfo;
    // Gets health for entity
    TMachineEntity TMEntity;
    // Controls when player can attack the enemy again
    [HideInInspector]
    public bool isHit = false;
    void Start()
    {
        // Gets components
        TMEntity = GetComponent<TMachineEntity>();
    }

    // When the player's attack enters the TMachine's collider
    private void OnTriggerEnter(Collider other)
    {
        // Checks to see if the TMachine has already been hit
        if (!isHit)
        {
            // Checks if either of the player's attacks have enter the TMachines collider
            if ((other.CompareTag("RT Attack") || other.CompareTag("Cupcake")) && other.GetComponent<AttackInfo>())
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
