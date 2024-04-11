using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGun : MonoBehaviour
{
    public float Damage;
    public float BulletRange;
    public Transform PlayerCam;

    private LineRenderer _lr;

    public AudioClip audioClip; 
    private AudioSource audioSource;

    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    public void Shoot()
    {
        Ray gunRay = new Ray(PlayerCam.position, PlayerCam.forward);
        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, BulletRange))
        {
            //line renders ray cast
            _lr.SetPositions(new []{PlayerCam.position, hitInfo.point});

            if (hitInfo.collider.gameObject.TryGetComponent(out Entity enemy))
            {
                enemy.Health -= Damage;
            }
        }
        else 
        {
            //line renders ray cast
            _lr.SetPositions(new []{PlayerCam.position, PlayerCam.position + PlayerCam.TransformDirection(Vector3.forward) * 1000});
        }
        audioSource.PlayOneShot(audioClip);

        //snaps line render
        StartCoroutine(LineRenderSnap());
        _lr.enabled = true;
    }

    IEnumerator LineRenderSnap()
    {
        yield return new WaitForSeconds(1f);
        _lr.enabled = false; 
    }
}
