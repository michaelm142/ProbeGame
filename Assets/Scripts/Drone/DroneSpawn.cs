using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawn : MonoBehaviour
{
    public GameObject hackerDronePrefab;
    public GameObject combatDronePrefab;
    public GameObject scoutDronePrefab;

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
                GameObject drone = null;
                switch (probe.type)
                {
                    case DroneType.Hacker:
                        drone = Instantiate(hackerDronePrefab);
                        break;
                    case DroneType.Combat:
                        drone = Instantiate(combatDronePrefab);
                        break;
                    case DroneType.Scout:
                        drone = Instantiate(scoutDronePrefab);
                        break;
                }

                // position and rotate drone
                drone.transform.position = spawnPoints[i].position;
                drone.transform.rotation = spawnPoints[i].rotation;
                if (probe.type == DroneType.Scout)
                    drone.transform.position += Vector3.up;

                var opticsUpgrade = probe.upgrades.Find(u => u.type == DroneUpgradeType.Optics);
                // enable probe upgrades
                if (opticsUpgrade != null)
                {
                    var sensors = drone.GetComponent<DroneSensors>();
                    switch (opticsUpgrade.Level)
                    {

                        case 1:
                            sensors.Radius = 10.0f;
                            break;
                        case 2:
                            sensors.Radius = 15.0f;
                            break;
                        case 3:
                            sensors.Radius = 30.0f;
                            break;
                    }
                }
                var hullUpgrade = probe.upgrades.Find(u => u.type == DroneUpgradeType.Hull);
                if (hullUpgrade != null)
                {
                    switch (hullUpgrade.Level)
                    {
                        case 1:
                            drone.GetComponent<Drone>().MaxHealth = 16.0f;
                            break;
                        case 2:
                            drone.GetComponent<Drone>().MaxHealth = 32.0f;
                            break;
                        case 3:
                            drone.GetComponent<Drone>().MaxHealth = 48.0f;
                            break;
                    }

                    drone.GetComponent<Drone>().Health = drone.GetComponent<Drone>().MaxHealth;
                }
                var speedUpgrade = probe.upgrades.Find(u => u.type == DroneUpgradeType.Speed);
                if (speedUpgrade != null)
                {
                    switch (speedUpgrade.Level)
                    {
                        case 1:
                            drone.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 3.5f;
                            break;
                        case 2:
                            drone.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 5.0f;
                            break;
                        case 3:
                            drone.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 7.0f;
                            break;
                    }
                }
            }
        }
    }
}
