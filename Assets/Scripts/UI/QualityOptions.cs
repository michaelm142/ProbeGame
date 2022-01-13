using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityOptions : MonoBehaviour
{
    public Text qualityName;
    public Text antiAliasingName;
    public Text anisoName;
    public Text shadowResolutionName;

    public Button AdvancedGraphicsSettingsButton;

    private string[] AntiAliasngNames = new string[]
    {
        "Disabled",
        "2x Multi Sampling",
        "4x Multi Sampling",
        "8x Multi Sampling",
    };

    // Start is called before the first frame update
    void Start()
    {
        int level = QualitySettings.GetQualityLevel();
        qualityName.text = QualitySettings.names[level];
    }

    // Update is called once per frame
    void Update()
    {
        int level = QualitySettings.GetQualityLevel();
        AdvancedGraphicsSettingsButton.interactable = level == 6;
    }

    public void IncreaseAnisoLevel()
    {
        int anisoLevel = (int)QualitySettings.anisotropicFiltering;
        anisoLevel++;
        if (anisoLevel > Enum.GetNames(typeof(AnisotropicFiltering)).Length - 1)
            anisoLevel = 0;

        QualitySettings.anisotropicFiltering = (AnisotropicFiltering)anisoLevel;
        anisoName.text = QualitySettings.anisotropicFiltering.ToString();
    }

    public void DecreaseAnisoLevel()
    {
        int anisoLevel = (int)QualitySettings.anisotropicFiltering;
        anisoLevel--;
        if (anisoLevel < 0)
            anisoLevel = Enum.GetNames(typeof(AnisotropicFiltering)).Length - 1;

        QualitySettings.anisotropicFiltering = (AnisotropicFiltering)anisoLevel;
        anisoName.text = QualitySettings.anisotropicFiltering.ToString();
    }

    public void IncreaseAntiAliasing()
    {
        int aaLevel = QualitySettings.antiAliasing;
        aaLevel++;
        if (aaLevel > 3)
            aaLevel = 0;

        QualitySettings.antiAliasing = aaLevel;
        antiAliasingName.text = AntiAliasngNames[aaLevel];
    }

    public void DecreaseAntiAliasing()
    {
        int aaLevel = QualitySettings.antiAliasing;
        aaLevel--;
        if (aaLevel < 0)
            aaLevel = 3;

        QualitySettings.antiAliasing = aaLevel;
        antiAliasingName.text = AntiAliasngNames[aaLevel];
    }

    public void  IncreaseShadowResolution()
    {
        int shadowLevel = (int)QualitySettings.shadowResolution;
        shadowLevel++;
        if (shadowLevel > Enum.GetNames(typeof(ShadowResolution)).Length - 1)
            shadowLevel = 0;

        QualitySettings.shadowResolution = (ShadowResolution)shadowLevel;
        shadowResolutionName.text = QualitySettings.shadowResolution.ToString();
    }

    public void DecreaseShadowResolution()
    {
        int shadowLevel = (int)QualitySettings.shadowResolution;
        shadowLevel--;
        if (shadowLevel < 0)
            shadowLevel = Enum.GetNames(typeof(ShadowResolution)).Length - 1;

        QualitySettings.shadowResolution = (ShadowResolution)shadowLevel;
        shadowResolutionName.text = QualitySettings.shadowResolution.ToString();
    }

    public void ToggleRealtimeReflections(bool value)
    {
        QualitySettings.realtimeReflectionProbes = value;
    }

    public void IncreaseQuality()
    {
        int level = QualitySettings.GetQualityLevel();
        level++;
        if (level > QualitySettings.names.Length - 1)
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
