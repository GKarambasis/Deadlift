using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent (typeof(Rigidbody))]
public class FireballProjectileScript : MonoBehaviour
{

    [SerializeField] GameObject impactVFX;
    [SerializeField] AudioClip impactSFX;
    [SerializeField] int projectileDamage = 10;
    public float projectileLife = 10;
    AudioSource audioSource;
    Collider myCollider;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(impactSFX != null)
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
        if (collision.gameObject.tag == "Team1")
        {
            return;
        }
        
        myCollider.enabled = false;
        var impact = Instantiate(impactVFX, collision.GetContact(0).point, Quaternion.identity);
        
        if(collision.gameObject.tag == "Team2")
        {
            collision.gameObject.GetComponent<NPCController>().Hit(projectileDamage, collision.GetContact(0).point);
        }
    
        if (audioSource.clip)
        {
            audioSource.Play();
        }
        
        Destroy(gameObject, 0.2f);
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(projectileLife);

        Destroy(gameObject);
    }
}
