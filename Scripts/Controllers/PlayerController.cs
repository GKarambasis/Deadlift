using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Health Info")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    public GameObject healthRegenVFX;
    public AudioClip damagedSFX;

    [Header("Aether Info")]
    public int maxAether = 100;
    public int currentAether = 100;
    public GameObject aetherRegenVFX;
    public AudioClip aetherRegenSFX;

    AudioSource myAudioSource;

    [Header("Excercise Settings")]
    [SerializeField] Transform headPosition;
    [SerializeField] Transform groundPosition;

    [Header("Squat Settings")]
    [SerializeField]
    [Range(0, 2f)] float minSquatDistance;
    [SerializeField]
    [Range(0, 2f)] float maxSquatDistance;

    [Header("Pushup Settings")]
    [SerializeField]
    [Range(0, 2f)] float minPushupDistance;
    [SerializeField]
    [Range(0, 2f)] float maxPushupDistance;

    [Header("UI")]
    public TextMeshProUGUI deathText;

    bool isDead;
    
    CanvasScript canvasScript;
    HUDFlash hudFlash;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        canvasScript = FindObjectOfType<CanvasScript>();
        hudFlash = FindObjectOfType<HUDFlash>();
        
    }

    public void Hit(int damage)
    {
        hudFlash.FadeIn();
        currentHealth -= damage;
        canvasScript.UpdateHealth(currentHealth);

        if(damagedSFX != null)
        {
            myAudioSource.clip = damagedSFX;
            myAudioSource.Play();
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void CastSpell(int aetherCost)
    {
        currentAether -= aetherCost;
        canvasScript.UpdateAether(currentAether);
    }

    public void RegenAether(int aetherAmount)
    {
        //VFX & SFX
        Instantiate(aetherRegenVFX, gameObject.transform);
        myAudioSource.clip = aetherRegenSFX;
        myAudioSource.Play();

        if (currentAether + aetherAmount > maxAether)
        {
            currentAether = maxAether;
            canvasScript.UpdateAether(currentAether);
            return;
        }
        
        currentAether += aetherAmount;
        canvasScript.UpdateAether(currentAether);

    }

    public void RegenHealth(int healthAmount)
    {
        //VFX
        Instantiate(healthRegenVFX, gameObject.transform);


        if (currentHealth + healthAmount > maxHealth)
        {
            currentHealth = maxHealth;
            canvasScript.UpdateHealth(currentHealth);
            return;
        }

        currentHealth += healthAmount;
        canvasScript.UpdateHealth(currentHealth);

    }

    public void PerformSquat(int aetherAmount)
    {
        float distanceFromGround = headPosition.position.y - groundPosition.position.y;

        if(distanceFromGround > minSquatDistance && distanceFromGround <= maxSquatDistance)
        {
            RegenAether(aetherAmount);
        }
    }

    public void PerformPushup(int aetherAmount)
    {
        float distanceFromGround = headPosition.position.y - groundPosition.position.y;

        if (distanceFromGround > minPushupDistance && distanceFromGround <= maxPushupDistance)
        {
            RegenAether(aetherAmount);
            RegenHealth(aetherAmount);
        }
        else
        {
            //RegenHealth(aetherAmount);
        }
    }

    void Death()
    {
        if (!isDead)
        {
            isDead = true;
            StartCoroutine(DeathCoroutine());
        }
    }

    
    IEnumerator DeathCoroutine()
    {
        deathText.color = Color.red;
        deathText.enabled = true;
        deathText.text = "You Died!";

        yield return new WaitForSeconds(5);

        deathText.enabled = false;

        SceneManager.LoadScene(1);
    }

}
