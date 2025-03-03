using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] PokeInteractable[] buttons;

    public void ButtonClicked(PokeInteractable buttonClicked)
    {
        foreach (PokeInteractable button in buttons)
        {
            if(buttonClicked == button)
            {
                continue;
            }
            
            button.Disable();
        }
    }

    public void ResetButtons()
    {
        foreach(PokeInteractable button in buttons)
        {
            button.Enable();
        }
    }

}
