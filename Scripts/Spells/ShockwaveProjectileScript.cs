using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveProjectileScript : MonoBehaviour
{
    [Header("ProjectileSettings")]
    [SerializeField] int projectileDamage = 50;

    [Header("Collider Explansion")]
    public float minColliderRadius = 0f;
    public float maxColliderRadius = 4f;

    public float colliderRadiusDuration = 0.8f;

    bool isExpanding = false;
    float elapsedTime = 0f;
    float t = 0;

    SphereCollider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        isExpanding = true;
        Destroy(gameObject, 3);
    }

    // Gradually increase collider radius
    void Update()
    {
        ExpandTriggerArea();
    }

    void ExpandTriggerArea()
    {
        if (isExpanding)
        {
            t = elapsedTime / colliderRadiusDuration;

            elapsedTime += Time.deltaTime;

            myCollider.radius = Mathf.Lerp(minColliderRadius, maxColliderRadius, t);

            if (t > 1)
            {
                isExpanding = false;
                myCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Team2")
        {
            NPCController controller = other.GetComponent<NPCController>();
            controller.Hit(projectileDamage, other.ClosestPoint(transform.position));
        }
    }
}
