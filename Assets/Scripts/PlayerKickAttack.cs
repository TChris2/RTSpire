using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAttack : MonoBehaviour
{
    [SerializeField]
    private float pushForce = 1500f;
    [SerializeField]
    private float kDamage = 1f;
    public bool isKicking = false;
    private void OnTriggerEnter(Collider other)
    {
        // Checks if enemy
        if (other.CompareTag("Enemy"))
        {
            UnityEngine.AI.NavMeshAgent enemyAgent = other.GetComponent<UnityEngine.AI.NavMeshAgent>();

            // Calculate the direction from the trigger to the enemy
            Vector3 directionToEnemy = other.transform.position - transform.position;
            directionToEnemy.y = 1f;
            // Push the enemy in the opposite direction
            enemyAgent.Move(directionToEnemy.normalized * pushForce * Time.deltaTime);
                
            // Deals damage
            if (other.TryGetComponent(out Entity enemy))
            {
                enemy.Health -= kDamage;
            }
        }
        // Checks if TMachine
        else if (other.CompareTag("TMachine"))
        {            
            // Deals damage
            if (other.TryGetComponent(out TMachineEntity TMachine))
            {
                TMachine.Health -= kDamage;
            }
        }
    }
}
