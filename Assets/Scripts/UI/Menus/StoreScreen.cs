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
