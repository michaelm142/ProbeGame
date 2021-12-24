using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraController.instance.ActiveCamera != null)
            transform.position = CameraController.instance.ActiveCamera.transform.position;
    }
}
