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

    private float damageIndicatorAlpha;

    public AudioClip SwitchToDrone;
    public AudioClip SwitchToCombatDrone;

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

    void OnCameraChanged()
    {
        var currentCamera = CameraController.instance.ActiveCamera;
        HackingOverlay.SetActive(false);
        if (currentCamera == null)
            return;

        if (currentCamera.transform.parent == null || currentCamera.transform.parent.tag != "Player")
            healthBar.SetActive(false);
        else
            healthBar.SetActive(true);

        if (DroneController.Instance.ActiveDrone != null)
        {
            if (DroneController.Instance.ActiveDrone.GetComponentInChildren<DroneGunAim>() == null)
                HUDAudioController.PlaySound(SwitchToDrone);
            else
                HUDAudioController.PlaySound(SwitchToCombatDrone);
        }
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
