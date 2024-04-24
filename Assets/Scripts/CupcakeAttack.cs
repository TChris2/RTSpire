using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeAttack : MonoBehaviour
{
    [SerializeField]
    private float pushForce = 10f;
    [SerializeField]
    private float cDamage = .25f;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider has the tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            UnityEngine.AI.NavMeshAgent enemyAgent = other.GetComponent<UnityEngine.AI.NavMeshAgent>();

            // Calculate the direction from the trigger to the enemy
            Vector3 directionToEnemy = other.transform.position - transform.position;
            directionToEnemy.y = 1f;
            // Push the enemy in the opposite direction
            enemyAgent.Move(directionToEnemy.normalized * pushForce * Time.deltaTime);
            
            if (other.TryGetComponent(out Entity enemy))
            {
                enemy.Health -= cDamage;
            }
        }
        // Check if the entering collider has the tag "Enemy"
        else if (other.CompareTag("TMachine"))
        {            
            if (other.TryGetComponent(out TMachineEntity TMachine))
            {
                TMachine.Health -= cDamage;
            }
        }
    }
}
