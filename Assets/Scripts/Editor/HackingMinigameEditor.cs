using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HackingMinigame))]
public class HackingMinigameEditor : Editor
{
    private string SaveFilename;
    private string LoadFilename;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HackingMinigame minigame = target as HackingMinigame;
        if (GUILayout.Button("Reset"))
            minigame.RestartGame();
        if (GUILayout.Button("Generate New"))
            minigame.GenerateTiles(minigame.Difficulty);

        EditorGUILayout.BeginHorizontal();

        SaveFilename = EditorGUILayout.TextField("Filename", SaveFilename);
        if (GUILayout.Button("Save"))
            minigame.SaveConfiguration(SaveFilename + ".lvlconfig");

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        LoadFilename = EditorGUILayout.TextField("Filename", LoadFilename);
        if (GUILayout.Button("Load"))
            minigame.LoadConfiguration(LoadFilename + ".lvlconfig");

        EditorGUILayout.EndHorizontal();
    }
}