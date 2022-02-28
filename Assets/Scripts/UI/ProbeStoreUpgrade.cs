using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProbeStoreUpgrade : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public DroneUpgradeType Type;

    public AudioSource buttonClick;

    public Image Checkmark;

    public Text LevelLabel;

    public float Cost;

    private DronePurchaseing purchaseing;
    private DroneUpgrade upgrade;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (purchaseing.UpgradeProbe(Type))
        {
            Checkmark.gameObject.SetActive(true);
            OnPointerEnter(eventData);
        }

        buttonClick.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        int index = 0;
        if (upgrade != null)
            index = upgrade.Level;

        if (index >= DroneUpgrade.UpgradeDescriptions[Type].Length)
            purchaseing.UpgradeInformationLabel.text = string.Empty;
        else
            purchaseing.UpgradeInformationLabel.text = DroneUpgrade.UpgradeDescriptions[Type][index];
    }

    // Start is called before the first frame update
    void Start()
    {
        purchaseing = transform.parent.GetComponent<DronePurchaseing>();
        if (purchaseing.drone != null)
            upgrade = purchaseing.drone.upgrades.Find(u => u.type == Type);
    }

    // Update is called once per frame
    void Update()
    {
        if (purchaseing.drone != null && upgrade == null)
            upgrade = purchaseing.drone.upgrades.Find(u => u.type == Type);
        if (upgrade != null)
            LevelLabel.text = upgrade.Level.ToString();
    }
}
