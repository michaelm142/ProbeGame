using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DoorControler : MonoBehaviour
{
    private List<Door> doors = new List<Door>();

    // Start is called before the first frame update
    void Awake()
    {
        doors = FindObjectsOfType<Door>().ToList();
        foreach (var door in doors)
            door.GetComponent<MiniMapIcon>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDoorLocked(Door door)
    {
        if (door.State == Door.DoorState.Disabled || !enabled) return;

        if (door.State == Door.DoorState.Locked)
            door.State = Door.DoorState.Closed;
        else
            door.State = Door.DoorState.Locked;
    }

    public void Interact()
    {
        enabled = true;
        foreach (Door d in doors)
            d.GetComponent<MiniMapIcon>().enabled = true;
        DroneController.Instance.ConnectDrone(gameObject);
    }

    void Disconnect()
    {
        foreach (Door d in doors)
            d.GetComponent<MiniMapIcon>().enabled = false;
        enabled = false;
    }
}
