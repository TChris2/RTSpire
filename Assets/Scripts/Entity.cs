using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls health of objects and determines when they get destroyed
public class Entity : MonoBehaviour
{
    public float StartHealth;
    private float health;
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            
            //destroys objects when their health reaches 0
            if (health <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        //sets health
        Health = StartHealth;
    }
}
