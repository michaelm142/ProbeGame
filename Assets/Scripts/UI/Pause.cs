using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool Paused { get; private set; }

    private void OnEnable()
    {
        Paused = true;
    }

    private void OnDisable()
    {
        Paused = false;
    }
}
