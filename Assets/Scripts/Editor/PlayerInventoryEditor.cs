using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerInventory))]
public class PlayerInventoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerInventory inventory = target as PlayerInventory;
        int i = 1;
        foreach (var probe in inventory.drones)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(string.Format("Probe:{0}", i));
                var opticsUpgrade = probe.upgrades.Find(u => u.type == DroneUpgradeType.Optics);
                var hullUpgrade = probe.upgrades.Find(u => u.type == DroneUpgradeType.Hull);
                var sensorUpgrade = probe.upgrades.Find(u => u.type == DroneUpgradeType.Sensor);
                var speedUpgrade = probe.upgrades.Find(u => u.type == DroneUpgradeType.Speed);

                if (opticsUpgrade != null)
                    EditorGUILayout.LabelField(string.Format("Optics Upgrade Level:{0}", opticsUpgrade.Level));
                else
                    EditorGUILayout.LabelField("No Optics Upgrade");

                if (hullUpgrade != null)
                    EditorGUILayout.LabelField(string.Format("Hull Upgrade Level:{0}", hullUpgrade.Level));
                else
                    EditorGUILayout.LabelField("No Hull Upgrade");

                if (sensorUpgrade != null)
                    EditorGUILayout.LabelField(string.Format("Sensor Upgrade Level:{0}", sensorUpgrade.Level));
                else
                    EditorGUILayout.LabelField("No Sensor Upgrade");

                if (speedUpgrade != null)
                    EditorGUILayout.LabelField(string.Format("Speed Upgrade Level:{0}", sensorUpgrade.Level));
                else
                    EditorGUILayout.LabelField("No Speed Upgrade");
            }
            EditorGUILayout.EndHorizontal();
            i++;
        }
    }
}
