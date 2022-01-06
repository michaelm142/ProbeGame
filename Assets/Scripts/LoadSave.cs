using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSave : MonoBehaviour
{
    public PlayerInventory LoadProgress(string filename)
    {
        FileInfo file = new FileInfo(filename);
        if (file == null || !file.Exists)
            throw new InvalidOperationException("Requested file could not be found");

        GameObject inventoryObj = new GameObject("Player Inventory", typeof(PlayerInventory));
        var playerInventory = inventoryObj.GetComponent<PlayerInventory>();


        XmlDocument doc = new XmlDocument();
        doc.Load(file.Open(FileMode.Open));
        var root = doc.SelectSingleNode("Player");
        var levelNode = root.SelectSingleNode("Level");
        var levelAttr = levelNode.Attributes["Name"];
        string sceneName = levelAttr.Value;

        var energyNode = root.SelectSingleNode("Energy");
        playerInventory.Energy = float.Parse(energyNode.InnerText);
        var metalNode = root.SelectSingleNode("Metal");
        playerInventory.Metal = float.Parse(metalNode.InnerText);

        var probeNodes = root.SelectNodes("Probe");
        foreach (XmlElement probeNode in probeNodes)
        {
            PlayerInventory.Drone probe = new PlayerInventory.Drone();

            foreach (DroneUpgradeType upgradeType in Enum.GetValues(typeof(DroneUpgradeType)))
            {
                var upgradeNode = probeNode.SelectSingleNode(upgradeType.ToString()) as XmlElement;
                if (upgradeNode != null)
                {
                    var upgradeLevelAttr = upgradeNode.Attributes["Level"];
                    int level = 0;
                    if (upgradeLevelAttr != null)
                        level = int.Parse(upgradeLevelAttr.Value);

                    probe.upgrades.Add(new DroneUpgrade(upgradeType, level));
                }
            }

            playerInventory.probes.Add(probe);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        return playerInventory;
    }

    public PlayerInventory LoadProgress(int slotIndex)
    {
        DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
        FileInfo file = dir.GetFiles().ToList().Find(f => f.Extension == string.Format(".slot{0}", slotIndex));

        return LoadProgress(file.FullName);
    }

    public void SaveProgress()
    {
        var inventory = FindObjectOfType<PlayerInventory>();

        var file = inventory.activeSaveFile;
        if (!file.Exists)
            throw new InvalidOperationException("Requested file could not be found");

        XmlDocument doc = new XmlDocument();
        var root = doc.AppendChild(doc.CreateElement("Player"));
        var levelNode = root.AppendChild(doc.CreateElement("Level"));
        var levelNameAttr = levelNode.Attributes.Append(doc.CreateAttribute("Name"));
        levelNameAttr.Value = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        var energyNode = root.AppendChild(doc.CreateElement("Energy"));
        energyNode.InnerText = inventory.Energy.ToString();
        var metalNode = root.AppendChild(doc.CreateElement("Metal"));
        metalNode.InnerText = inventory.Metal.ToString();

        foreach (var probe in inventory.probes)
        {
            var probeNode = root.AppendChild(doc.CreateElement("Probe"));
            foreach (var upgrade in probe.upgrades)
            {
                var upgradeElement = doc.CreateElement(upgrade.ToString());
                var levelAttr = doc.CreateAttribute("Level");
                levelAttr.Value = upgrade.Level.ToString();
                
                upgradeElement.Attributes.Append(levelAttr);
                probeNode.AppendChild(upgradeElement);
            }
        }
        
        doc.Save(file.OpenWrite());

        MessageBox.Show(string.Format("Game {0} has been saved", Path.GetFileNameWithoutExtension(file.Name)));
    }
}
