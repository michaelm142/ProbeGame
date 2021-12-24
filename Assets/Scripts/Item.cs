using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public AudioClip pickupSound;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = FindObjectsOfType<AudioSource>().ToList().Find(s => s.clip == pickupSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Interact()
    {
        source.Play();
        Destroy(gameObject);
    }
}
