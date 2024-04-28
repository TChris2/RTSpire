using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    [SerializeField]
    private Animator shakeStartScreen;

    void Start()
    {
        shakeStartScreen.speed = 0.5f;
    }
}
