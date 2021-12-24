using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DroneAudio : MonoBehaviour
{
    public AnimationCurve pitchCurve;

    private Vector3 positionPrevious;
    private Vector3 velocityPrevious;

    public AudioClip movingAudio;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        positionPrevious = transform.position;

        source = gameObject.AddComponent<AudioSource>();
        source.clip = movingAudio;
        source.loop = true;
        source.spatialBlend = 1.0f;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        var velocity = position - positionPrevious;
        source.volume = velocity.magnitude;
        source.pitch = pitchCurve.Evaluate(velocity.magnitude);

        positionPrevious = position;
        velocityPrevious = velocity;
    }
}
