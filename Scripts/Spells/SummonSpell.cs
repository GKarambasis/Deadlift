using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SummonSpell : Spell
{

    [Header("Charging VFX Settings")]
    public GameObject chargingspellVFXRight;
    public GameObject chargingspellVFXLeft;
    ParticleSystem spellPSLeft;
    ParticleSystem spellPSRight;
    public float spellCooldown = 5;
    bool coolingDown;

    [Header("Projectile Settings")]
    public GameObject projectile;
    public float projectileSpeed = 10;
    bool isChargingLeft;
    bool isChargingRight;

    public void ChargeSummonLeft(Transform originPoint)
    {
        if (playerController.currentAether > aetherCost)
        {
            if (spellPSLeft == null)
            {
                audioSourceLH.clip = SFX;
                audioSourceLH.Play();

                spellPSLeft = Instantiate(chargingspellVFXLeft, originPoint).GetComponent<ParticleSystem>();
                isChargingLeft = true;
            }
        }
    }

    public void CancelChargeSummonLeft()
    {
        if (spellPSLeft != null)
        {
            if (audioSourceLH.isPlaying)
            {
                audioSourceLH.Stop();
            }

            spellPSLeft.Stop();
            isChargingLeft = false;
        }
    }

    public void ChargeSummonRight(Transform originPoint)
    {
        if (playerController.currentAether > aetherCost)
        {
            if (spellPSRight == null)
            {
                audioSourceRH.clip = SFX;
                audioSourceRH.Play();

                spellPSRight = Instantiate(chargingspellVFXRight, originPoint).GetComponent<ParticleSystem>();
                isChargingRight = true;
            }
        }
    }

    public void CancelChargeSummonRight()
    {
        if (spellPSRight != null)
        {
            if (audioSourceRH.isPlaying)
            {
                audioSourceRH.Stop();
            }

            spellPSRight.Stop();
            isChargingRight = false;
        }
    }

    public void CastSummonLeft(Transform originPoint)
    {
        if (isChargingRight && !coolingDown)
        {
            StartCoroutine(Cooldown());
            var currentProjectile = Instantiate(projectile, originPoint.position, originPoint.rotation);
            currentProjectile.GetComponent<Rigidbody>().velocity = -originPoint.right * projectileSpeed;

            playerController.CastSpell(aetherCost);
        }
    }

    public void CastSummonRight(Transform originPoint)
    {
        if (isChargingLeft && !coolingDown)
        {
            StartCoroutine(Cooldown());
            var currentProjectile = Instantiate(projectile, originPoint.position, originPoint.rotation);
            currentProjectile.GetComponent<Rigidbody>().velocity = originPoint.right * projectileSpeed;

            playerController.CastSpell(aetherCost);
        }
    }

    IEnumerator Cooldown()
    {
        coolingDown = true;

        yield return new WaitForSeconds(spellCooldown);

        coolingDown = false;
    }
}
