using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls attacks from Cupcake
public class CupcakeAttack : MonoBehaviour
{
    // How much it pushes enemies
    [SerializeField]
    private float pushForce = 10f;
    // How much damage it deals to enemies
    [SerializeField]
    private float cDamage = .25f;
    private void OnTriggerEnter(Collider other)
    {
        // Checks if enemy
        if (other.CompareTag("Enemy"))
        {
            UnityEngine.AI.NavMeshAgent enemyAgent = other.GetComponent<UnityEngine.AI.NavMeshAgent>();

            // Calculate the direction from the trigger to the enemy
            Vector3 directionToEnemy = other.transform.position - transform.position;
            directionToEnemy.y = 1f;
            // Pushes the enemy in the opposite direction
            enemyAgent.Move(directionToEnemy.normalized * pushForce * Time.deltaTime);
            
            // deals damage
            if (other.TryGetComponent(out Entity enemy))
            {
                enemy.Health -= cDamage;
            }
        }
        // Checks if TMachine
        else if (other.CompareTag("TMachine"))
        {            
            // deals damage
            if (other.TryGetComponent(out TMachineEntity TMachine))
            {
                TMachine.Health -= cDamage;
            }
        }
    }
}
