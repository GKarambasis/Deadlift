using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Collider myCollider;
    [SerializeField] NPCController myController;
    
    string enemyTeam;
    int weaponDamage;
    
    
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        
        switch (myController.team)
        {
            case NPCController.TeamSelector.Team1: enemyTeam = "Team2"; break;
            case NPCController.TeamSelector.Team2: enemyTeam = "Team1"; break;
            default: return;
        }
        
        weaponDamage = myController.weaponDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTeam))
        {
            if (other.GetComponent<NPCController>() != null)
            {
                NPCController npcController = other.gameObject.GetComponent<NPCController>();
                npcController.Hit(weaponDamage, other.ClosestPoint(transform.position));
                myCollider.enabled = false;
            }
            else if (other.GetComponent<PlayerController>() != null)
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.Hit(weaponDamage);
                myCollider.enabled = false;
            }
            else
            {
                myCollider.enabled = false;
            }
        }
    }
}
