using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlankGameMode : MonoBehaviour
{
    
    [Header("Activate Enemy Spawner")]
    public bool isActive;

    [Header("Gamemode Settings")]
    int timer; //timer counting up how long the player has been planking for
    const float Multiplier = 1.666f; //transforming seconds into health value (1 min of plank = 100 hp)

    Coroutine currentCoroutine;

    PlayerController playerController;
    CanvasScript canvasScript;

    [Header("Default Health & Aether Values")]
    public int defaultHealth;
    public int defaultAether;

    [Header("Button Settings")]
    public Material selectedMaterial;
    public Material deselectedMaterial;
    public Renderer buttonRenderer;


    [Header("Canvas Settings")]
    [SerializeField] TextMeshPro page1;
    [SerializeField] TextMeshPro page2;
    [SerializeField] TextMeshProUGUI canvasTimerText;

    private ButtonController buttonController;

    // Start is called before the first frame update
    void Start()
    {
        buttonController = FindObjectOfType<ButtonController>();
        playerController = FindObjectOfType<PlayerController>();
        canvasScript = FindObjectOfType<CanvasScript>();

        if (!PlayerPrefs.HasKey("MaxHealth"))
        {
            SetHealth(defaultHealth);
        }
        else
        {
            SetHealth(PlayerPrefs.GetInt("MaxHealth"));
        }

        if (!PlayerPrefs.HasKey("MaxAether"))
        {
            SetAether(defaultAether);
        }
        else
        {
            SetAether(PlayerPrefs.GetInt("MaxAether"));

        }
    }

    public void ToggleGamemode(bool state)
    {
        if (!state)
        {
            isActive = false;
            buttonRenderer.material = deselectedMaterial;

            buttonController.ResetButtons();
        }
        else if (state) 
        {
            isActive = true;
            buttonRenderer.material = selectedMaterial; 

            //Enable GUI Prompting the player to do a plank
            ToggleHUD(true);
        }
    }

    public void ToggleHUD(bool state)
    {
        if (state)
        {
            page1.enabled = true;
            canvasTimerText.enabled = true;
            page2.enabled = true;
        }
        else if (!state)
        {
            page1.enabled = false;
            canvasTimerText.enabled = false;
            page2.enabled = false;
        }
    }

    public void OnPoseDetected()
    {
        if (isActive)
        {
            page2.color = Color.yellow;
            canvasTimerText.color = Color.yellow;


            currentCoroutine = StartCoroutine(CounterCoroutine());

        }
    }

    public void OnPoseLost()
    {
        if(currentCoroutine == null)
        {
            return;
        }
        
        StopCoroutine(currentCoroutine); //Stop The Timer

        UpdateStats(timer); //Update the Score

        ToggleGamemode(false);

        StartCoroutine(ResultsCoroutine());

    }

    //Timer
    IEnumerator CounterCoroutine()
    {
        timer = 0;

        while (true)
        {
            page2.text = "Time: " + timer.ToString();
            canvasTimerText.text = "Time: " + timer.ToString();

            yield return new WaitForSeconds(1);

            timer += 1; 
        }
    }

    IEnumerator ResultsCoroutine()
    {
        page2.color = Color.green;
        canvasTimerText.color = Color.green;

        page2.text = "Final Time: " + timer.ToString() + "\n Health: " + PlayerPrefs.GetInt("MaxHealth").ToString() + "\n Aether: " + PlayerPrefs.GetInt("MaxAether").ToString();

        yield return new WaitForSeconds(5f);
        
        ToggleHUD(false);

    }

    void UpdateStats(int timerResult) 
    {
        int roundedResult = (int)Math.Round(timerResult * Multiplier, 0);

        //Set Max Health
        if(PlayerPrefs.GetInt("MaxHealth") < roundedResult)
        {
            SetHealth(roundedResult);
        }

        //Set Max Aether
        if (PlayerPrefs.GetInt("MaxAether") < roundedResult)
        {
            SetAether(roundedResult);
        }
   
    }

    public void ResetStats()
    {
        SetHealth(defaultHealth);
        SetAether(defaultAether);
    }

    void SetHealth(int newHealth)
    {
        PlayerPrefs.SetInt("MaxHealth", newHealth);
        playerController.maxHealth = newHealth;
        playerController.currentHealth = playerController.maxHealth;

        canvasScript.UpdateMaxHealth(newHealth);
        canvasScript.UpdateHealth(newHealth);
    }

    void SetAether(int newAether)
    {
        PlayerPrefs.SetInt("MaxAether", newAether);
        playerController.maxAether = newAether;
        playerController.currentAether = playerController.maxAether;

        canvasScript.UpdateMaxAether(newAether);
        canvasScript.UpdateAether(newAether);
    }
}
