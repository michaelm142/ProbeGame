using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the HUD elements when an drone is being controlled
public class DroneUIController : MonoBehaviour
{
    public GameObject healthBar;

    public GameObject DroneControlUIElement;

    void Awake()
    {
        CameraController.instance.OnCameraChanged.AddListener(new UnityEngine.Events.UnityAction(OnCameraChanged));
    }

    void OnCameraChanged()
    {
        var currentCamera = CameraController.instance.ActiveCamera;
        if (currentCamera == null)
            return;

        if (currentCamera.transform.parent == null || currentCamera.transform.parent.tag != "Player")
            healthBar.SetActive(false);
        else
            healthBar.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        var drone = DroneController.Instance.ActiveDrone;
        if (drone == null)
            return;
        
        var bar = healthBar.GetComponent<UnityEngine.UI.Image>();
        bar.fillAmount = drone.Health / drone.MaxHealth;
    }
}
