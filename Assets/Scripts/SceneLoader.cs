using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    Scene currentScene;

    void Awake() 
    {
        currentScene = SceneManager.GetActiveScene();
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(currentScene.buildIndex);
    }


}
