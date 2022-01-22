using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSensors : MonoBehaviour
{
    public float Radius = 10.0f;

    private List<GameObject> localEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        var collider = gameObject.AddComponent<SphereCollider>();
        var body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
        collider.isTrigger = true;
    }

    private void OnDestroy()
    {
        localEnemies.ForEach(e => e.GetComponent<MiniMapIcon>().enabled = false);
    }

    private void Update()
    {
        localEnemies.ForEach(e => e.GetComponent<MiniMapIcon>().enabled = true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            localEnemies.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            localEnemies.Remove(other.gameObject);
            other.GetComponent<MiniMapIcon>().enabled = false;
        }
    }
}
