using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHackCondition : TutorialCondition
{
    public override bool Complete
    {
        get { return FindObjectOfType<CameraController>().enabled = true; }
    }

    public string startText = "";
    public override string StartText
    {
        get { return startText; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
