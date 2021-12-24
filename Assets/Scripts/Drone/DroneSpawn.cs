using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawn : MonoBehaviour
{
    public GameObject dronePrefab;

    // Start is called before the first frame update
    void Start()
    {
        var inventory = FindObjectOfType<PlayerInventory>();
        var spawnPoints = new List<Transform>();
        foreach (Transform t in transform)
        {
            if (t.name == "DroneSpawn")
                spawnPoints.Add(t);
        }
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (i < inventory.probes.Count)
            {
                var probe = inventory.probes[i];
                var drone = Instantiate(dronePrefab);

                // position and rotate drone
                drone.transform.position = spawnPoints[i].position;
                drone.transform.rotation = spawnPoints[i].rotation;

                // enable probe upgrades
                if (probe.upgrades.Contains(PlayerInventory.Upgrades.Sensor))
                {
                    GameObject sensors = new GameObject("Sensors");
                    sensors.AddComponent<DroneSensors>();

                    sensors.transform.SetParent(drone.transform);
                    sensors.transform.localPosition = Vector3.zero;
                }
                if (probe.upgrades.Contains(PlayerInventory.Upgrades.Hull))
                    drone.GetComponent<Drone>().DamageMultiplyer = 0.25f;
                if (probe.upgrades.Contains(PlayerInventory.Upgrades.Speed))
                    drone.GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 2.0f;
            }
        }
    }
}
