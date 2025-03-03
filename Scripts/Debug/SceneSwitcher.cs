using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    int debugScene = 1;
    int gameScene = 0;


    public void SwitchScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == debugScene)
        {
            SceneManager.LoadScene(gameScene);
            return;
        }

        if (currentSceneIndex == gameScene) 
        {
            SceneManager.LoadScene(debugScene);
            return;
        }
    }


}
