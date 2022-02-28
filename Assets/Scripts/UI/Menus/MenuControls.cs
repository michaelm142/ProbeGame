using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public void LoadLevelAsync(string name)
    {
        AsyncSceneLoader.LoadScene("VonBron");
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadLevel (int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadNextLevel()
    {
        AsyncSceneLoader.LoadScene("VonBron");
    }
}
