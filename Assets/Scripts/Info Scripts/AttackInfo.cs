using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores attack info for players & enemies
public class AttackInfo : MonoBehaviour
{
    // How much damage it deals to enemies
    public float dmg = 0f;
    // How much it pushes enemies in each direction
    public float forForce = 0f;
    public float upForce = 0f;
}
