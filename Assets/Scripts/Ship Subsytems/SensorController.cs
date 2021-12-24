using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Disconnect()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            enemy.GetComponent<MiniMapIcon>().enabled = false;

    }

    void Interact()
    {
        DroneController.Instance.ConnectDrone(gameObject);
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            enemy.GetComponent<MiniMapIcon>().enabled = true;
    }
}
