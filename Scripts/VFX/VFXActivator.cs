using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VFXActivator : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;

    [SerializeField] Renderer meshRenderer;

    AudioSource myAudioSource;

    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (particle != null)
        {
            CheckMeshRenderer();
        }
    }

    void CheckMeshRenderer()
    {
        if (meshRenderer)
        {
            if (meshRenderer.enabled)
            {
                if (myAudioSource.clip != null) { myAudioSource.Play(); }

                particle.gameObject.SetActive(true);
                particle.Play();
            }
            else
            {
                particle.Stop();
            }
        }
    }

}
