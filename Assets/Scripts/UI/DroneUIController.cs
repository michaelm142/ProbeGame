using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls the HUD elements when an drone is being controlled
public class DroneUIController : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject DamageDirectionIndicator;
    public GameObject HackingOverlay;
    public GameObject DroneButtonPanel;
    public GameObject ScoutReticlue;

    private Image damageDirectionIndicatorImage;
    private Image healthBarImage;
    public Image energyBar;
    public Image ScanMeter;

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

    public HackingMinigame hackingMinigame;

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

        var activeDrone = DroneController.Instance.ActiveDrone;
        if (currentCamera.transform.parent.tag == "SecurityCamera")
            HUDAudioController.PlaySound(switchToCamera);
        else if (activeDrone != null)
        {
            if (activeDrone.type == DroneType.Hacker)
            {
                ScoutReticlue.SetActive(false);
                HUDAudioController.PlaySound(SwitchToDrone);
            }
            else if (activeDrone.type == DroneType.Combat)
            {
                ScoutReticlue.SetActive(false);
                HUDAudioController.PlaySound(SwitchToCombatDrone);
            }
            else if (activeDrone.type == DroneType.Scout)
            {
                ScoutReticlue.SetActive(true);
            }    
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

        if (drone.type != DroneType.Scout)
        {
            if (ScanMeter.gameObject.activeSelf)
                ScanMeter.gameObject.SetActive(false);
        }
        else
        {
            if (!ScanMeter.gameObject.activeSelf)
                ScanMeter.gameObject.SetActive(true);

            var scanner = drone.GetComponent<DroneScanner>();
            ScanMeter.fillAmount = scanner.Value / scanner.MaxValue;
        }

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

        var buttons = DroneButtonPanel.GetComponentsInChildren<Button>();
        int activeIndex = DroneController.Instance.Drones.ToList().IndexOf(drone);
        foreach (Button b in buttons)
            b.interactable = true;

        buttons[activeIndex].interactable = false;
        for (int i = DroneController.Instance.DroneCount; i < DroneController.MaxDroneCount; i++)
            buttons[i].interactable = false;
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
