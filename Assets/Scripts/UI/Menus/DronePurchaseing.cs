using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DronePurchaseing : MonoBehaviour
{
    private Button purchasingButton;

    private PlayerInventory.Probe probe;

    // Start is called before the first frame update
    void Start()
    {
        purchasingButton = GetComponentInChildren<Button>();
        purchasingButton.onClick.AddListener(new UnityEngine.Events.UnityAction(BuyProbe));
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
        purchasingButton.interactable = false;
    }

    public void UpgradeProbe(int index)
    {
        PlayerInventory.Upgrades upgrade = (PlayerInventory.Upgrades)index;
        if (!probe.upgrades.Contains(upgrade))
            probe.upgrades.Add(upgrade);
    }
}
