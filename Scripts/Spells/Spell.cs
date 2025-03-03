using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [Header("General Spell Settings")]
    public AudioSource audioSourceLH; //Audio Source for Left Hand
    public AudioSource audioSourceRH; //Audio Source for Right Hand
    public AudioClip SFX;
    public int aetherCost;

    protected PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

}
