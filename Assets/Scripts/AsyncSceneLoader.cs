using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    public static void LoadScene(string sceneName)
    {
        GameObject obj = new GameObject("Scene Loader");
        var loader = obj.AddComponent<AsyncSceneLoader>();
        var instance = Instantiate(obj);
        DontDestroyOnLoad(instance);
        instance.GetComponent<AsyncSceneLoader>().sceneName = sceneName;
        SceneManager.LoadScene("LoadingScreen");
    }

    public string sceneName;

    const float Lifetime = 1.0f;
    private float timer;
    
    private void Start()
    {
        timer = Lifetime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SceneManager.LoadSceneAsync(sceneName);
            LoadSave.SaveProgress();
            Destroy(gameObject);
        }
    }
}
