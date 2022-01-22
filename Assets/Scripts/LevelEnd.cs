using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private List<GameObject> localDrones = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Interact()
    {
        if (DroneController.Instance.DroneCount == localDrones.Count)
            FindObjectOfType<MenuControls>().LoadLevel("StoreScreen");
        else
            MessageBox.Show("All drones must be aboard before you can leave");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            localDrones.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            localDrones.Remove(other.gameObject);
    }
}
