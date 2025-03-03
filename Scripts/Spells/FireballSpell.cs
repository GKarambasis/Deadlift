using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpell : Spell
{

    [Header("Charging VFX Settings")]
    public GameObject chargingspellVFX;
    ParticleSystem spellPSLeft;
    ParticleSystem spellPSRight;

    [Header("Projectile Settings")]
    public GameObject projectile;
    public float projectileSpeed = 10;
    bool isChargingLeft;
    bool isChargingRight;


    public void ChargeFireballLeft(Transform originPoint)
    {
        if(playerController.currentAether > aetherCost)
        {
            if(spellPSLeft == null)
            {
                spellPSLeft = Instantiate(chargingspellVFX, originPoint).GetComponent<ParticleSystem>();
                isChargingLeft = true;
            }
        }
    }

    public void CancelChargeFireballLeft()
    {
        if (spellPSLeft != null) 
        {
            spellPSLeft.Stop();
            isChargingLeft=false;
        }
    }

    public void ChargeFireballRight(Transform originPoint)
    {
        if (playerController.currentAether > aetherCost)
        {
            if (spellPSRight == null)
            {
                spellPSRight = Instantiate(chargingspellVFX, originPoint).GetComponent<ParticleSystem>();
                isChargingRight = true;
            }
        }
    }

    public void CancelChargeFireballRight()
    {
        if (spellPSRight != null)
        {
            spellPSRight.Stop();
            isChargingRight = false;
        }
    }

    public void CastFireballLeft(Transform originPoint)
    {
        if (isChargingRight)
        {
            if(SFX)
            {
                audioSourceLH.clip = SFX;
                audioSourceLH.Play();
            }
            
            var currentProjectile = Instantiate(projectile, originPoint.position, originPoint.rotation);
            currentProjectile.GetComponent<Rigidbody>().velocity = -originPoint.right * projectileSpeed;

            playerController.CastSpell(aetherCost);
        }
    }

    public void CastFireballRight(Transform originPoint)
    {
        if (isChargingLeft)
        {
            if (SFX)
            {
                audioSourceRH.clip = SFX;
                audioSourceRH.Play();
            }

            var currentProjectile = Instantiate(projectile, originPoint.position, originPoint.rotation);
            currentProjectile.GetComponent<Rigidbody>().velocity = originPoint.right * projectileSpeed;

            playerController.CastSpell(aetherCost);
        }
    }
}
