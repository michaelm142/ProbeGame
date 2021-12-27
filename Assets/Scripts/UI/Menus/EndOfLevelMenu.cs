using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevelMenu : MonoBehaviour
{
    public Text totalEnergy;
    public Text totalFuel;
    public Text totalMetal;

    public float Duration = 5.0f;

    private string totalEnergyText;
    private string totalFuelText;
    private string totalMetalText;

    private float energyCount;
    private float fuelCount;
    private float metalCount;

    private float energyRate;
    private float fuelRate;
    private float metalRate;

    // Start is called before the first frame update
    void Start()
    {
        totalEnergyText = totalEnergy.text;
        totalFuelText = totalFuel.text;
        totalMetalText = totalMetal.text;

        var inventory = FindObjectOfType<PlayerInventory>();
        energyRate = inventory.Energy / Duration;
        fuelRate = inventory.Fuel / Duration;
        metalRate = inventory.Metal / Duration; 
    }

    // Update is called once per frame
    void Update()
    {
        var inventory = FindObjectOfType<PlayerInventory>();
        if (energyCount < inventory.Energy)
            energyCount += Time.deltaTime * energyRate;
        if (fuelCount < inventory.Fuel)
            fuelCount += Time.deltaTime * fuelRate;
        if (metalCount < inventory.Metal)
            metalCount += Time.deltaTime * metalRate;

        totalEnergy.text = string.Format(totalEnergyText, Mathf.Round(energyCount));
        totalFuel.text = string.Format(totalFuelText, Mathf.Round(fuelCount));
        totalMetal.text = string.Format(totalMetalText, Mathf.Round(metalCount));

    }

}
