using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DevTools))]
public class DevToolsEditor : Editor
{
    private CardType card = CardType.WaterE;
    //private bool forPlayer = true;
    private string player;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DevTools dtc = (DevTools) target;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Card", GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
        card = (CardType)EditorGUILayout.EnumPopup(card, GUILayout.MinWidth(60f));
        //EditorGUILayout.Space();
        //EditorGUILayout.LabelField("Player /Enemy", GUILayout.MaxWidth(100f), GUILayout.MinWidth(70f));
        //forPlayer = EditorGUILayout.Toggle(forPlayer, GUILayout.Width(15f));
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Create card for Player", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        {
            if (Application.isPlaying)
                dtc.AddCardToHand(true, card);
        }

        GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
        if (GUILayout.Button("Create card for Enemy", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        {
            if (Application.isPlaying)
                dtc.AddCardToHand(false, card);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Remove card for Player", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        {
            if (Application.isPlaying)
                dtc.RemoveCard(true, card);
        }

        GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
        if (GUILayout.Button("Remove card for Enemy", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        {
            if (Application.isPlaying)
                dtc.RemoveCard(false, card);
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Discard Player's hand", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        {
            if (Application.isPlaying)
                dtc.DiscardHand(true);
        }

        GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
        if (GUILayout.Button("Discard Enemy's hand", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        {
            if (Application.isPlaying)
                dtc.DiscardHand(false);
        }
        GUILayout.EndHorizontal();
		
		
		GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Clear Player's Standby", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        {
            if (Application.isPlaying)
                dtc.ClearStandBy(true);
        }

        GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
        if (GUILayout.Button("Clear Enemy's Standby", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        {
            if (Application.isPlaying)
                dtc.ClearStandBy(false);
        }
        GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();

        GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
        if (GUILayout.Button("Reveal Enemy's Cards", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        {
            if (Application.isPlaying)
			{
				dtc.RevealEnemy();
			}
        }

        GUI.backgroundColor = new Color(1f, 1f, 1f);
    }
}
