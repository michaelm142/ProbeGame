using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls the HUD elements when an drone is being controlled
public class DroneUIController : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject DamageDirectionIndicator;
    public GameObject DroneControlUIElement;
    public GameObject HackingOverlay;

    private Image damageDirectionIndicatorImage;
    private Image healthBarImage;
    public Image energyBar;

    public Text MetalCount;
    public Text MetalCountChanged;
    public Text EnergyCountChanged;

    private float damageIndicatorAlpha;

    public AudioClip SwitchToDrone;
    public AudioClip SwitchToCombatDrone;
    public AudioClip switchToCamera;

    public GameObject connectionLostEffect;
    public GameObject GameOverScreen;

    public Animator InventoryChangedAnimator;

    void Awake()
    {
        CameraController.instance.OnCameraChanged.AddListener(new UnityEngine.Events.UnityAction(OnCameraChanged));
    }

    private void Start()
    {
        damageDirectionIndicatorImage = DamageDirectionIndicator.GetComponent<Image>();
        damageIndicatorAlpha = damageDirectionIndicatorImage.color.a;
        healthBarImage = healthBar.GetComponent<Image>();
    }

    public void ChangeMetal(float ammount)
    {
        DroneController.Instance.ActiveDrone.GetComponent<DroneInventory>().Metal += ammount;
        InventoryChangedAnimator.SetTrigger("MetalChanged");
        MetalCountChanged.text = "+" + ammount.ToString();
    }

    public void ChangeEnergy(float ammount)
    {
        DroneController.Instance.ActiveDrone.GetComponent<DroneInventory>().Energy += ammount;
        InventoryChangedAnimator.SetTrigger("EnergyChanged");
        EnergyCountChanged.text = "+" + ammount.ToString() + "%";
    }

    void DroneDestroyed(Drone drone)
    {
        if (drone == DroneController.Instance.ActiveDrone)
            connectionLostEffect.SetActive(true);

        int dronesAlive = DroneController.Instance.DroneCount;
        foreach (var d in DroneController.Instance.Drones)
        {
            if (d == null)
                dronesAlive--;
        }

        if (dronesAlive == 1)
            GameOverScreen.SetActive(true);
        else
        {
            var destoryedIndicator = Instantiate(Resources.Load<GameObject>("UI/PlayerDestroyedIndicator"));
            destoryedIndicator.transform.position = drone.transform.position;
        }
    }

    void OnCameraChanged()
    {
        if (connectionLostEffect.activeSelf)
            connectionLostEffect.SetActive(false);

        var currentCamera = CameraController.instance.ActiveCamera;
        HackingOverlay.SetActive(false);
        if (currentCamera == null)
            return;

        if (currentCamera.transform.parent == null || currentCamera.transform.parent.tag != "Player")
            healthBar.SetActive(false);
        else
            healthBar.SetActive(true);

        if (currentCamera.transform.parent.tag == "SecurityCamera")
            HUDAudioController.PlaySound(switchToCamera);
        else if (DroneController.Instance.ActiveDrone != null)
        {
            if (DroneController.Instance.ActiveDrone.GetComponentInChildren<DroneGunAim>() == null)
                HUDAudioController.PlaySound(SwitchToDrone);
            else
                HUDAudioController.PlaySound(SwitchToCombatDrone);
        }

        if (CameraController.instance.ActiveCamera.transform.parent != null && CameraController.instance.ActiveCamera.transform.parent.tag == "Player")
            transform.Find("HackingOverlay").gameObject.SetActive(DroneController.Instance.ActiveDrone.hacking);
    }

    public void LoadLatestSave()
    {
        var playerInventory = FindObjectOfType<PlayerInventory>();
        LoadSave.LoadProgress(playerInventory.activeSaveFile.FullName);
    }

    // Update is called once per frame
    void Update()
    {
        var drone = DroneController.Instance.ActiveDrone;
        if (drone == null)
            return;

        int totalMetal = 0;
        float totalEnergy = 0.0f;
        foreach (var d in FindObjectsOfType<DroneInventory>())
        {
            totalMetal += (int)d.Metal;
            totalEnergy += d.Energy;
        }
        energyBar.fillAmount = Mathf.Clamp(totalEnergy, 0.0f, 100.0f) / 100.0f;
        MetalCount.text = (totalMetal < 10000 ? "0" : "") + (totalMetal < 1000 ? "0" : "") + (totalMetal < 100 ? "0" : "") + (totalMetal < 10 ? "0" : "") + totalMetal.ToString();


        healthBarImage.fillAmount = drone.Health / drone.MaxHealth;
        if (damageIndicatorAlpha > 0.0f)
            damageIndicatorAlpha -= Time.deltaTime;
        Color c = healthBarImage.color;
        c.a = damageIndicatorAlpha;
        damageDirectionIndicatorImage.color = c;

    }

    void OnDroneDamaged(float angle)
    {
        var damageOverlay = transform.Find("DamageOverlay");
        damageOverlay.GetComponent<Animator>().SetTrigger("Trigger");
        damageOverlay.transform.Find("RawImage").GetComponent<UnityEngine.Video.VideoPlayer>().time = 0.0f;

        DamageDirectionIndicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        damageIndicatorAlpha = 1.0f;
    }
}
