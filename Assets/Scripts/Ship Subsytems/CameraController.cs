using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    public CameraController()
    {
        instance = this;
    }

    private Camera _camera;
    public Camera ActiveCamera
    {
        set
        {
            if (_camera != null)
                _camera.enabled = false;

            _camera = value;
            OnCameraChanged.Invoke();

            if (_camera != null)
                _camera.enabled = true;
        }

        get { return _camera; }
    }

    public UnityEvent OnCameraChanged;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var camera in GameObject.FindGameObjectsWithTag("SecurityCamera"))
        {
            Camera cam = camera.GetComponentInChildren<Camera>();
            camera.GetComponent<MiniMapIcon>().OnClicked.AddListener(delegate () { ActiveCamera = cam; });
            cam.enabled = false;
            camera.GetComponent<MiniMapIcon>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Disconnect()
    {
        foreach (var camera in GameObject.FindGameObjectsWithTag("SecurityCamera"))
            camera.GetComponent<MiniMapIcon>().enabled = false;
    }

    void Interact()
    {
        DroneController.Instance.BeginHacking(gameObject);
    }

    void ActivateSubsystem()
    {
        foreach (var camera in GameObject.FindGameObjectsWithTag("SecurityCamera"))
            camera.GetComponent<MiniMapIcon>().enabled = true;
    }
}
