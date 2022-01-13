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

    public void UpgradeProbe(int type)
    {
        DroneUpgradeType t = (DroneUpgradeType)type;
        var upgrade = drone.upgrades.Find(u => u.type == t);
        if (upgrade == null)
            drone.upgrades.Add(new DroneUpgrade(t));
        else if (upgrade.Level < MaxLevels)
            upgrade.Level++;
    }
}
