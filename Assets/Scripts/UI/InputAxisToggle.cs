using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAxisToggle : MonoBehaviour
{
    public string AxisName;

    public GameObject Target;

    private float axisValuePrev;

    // Start is called before the first frame update
    void Start()
    {
        axisValuePrev = Input.GetAxis(AxisName);
    }

    // Update is called once per frame
    void Update()
    {
        float axisVal = Input.GetAxis(AxisName);

        if (axisValuePrev >= 1.0f && axisVal < 1.0f)
            Target.SetActive(!Target.activeSelf);

        axisValuePrev = Input.GetAxis(AxisName);
    }
}
