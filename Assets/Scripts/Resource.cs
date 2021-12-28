using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float value;
    public enum ResourceType
    {
        Fuel,
        Energy,
        Metal,
    }

    public ResourceType type;


    public void Interact()
    {
        var inventory = FindObjectOfType<PlayerInventory>();
        switch (type)
        {
            case ResourceType.Energy:
                inventory.Energy += value;
                break;
            case ResourceType.Fuel:
                inventory.Fuel += value;
                break;
            case ResourceType.Metal:
                inventory.Metal += value;
                break;
        }

        Destroy(gameObject);
    }
}
