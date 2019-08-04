using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DevTools))]
public class DevToolsEditor : Editor
{
    private CardType card = CardType.WaterE;
    private bool forPlayer = true;
    private string player;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DevTools dtc = (DevTools) target;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Card", GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
        card = (CardType)EditorGUILayout.EnumPopup(card, GUILayout.MaxWidth(120f), GUILayout.MinWidth(60f));
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Player /Enemy", GUILayout.MaxWidth(100f), GUILayout.MinWidth(70f));
        forPlayer = EditorGUILayout.Toggle(forPlayer, GUILayout.Width(15f));
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();


        GUILayout.BeginHorizontal();

        player = (forPlayer ? "Player" : "Enemy");
        GUI.backgroundColor = (forPlayer ? Color.green : new Color(255f / 255f, 100f / 255f, 100f / 255f));
        if (GUILayout.Button("Create card for " + player, GUILayout.Height(25f)))
        {
            if (Application.isPlaying)
                dtc.AddCardToHand(forPlayer, card);
        }
        GUI.backgroundColor = new Color(1f, 1f, 1f);

        GUI.backgroundColor = (forPlayer ? Color.green : new Color(236f / 255f, 71f / 255f, 71f / 255f));
        if (GUILayout.Button("Remove card for " + player, GUILayout.Height(25f)))
        {
            if (Application.isPlaying)
                dtc.RemoveCard(forPlayer, card);
        }
        GUI.backgroundColor = new Color(1f, 1f, 1f);

        GUILayout.EndHorizontal();


        EditorGUILayout.Space();

        GUI.backgroundColor = (forPlayer ? Color.green : new Color(236f / 255f, 71f / 255f, 71f / 255f));
        if (GUILayout.Button("Discard " + player + "'s hand", GUILayout.Height(25f)))
        {
            if (Application.isPlaying)
                dtc.DiscardHand(forPlayer);
        }
        GUI.backgroundColor = new Color(1f, 1f, 1f);
    }
}
