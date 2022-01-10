using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDAudioController : MonoBehaviour
{
    private static HUDAudioController instance;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Debug.LogError("More than one HUDAudioController");
        instance = this;

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(AudioClip clip)
    {
        instance.source.clip = clip;
        instance.source.Play();
    }
}
