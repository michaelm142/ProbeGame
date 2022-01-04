using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControler : MonoBehaviour
{
    private List<Door> doors = new List<Door>();

    // Start is called before the first frame update
    void Start()
    {
        
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
        DroneController.Instance.ConnectDrone(gameObject);
    }

    void Disconnect()
    {
        enabled = false;
    }

    public void AddDoor(Door door)
    {
        doors.Add(door);
    }

    public void RemoveDoor(Door door)
    {
        doors.Remove(door);
    }
}
