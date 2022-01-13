using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<Drone> probes = new List<Drone>();

    public int NumStartProbes;
    public float Energy;
    public float Metal;

    private List<Button> buyProbesButtons;

    public System.IO.FileInfo activeSaveFile;

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(this);

        buyProbesButtons = FindObjectsOfType<Button>().ToList().FindAll(b => b.name == "BuyProbeButton");

        for (int i = 0; i < NumStartProbes; i++)
        {
            probes.Add(new Drone());
            buyProbesButtons[i].interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {


    }

    public Drone BuyDrone(DroneType type)
    {
        var p = new Drone(type);
        probes.Add(p);

        return p;
    }

    public class Drone
    {
        public List<DroneUpgrade> upgrades;

        public DroneType type;

        public Drone(DroneType type = DroneType.Hacker)
        {
            upgrades = new List<DroneUpgrade>();
            this.type = type;
        }
    }
}

public enum DroneType
{
    Hacker,
    Combat,
    Scout,
}

public class DroneUpgrade
{
    public DroneUpgradeType type;
    public int Level;

    public DroneUpgrade(DroneUpgradeType type, int Level = 1)
    {
        this.type = type;
        this.Level = Level;
    }

    public static Dictionary<DroneUpgradeType, string[]> UpgradeDescriptions = new Dictionary<DroneUpgradeType, string[]>()
    {
        { DroneUpgradeType.Hull,     new string[]
        {
            "Titanium alloy frame fortifies structural integrity.\n\nIncreaces max health to 16",
            "Increaced armor plating around the drone hull to protect it from hostile environments.\n\nIncreases max health to 32",
            "Anti-balistic foam insets absorb kenitc energy from incomming attacks.\n\nIncreases max health to 48",
        }  },
        { DroneUpgradeType.Optics,   new string[]
        {
            "Advanced Opical sensors increase detection and scanning capabilities",
            "Advanced Opical sensors increase detection and scanning capabilities",
            "Advanced Opical sensors increase detection and scanning capabilities",
        } },
        { DroneUpgradeType.Sensor,   new string[]
        {
            "Augmented scanner array. Scans in both radar and infrared spectral bands for more accurate target detection.\n\nIncreases detection radius to 10 meters",
            "Advanced on-board tracking computer allows for faster target anylsys and aquisition.\n\nIncreases detection radius to 15 meters",
            "Additional capacitors to increase signal amplitude and range.\n\nIncreaces detection radius to 30 meters",
        } },
        { DroneUpgradeType.Speed,    new string[]
        {
            "Advanced motors and wheel kit for increaced speed.\n\nIncreaces drone speed to 3.5 meters per second",
            "Augmented power system allows for higher output torque.\n\nIncreaces drone speed to 5 meters per second",
            "Pnumatic-driven turbo increases motor torque.\n\nIncreases drone speed to 7 meters per second"
        } },
    };
}
public enum DroneUpgradeType
{
    Optics,
    Hull,
    Sensor,
    Speed,
}
