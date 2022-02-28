using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreScreen : MonoBehaviour
{
    public Text MetalCount;

    public Animator MainPage;

    private DronePurchaseing activeSlot;

    // Start is called before the first frame update
    void Start()
    {
        var playerInventory = FindObjectOfType<PlayerInventory>();
        if (playerInventory != null)
        {
            var purchacing = FindObjectsOfType<DronePurchaseing>().ToList();
            for (int i = 0; i < purchacing.Count; i++)
            {
                if (i >= playerInventory.drones.Count)
                    continue;
                var drone = playerInventory.drones[i];
                DronePurchaseing p = purchacing[i];
                var button = p.transform.parent.GetComponentInChildren<Button>();
                if (drone != null)
                {
                    p.drone = new PlayerInventory.Drone(drone.type);
                    button.interactable = false;
                    foreach (var upgrade in drone.upgrades)
                        p.LoadUpgrade(upgrade.type, upgrade.Level);
                }
                else
                    button.interactable = true;
            }
        }
    }

    public void ContinueButtonPressed()
    {
        var playerInventory = FindObjectOfType<PlayerInventory>();
        playerInventory.drones.Clear();
        foreach (DronePurchaseing p in FindObjectsOfType<DronePurchaseing>())
        {
            if (p.drone == null)
                continue;

            playerInventory.drones.Add(p.drone);
        }

        AsyncSceneLoader.LoadScene("VonBron");
    }

    // Update is called once per frame
    void Update()
    {
        MetalCount.text = ((int)FindObjectOfType<PlayerInventory>().Metal).ToString();
    }

    public void BeginBuyDrone(DronePurchaseing slot)
    {
        activeSlot = slot;
        MainPage.SetTrigger("BuyProbe");
    }

    public void BuyDrone(int type)
    {
        BuyDrone((DroneType)type);
    }

    public void BuyDrone(DroneType type)
    {
        var drone = FindObjectOfType<PlayerInventory>().BuyDrone(type);
        if (drone != null)
        {
            activeSlot.transform.parent.GetComponentInChildren<Button>().interactable = false;
            activeSlot.drone = drone;
        }
        MainPage.SetTrigger("MainPage");
    }

    public void CancelBuyDrone()
    {
        activeSlot = null;
        MainPage.SetTrigger("MainPage");
    }

}
