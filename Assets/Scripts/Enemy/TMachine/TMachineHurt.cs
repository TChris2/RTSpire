using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMachineHurt : MonoBehaviour
{
    AttackInfo atInfo;
    TMachineEntity TMEntity;
    public bool isHit;
    void Start()
    {
        TMEntity = GetComponent<TMachineEntity>();
        isHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHit)
        {
            if (other.CompareTag("RT Kick") && other.gameObject.name == "Kick Hitbox")
            {
                isHit = true;
                atInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                TMEntity.Health -= atInfo.dmg;
                StartCoroutine(HitCooldown());
            }
            else if (other.CompareTag("RT Punch") && other.gameObject.name == "Punch Hitbox")
            {
                isHit = true;
                atInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                TMEntity.Health -= atInfo.dmg;
                StartCoroutine(HitCooldown());
            }
            else if (other.CompareTag("Cupcake") && other.gameObject.name == "Cupcake Orientate")
            {
                isHit = true;
                atInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                TMEntity.Health -= atInfo.dmg;

                StartCoroutine(HitCooldown());
            }
        }
    }
    
    IEnumerator HitCooldown() {

        yield return new WaitForSeconds(.55f);
        isHit = false;
    }
}
