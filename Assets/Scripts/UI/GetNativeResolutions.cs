using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class GetNativeResolutions : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var dropDown = GetComponent<Dropdown>();
        if (dropDown.options.Count == 0)
        {
            foreach (var res in Screen.resolutions)
            {
                dropDown.options.Add(new Dropdown.OptionData(string.Format("{0} x {1}", res.width, res.height)));
            }
        }
        int currentResIndex = Screen.resolutions.ToList().IndexOf(Screen.currentResolution);
        dropDown.value = currentResIndex;
    }
}
