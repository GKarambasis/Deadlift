using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class NPCController : MonoBehaviour
{
    [Header("Generic Info")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int weaponDamage;
    bool isDead;
    private Rigidbody[] ragdollBodies;

    [Header("Attack Settings")]
    public bool isActive = true;
    public float attackRange = 2f;          // Range within which the NPC attacks
    public float attackCooldown = 2f;       // Cooldown time between attacks
    public float minAttackCooldown = 1f;
    public float maxAttackCooldown = 3f;
    public float rotationSpeed = 5f;        // Rotation speed for looking at target when attacking

    private Transform targetEnemy;         // Current player targetEnemy
    private float lastAttackTime = 0f;      // Time since the last attack
    
    public enum TeamSelector
    {
        Team1,
        Team2
    }
    public TeamSelector team;              // Team Selector

    [Header("Sound Settings")]
    [SerializeField] AudioClip[] attackSFX;
    [SerializeField] AudioClip[] deathSFX;

    [Header("Cleanup Settings")]
    public float deleteBodyTimer = 15f;    //Time after death to wait before Deleting Gameobject


    NavMeshAgent agent;             
    Animator animator;
    WeaponScript weaponScript;
    EnemySpawner enemySpawner;
    Collider myCollider;
    AudioSource audioSource;

    private void Awake()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weaponScript = GetComponentInChildren<WeaponScript>();
        myCollider = GetComponent<Collider>();

        if (FindObjectOfType<EnemySpawner>() != null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }

        currentHealth = maxHealth;
        
        switch (team)
        {
            case TeamSelector.Team1:
                gameObject.tag = "Team1";
                break;

            case TeamSelector.Team2:
                gameObject.tag = "Team2";
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        if (!isDead)
        {
            ScanForEnemies();

            if (targetEnemy != null)
            {
                MoveToTarget();

                if (Vector3.Distance(transform.position, targetEnemy.position) <= attackRange)
                {
                    AttackEnemy();
                }
            }

            UpdateAnimations();
        }
    }

    void ScanForEnemies()
    {
        string enemyTeam;
        
        switch (team)
        {
            case TeamSelector.Team1:
                enemyTeam = "Team2";
                break;

            case TeamSelector.Team2:
                enemyTeam = "Team1";
                break;

            default:
                return;
        }

        // Find all game objects with the opposing team's tag
        GameObject[] players = GameObject.FindGameObjectsWithTag(enemyTeam); 
        
        
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity; 

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                closestEnemy = player.transform;
                closestDistance = distanceToPlayer;
            }
        }

        // Assign closest player, or null if none found
        targetEnemy = closestEnemy; 
    }

    void MoveToTarget()
    {
        if (targetEnemy != null)
        {
            agent.SetDestination(targetEnemy.position);
            
        }
    }

    void AttackEnemy()
    {
        //Look at enemy
        Vector3 directionToTarget = (targetEnemy.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            
            string Attack;

            switch(Random.Range(0, 3))
            {
                case 0: Attack = "Attack1"; break;
                case 1: Attack = "Attack2"; break;
                case 2: Attack = "Attack3"; break;
                default: return;
            }
            
            animator.SetTrigger(Attack);

            StartCoroutine(PerformAttack());

            lastAttackTime = Time.time; // Update last attack time

            //Play SFX
            if (attackSFX.Length > 0)
            {
                audioSource.clip = attackSFX[Random.Range(0, attackSFX.Length)];
                audioSource.Play();
            }

            attackCooldown = Random.Range(minAttackCooldown, maxAttackCooldown); //update the next attack time
        }
    }

    IEnumerator PerformAttack()
    {
        weaponScript.myCollider.enabled = true;

        yield return new WaitForSeconds(2);

        weaponScript.myCollider.enabled = false;
    }

    void UpdateAnimations()
    {
        //animator.SetFloat("HorizontalSpeed", agent.velocity.magnitude);
        animator.SetFloat("VerticalSpeed", agent.velocity.magnitude);
    }



    public void Hit(int damage)
    {
        animator.SetTrigger("Hit");
        
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Death(); 
        }
    }
    public void Hit(int damage, Vector3 forceDirection)
    {
        animator.SetTrigger("Hit");

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Death();
            ApplyRagdollForce(forceDirection, damage/2);
        }        
    }

    void Death()
    {
        if (!isDead)
        {
            isDead = true;
            
            //Play SFX
            if (deathSFX.Length > 0) 
            {
                audioSource.clip = deathSFX[Random.Range(0, deathSFX.Length)];
                audioSource.Play();            
            }

            EnableRagdoll();

            gameObject.tag = "Untagged";

            //Remove the NPC from the alive Enemies list
            if (enemySpawner && enemySpawner.aliveEnemies.Contains(gameObject))
            {
                enemySpawner.aliveEnemies.Remove(gameObject);
            }

            StartCoroutine(DestroyNPC());
        }
    }

    IEnumerator DestroyNPC()
    {
        yield return new WaitForSeconds(deleteBodyTimer);

        Destroy(gameObject);

    }

    void EnableRagdoll()
    {
        animator.enabled = false;
        agent.enabled = false;
        myCollider.enabled = false;
    }

    void ApplyRagdollForce(Vector3 collisionPoint, float forceIntensity)
    {
        Vector3 forceDirection = (transform.position - collisionPoint).normalized;
        
        foreach (Rigidbody rb in ragdollBodies)
        {

            rb.AddForce(forceDirection * forceIntensity, ForceMode.Impulse); // Adjust force value as needed
        }
    }

}
