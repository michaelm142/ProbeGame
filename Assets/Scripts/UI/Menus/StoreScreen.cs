using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreScreen : MonoBehaviour
{
    public Animator MainPage;

    private DronePurchaseing activeSlot;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
        activeSlot.drone = FindObjectOfType<PlayerInventory>().BuyDrone(type);
        MainPage.SetTrigger("MainPage");
    }

    public void CancelBuyDrone()
    {
        activeSlot = null;
        MainPage.SetTrigger("MainPage");
    }
}
