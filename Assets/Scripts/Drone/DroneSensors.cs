using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSensors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var collider = gameObject.AddComponent<SphereCollider>();
        var body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
        collider.radius = 10.0f;
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
