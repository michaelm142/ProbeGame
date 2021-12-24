using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SonarPingControl : MonoBehaviour
{
    public float CoolDownLength = 30.0f;
    private float coolDownTimer;

    public bool useSonarPing;

    MiniMapClickNavigation mmcn;

    public Image Image;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        mmcn = FindObjectOfType<MiniMapClickNavigation>();
        mmcn.OnClick += MinimapClick;

        button = GetComponent<Button>();

        coolDownTimer = CoolDownLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDownTimer < CoolDownLength)
        {
            coolDownTimer += Time.deltaTime;
            button.interactable = false;
        }
        else
            button.interactable = true;

        Image.fillAmount = coolDownTimer / CoolDownLength;
        Image.SetAllDirty();
    }

    public void ButtonClick()
    {
        useSonarPing = mmcn.IgnoreNextClick = true;
    }

    void MinimapClick(object sender, System.EventArgs e)
    {
        if (!useSonarPing || coolDownTimer < CoolDownLength) return;

        GameObject prefab = Resources.Load<GameObject>("UI/SonarPing");
        var ping = Instantiate(prefab);
        ping.transform.position = (Vector3)sender;
        coolDownTimer = 0.0f;
        useSonarPing = false;
    }
}
