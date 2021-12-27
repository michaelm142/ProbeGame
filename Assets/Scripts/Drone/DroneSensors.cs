using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSensors : MonoBehaviour
{
    public float Radius = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        var collider = gameObject.AddComponent<SphereCollider>();
        var body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            other.GetComponent<MiniMapIcon>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
            other.GetComponent<MiniMapIcon>().enabled = false;
    }
}
