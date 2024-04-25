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
    public static bool eSpawnTime;
    public TMachineEntity TMEntity;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        mOpen.SetActive(false);
    }

    void Update()
    {
        if (eSpawnTime == true && TMEntity.isTMachineDestroyed == false && !PlayerState.isDead)
        {
            eSpawnTime = false;
            mClosed.SetActive(false);
            mOpen.SetActive(true);
            audioSource.PlayOneShot(ding);
            float delay = ding.length; 
            Invoke("MachineClose", delay-1);
        }
    }

    void MachineClose()
    {
        if (TMEntity.isTMachineDestroyed == false)
        {
            mOpen.SetActive(false);
            mClosed.SetActive(true);
        }
    }
}
