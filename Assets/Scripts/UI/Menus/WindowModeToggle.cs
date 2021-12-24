using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowModeToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Toggle>().isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;   
    }

    public void ValueChanged()
    {
        Screen.fullScreen = GetComponent<Toggle>().isOn;
    }
}
