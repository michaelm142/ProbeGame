using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private List<GameObject> localDrones = new List<GameObject>();

    void Interact()
    {
        if (DroneController.Instance.DroneCount == localDrones.Count)
        {
            float energyAmmount = 0.0f;
            foreach (var inventory in FindObjectsOfType<DroneInventory>())
                energyAmmount += inventory.Energy;
            if (energyAmmount < 100.0f)
            {
                MessageBox.Show("We still don't have enough energy to power the ship!");
            }
            else
            {
                FindObjectOfType<MenuControls>().LoadLevel("EndOfLevelScreen");
                var playerInventory = FindObjectOfType<PlayerInventory>();
                foreach (var inventory in FindObjectsOfType<DroneInventory>())
                {
                    playerInventory.Metal += inventory.Metal;
                    playerInventory.Energy += inventory.Energy;

                    Destroy(inventory.gameObject);
                }
            }
        }
        else
            MessageBox.Show("All drones must be aboard before you can leave");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !localDrones.Contains(other.gameObject))
            localDrones.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            localDrones.Remove(other.gameObject);
    }
}
