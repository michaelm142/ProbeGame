using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreScreen : MonoBehaviour
{
    public GameObject MainPage;
    public GameObject BuyProbeScreen;

    private DronePurchaseing activeSlot;

    // Start is called before the first frame update
    void Start()
    {
        BuyProbeScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginBuyDrone(DronePurchaseing slot)
    {
        activeSlot = slot;
        MainPage.SetActive(false);
        BuyProbeScreen.SetActive(true);
    }

    public void BuyDrone(int type)
    {
        BuyDrone((DroneType)type);
    }

    public void BuyDrone(DroneType type)
    {
        activeSlot.drone = FindObjectOfType<PlayerInventory>().BuyDrone(type);
        BuyProbeScreen.SetActive(false);
        MainPage.SetActive(true);
    }

    public void CancelBuyDrone()
    {
        activeSlot = null;
        BuyProbeScreen.SetActive(false);
        MainPage.SetActive(true);
    }
}
