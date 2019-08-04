using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(DevTools))]
public class DevToolsEditor : Editor
{
	private CardType card = CardType.None;
	private CardType cardToPlay = CardType.None;
	private bool waitToPlay = false;
	//private bool forPlayer = true;
	private string rev = "Reveal";

	public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUI.backgroundColor = new Color(.8f, .8f, .8f);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Instantiation attributes", EditorStyles.boldLabel);

        DevTools dtc = (DevTools) target;

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(new GUIContent("Card", "Target Card you want to add/remove"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
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
		if (GUILayout.Button("Discard Player hand", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
		{
			if (Application.isPlaying)
				dtc.DiscardHand(true);
		}

		GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
		if (GUILayout.Button("Discard Enemy hand", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
		{
			if (Application.isPlaying)
				dtc.DiscardHand(false);
		}
		GUILayout.EndHorizontal();
		
		
		GUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.green;
		if (GUILayout.Button("Clear Player Standby", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
		{
			if (Application.isPlaying)
				dtc.ClearStandBy(true);
		}

		GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
		if (GUILayout.Button("Clear Enemy Standby", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
		{
			if (Application.isPlaying)
				dtc.ClearStandBy(false);
		}
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();

		GUI.backgroundColor = (rev=="Reveal")? new Color(100f / 255f, 180f / 255f, 255f / 255f) : new Color(200f / 255f, 100f / 255f, 255f / 255f);
		if (GUILayout.Button(rev + " Enemy Cards", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
		{
            if (Application.isPlaying)
            {
                rev = (rev == "Reveal") ? "Hide" : "Reveal";
                dtc.RevealEnemy();
            }
		}

		EditorGUILayout.Space();

        //EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), new Color(0f, 0f, 0f));
        GUI.backgroundColor = new Color(.8f, .8f, .8f);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Gamestate attributes", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = new Color(.8f, .8f, .8f);
		EditorGUILayout.LabelField(new GUIContent("Forced Card", "Enemy MUST play this card (Even if not allowed)"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
		CardType old_card = cardToPlay;
		cardToPlay = (CardType)EditorGUILayout.EnumPopup(cardToPlay, GUILayout.MinWidth(60f));

        

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Block Enemy Actions", "The Enemy can't play cards while this is active"),
            GUILayout.MinWidth(70f));
        bool old_wait = waitToPlay;
        waitToPlay = EditorGUILayout.Toggle(waitToPlay, GUILayout.Width(15f));

        if (GUILayout.Button(new GUIContent("Step", "Unblocks the Player for only 1 action"),
            GUILayout.Height(15f), GUILayout.MinWidth(20f)))
        {
            if (Application.isPlaying)
            {
                if (!waitToPlay) waitToPlay = true;
                dtc.StartCoroutine(dtc.SetWaitToPlayOneStep());
            }
        }

        GUILayout.EndHorizontal();

        if (!Application.isPlaying)
        {
            waitToPlay = false;
            cardToPlay = CardType.None;
            card = CardType.None;
        }

        if (old_card != cardToPlay)
            if (Application.isPlaying)
                dtc.SetForcedCard(cardToPlay);

        if (old_wait != waitToPlay)
            if (Application.isPlaying)
                dtc.SetWaitToPlay(waitToPlay);

        //GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
        //if (GUILayout.Button("Force Card", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
        //{
        //	if (Application.isPlaying)
        //	{
        //		dtc.SetForcedCard(cardToPlay);
        //	}
        //}


    }
}
