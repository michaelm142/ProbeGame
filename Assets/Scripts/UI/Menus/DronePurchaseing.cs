using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DronePurchaseing : MonoBehaviour
{
    private const int MaxLevels = 3;

    public Text UpgradeInformationLabel;

    public PlayerInventory.Drone drone { get; set; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyProbe()
    {
        if (drone != null)
            return;

        FindObjectOfType<StoreScreen>().BeginBuyDrone(this);
    }

    public void LoadUpgrade(DroneUpgradeType type, int level)
    {
        var upgrade = drone.upgrades.Find(u => u.type == type);
        if (upgrade == null)
        {
            upgrade = new DroneUpgrade(type);
            drone.upgrades.Add(upgrade);
        }

        var storeUpgrades = GetComponentsInChildren<ProbeStoreUpgrade>().ToList();
        var u = storeUpgrades.Find(u => u.Type == type).Checkmark.gameObject;
        u.SetActive(true);

        upgrade.Level = level;
    }

    public bool UpgradeProbe(DroneUpgradeType type)
    {
        if (FindObjectOfType<PlayerInventory>().Metal < 250.0f)
        {
            MessageBox.Show("Not enough metal");
            return false;
        }
        var upgrade = drone.upgrades.Find(u => u.type == type);
        if (upgrade == null)
            drone.upgrades.Add(new DroneUpgrade(type));
        else if (upgrade.Level < MaxLevels)
            upgrade.Level++;
        else
            return true;
        FindObjectOfType<PlayerInventory>().Metal -= 250.0f;
        return true;
    }
}
