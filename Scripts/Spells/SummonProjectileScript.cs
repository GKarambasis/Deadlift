using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonProjectileScript : MonoBehaviour
{
    [SerializeField] GameObject impactVFX;
    [SerializeField] AudioClip impactSFX;
    public float projectileLife = 10;
    AudioSource audioSource;
    Collider myCollider;

    [SerializeField]
    GameObject SummonedCreature;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (impactSFX != null)
        {
            audioSource.clip = impactSFX;
        }

        myCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        StartCoroutine(DestroyProjectile());
    }

    private void OnCollisionEnter(Collision collision)
    {
        myCollider.enabled = false;

        Instantiate(impactVFX, collision.GetContact(0).point, Quaternion.Euler(-90, 0, 0));
        Instantiate(SummonedCreature, collision.GetContact(0).point, Quaternion.identity);

        if (audioSource.clip)
        {
            audioSource.Play();
        }

        Destroy(gameObject, 3);   
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(projectileLife);

        Destroy(gameObject);
    }
}
