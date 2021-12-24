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
        foreach (var probe in inventory.probes)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(string.Format("Probe:{0}", i));
                if (probe.upgrades.Contains(PlayerInventory.Upgrades.Optics))
                    EditorGUILayout.Toggle(true);
                else
                    EditorGUILayout.Toggle(false);

                if (probe.upgrades.Contains(PlayerInventory.Upgrades.Hull))
                    EditorGUILayout.Toggle(true);
                else
                    EditorGUILayout.Toggle(false);

                if (probe.upgrades.Contains(PlayerInventory.Upgrades.Sensor))
                    EditorGUILayout.Toggle(true);
                else
                    EditorGUILayout.Toggle(false);

                if (probe.upgrades.Contains(PlayerInventory.Upgrades.Speed))
                    EditorGUILayout.Toggle(true);
                else
                    EditorGUILayout.Toggle(false);
            }
            EditorGUILayout.EndHorizontal();
            i++;
        }
    }
}
