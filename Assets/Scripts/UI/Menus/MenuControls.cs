using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public void LoadLevel (int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(2);
    }
}
