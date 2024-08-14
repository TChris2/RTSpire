using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores attack info for players & enemies
public class AttackInfo : MonoBehaviour
{
    //Make Sure when doing upgrades changes to add save/update vars in start func

    // How much damage it deals to enemies
    public float dmg = 0f;
    // How much it pushes enemies in each direction
    public float forForce = 0f;
    public float upForce = 0f;
}
