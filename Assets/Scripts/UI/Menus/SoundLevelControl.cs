using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundLevelControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().value = AudioListener.volume;
    }

    public void ValueChanged()
    {
        AudioListener.volume = GetComponent<Slider>().value;
    }
}
