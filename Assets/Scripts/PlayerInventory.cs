using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<Probe> probes = new List<Probe>();

    public int NumStartProbes;
    public float Fuel;
    public float Energy;
    public float Metal;

    private List<Button> buyProbesButtons;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        buyProbesButtons = FindObjectsOfType<Button>().ToList().FindAll(b => b.name == "BuyProbeButton");
        
        for (int i = 0; i < NumStartProbes; i++)
        {
            probes.Add(new Probe());
            buyProbesButtons[i].interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(string.Format("Number of probes:{0}", probes.Count));
        
    }

    public Probe BuyProbe()
    {
        var p = new Probe();
        probes.Add(p);

        return p;
    }

    public class Probe
    {
        public List<Upgrades> upgrades;
        
        public Probe()
        {
            upgrades = new List<Upgrades>();
        }
    }

    public enum Upgrades
    {
        Optics,
        Hull,
        Sensor,
        Speed,
    }
}     
