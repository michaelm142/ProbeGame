using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    public float value;
    public enum ResourceType
    {
        Energy,
        Metal,
    }

    public ResourceType type;

    public AudioClip pickupSound;

    private void Start()
    {
        GetComponent<Interactable>().icon.transform.Find("NameLabel").GetComponent<Text>().text = gameObject.name;
        GetComponent<Interactable>().icon.transform.Find("AmmountLabel").GetComponent<Text>().text = Mathf.RoundToInt(value).ToString();
    }

    public void Interact()
    {
        switch (type)
        {
            case ResourceType.Energy:
                FindObjectOfType<DroneUIController>().ChangeMetal(value);
                break;
            case ResourceType.Metal:
                FindObjectOfType<DroneUIController>().ChangeEnergy(value);
                break;
        }

        HUDAudioController.PlaySound(pickupSound);
        Destroy(gameObject);
    }
}
