using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityOptions : MonoBehaviour
{
    public Text qualityName;

    // Start is called before the first frame update
    void Start()
    {
        int level = QualitySettings.GetQualityLevel();
        qualityName.text = QualitySettings.names[level];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseQuality()
    {
        int level = QualitySettings.GetQualityLevel();
        level++;
        if (level >= QualitySettings.names.Length - 1)
            level = 0;

        QualitySettings.SetQualityLevel(level);
        qualityName.text = QualitySettings.names[level];
    }

    public void DecreaseQuality()
    {
        int level = QualitySettings.GetQualityLevel();
        level--;
        if (level < 0)
            level = QualitySettings.names.Length - 1;

        QualitySettings.SetQualityLevel(level);
        qualityName.text = QualitySettings.names[level];
    }
}
