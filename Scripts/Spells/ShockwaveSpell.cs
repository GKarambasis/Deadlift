using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveSpell : Spell
{

    [Header("Shockwave Settings")]
    public float chargeTime = 3;
    public AudioClip chargeSFX;

    [Header("Charging VFX Settings")]
    public GameObject chargingspellVFXRight;
    public GameObject chargingspellVFXLeft;
    ParticleSystem spellPSLeft;
    ParticleSystem spellPSRight;

    [Header("Projectile Settings")]
    public GameObject projectile;
    public Transform projectileOrigin;
    bool isChargingLeft;
    bool isChargingRight;

    Coroutine currentCoroutine;

    public void ChargeShockwaveLeft(Transform originPoint)
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

    public void CancelChargeShockwaveLeft()
    {
        if (spellPSLeft != null)
        {
            if (audioSourceLH.isPlaying)
            {
                audioSourceLH.Stop();
            }

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            spellPSLeft.Stop();
            isChargingLeft = false;
        }
    }

    public void ChargeShockwaveRight(Transform originPoint)
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

    public void CancelChargeShockwaveRight()
    {
        if (spellPSRight != null)
        {
            if (audioSourceRH.isPlaying)
            {
                audioSourceRH.Stop();
            }

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            spellPSRight.Stop();
            isChargingRight = false;
        }
    }

    public void CastShockwave()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            return;
        }
        
        if(isChargingLeft && isChargingRight)
        {
            currentCoroutine = StartCoroutine(CastShockwaveCoroutine());
        }


    }

    IEnumerator CastShockwaveCoroutine()
    {
        audioSourceLH.clip = chargeSFX;
        audioSourceLH.Play();

        audioSourceRH.clip = chargeSFX;
        audioSourceRH.Play();

        yield return new WaitForSeconds(2f);
        
        Instantiate(projectile, projectileOrigin);
        playerController.CastSpell(aetherCost);
        CancelChargeShockwaveLeft();
        CancelChargeShockwaveRight();

        currentCoroutine = null;
    }
}
