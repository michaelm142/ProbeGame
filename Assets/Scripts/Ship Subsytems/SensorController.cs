using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    void Disconnect()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            enemy.GetComponent<MiniMapIcon>().enabled = false;

    }

    void Interact()
    {
        DroneController.Instance.BeginHacking(gameObject);
    }

    void ActivateSubsystem()
    {
        FindObjectOfType<MinimapFogOfWar>().GetComponent<UnityEngine.UI.RawImage>().enabled = false;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            enemy.GetComponent<MiniMapIcon>().enabled = true;
    }
}
