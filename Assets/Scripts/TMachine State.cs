using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMachineState : MonoBehaviour
{
    [SerializeField]
    private GameObject mOpen;
    [SerializeField]
    private GameObject mClosed;
    public AudioClip ding; 
    private AudioSource audioSource;
    public bool eSpawnTime;
    public TMachineEntity TMEntity;
    [SerializeField]
    private bool isBreakable;
    private Animator TMAni;


    void Start()
    {
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
        TMAni = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (TMEntity != null && TMEntity.isTMachineDestroyed)
            TMAni.SetTrigger("Destroyed");
        else if (!isBreakable && eSpawnTime && !PlayerState.isDead && !PlayerState.isWin || eSpawnTime && !PlayerState.isDead && !PlayerState.isWin && TMEntity != null && !TMEntity.isTMachineDestroyed)
        {
            eSpawnTime = false;
            TMAni.SetTrigger("Open");
            audioSource.PlayOneShot(ding);
            float delay = ding.length; 
            Invoke("MachineClose", delay-1f);
        }
        else if (PlayerState.isDead || PlayerState.isWin)
            TMAni.SetTrigger("Close");
        
    }

    void MachineClose()
    {
        if (!isBreakable || TMEntity.isTMachineDestroyed == false)
        {
            TMAni.SetTrigger("Close");
        }
    }
}
