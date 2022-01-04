using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HackingMinigame))]
public class HackingMinigameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HackingMinigame minigame = target as HackingMinigame;
        if (GUILayout.Button("Reset"))
            minigame.RestartGame();
    }
}