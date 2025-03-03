using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoseVisualisation : MonoBehaviour
{
    private TextMeshPro textMesh;

    public string poseName;
    public int poseDetectedCount = 0;

    public bool isActive;
    
    // Start is called before the first frame update
    void Start()
    {
        
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = poseName + ": " + poseDetectedCount.ToString();

        
        if (!isActive)
        {
            textMesh.enabled = false;
        }

    }

    // Update is called once per frame
    public void OnPoseDetected()
    {
        if (!isActive)
        {
            return;
        }

        poseDetectedCount++;
        
        textMesh.text = poseName + ": " +poseDetectedCount.ToString();
        textMesh.color = Color.green;
    }

    public void OnPoseLost()
    {
        if (!isActive)
        {
            return;
        }

        textMesh.color = Color.red;
    }
}
