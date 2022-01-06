using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DronePurchaseing : MonoBehaviour
{
    private const int MaxLevels = 3;

    public Text UpgradeInformationLabel;

    public PlayerInventory.Drone probe { get; private set; }

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
        if (probe != null)
            return;

        probe = FindObjectOfType<PlayerInventory>().BuyProbe();
    }

    public void UpgradeProbe(int type)
    {
        DroneUpgradeType t = (DroneUpgradeType)type;
        var upgrade = probe.upgrades.Find(u => u.type == t);
        if (upgrade == null)
            probe.upgrades.Add(new DroneUpgrade(t));
        else if (upgrade.Level < MaxLevels)
            upgrade.Level++;
    }
}
