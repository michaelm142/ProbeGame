using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevelMenu : MonoBehaviour
{
    public AudioClip pointSound;

    private AudioSource source;

    public Text Energy;
    public Text Metal;
    public Text TotalMetal;

    public bool tallyingEnergy;
    public bool tallyingMetal;
    public bool TotalingMetal;
    public bool FactoringEnergy;
    public float Duration = 5.0f;

    private string totalEnergyText;
    private string totalMetalText;
    private string MetalText;

    // these values track the currently displayed ammount
    private float totalMetalCount;
    private float energyCount;
    private float metalCount;

    private float energyRate;
    private float metalRate;
    private float totalMetalRate;

    // these values track the target value acending towards
    private float totalMetalValue;
    private float metalValue;
    private float energyValue;

    // Start is called before the first frame update
    void Start()
    {
        totalEnergyText = Energy.text;
        MetalText = Metal.text;
        totalMetalText = TotalMetal.text;

        energyValue = FindObjectOfType<PlayerInventory>().Energy;
        metalValue = FindObjectOfType<PlayerInventory>().Metal;

        energyRate = energyValue / Duration;
        metalRate = metalValue / Duration;
        totalMetalValue = (metalValue + energyValue * 20);
        totalMetalRate = totalMetalValue / Duration;

        source = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tallyingEnergy && !FactoringEnergy && energyCount < energyValue)
        { 
            energyCount += Time.deltaTime * energyRate;
            HUDAudioController.PlaySound(pointSound);
        }
        if (tallyingMetal && !TotalingMetal && !FactoringEnergy && metalCount < metalValue)
        {
            metalCount += Time.deltaTime * metalRate;
            HUDAudioController.PlaySound(pointSound);
        }
        if (FactoringEnergy)
        {
            if (metalCount > 0.0f)
            {
                metalCount -= Time.deltaTime * metalRate;
                HUDAudioController.PlaySound(pointSound);
            }
            if (energyCount > 0.0f)
            {
                energyCount -= Time.deltaTime * energyRate;
                HUDAudioController.PlaySound(pointSound);
            }
            if (totalMetalCount < totalMetalValue)
            {
                totalMetalCount += Time.deltaTime * totalMetalRate;
                HUDAudioController.PlaySound(pointSound);
            }
            else
                FindObjectOfType<PlayerInventory>().Metal = totalMetalCount;
        }
        if (TotalingMetal && totalMetalCount < metalCount)
        {
            metalCount -= Time.deltaTime * metalRate;
            totalMetalCount += Time.deltaTime * metalRate;
            if (totalMetalCount >= metalCount)
            {
                TotalingMetal = false;
                FactoringEnergy = true;
            }
        }

        metalCount = Mathf.Clamp(metalCount, 0, float.PositiveInfinity);

        Energy.text = string.Format(totalEnergyText, Mathf.Round(energyCount));
        Metal.text = string.Format(MetalText, Mathf.Round(metalCount));
        TotalMetal.text = string.Format(totalMetalText, Mathf.Round(totalMetalCount));
    }

}
