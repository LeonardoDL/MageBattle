using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(DevTools))]
public class DevToolsEditor : Editor
{
	private EL cardElement = EL.None;
	private PW cardPower = PW.None;
	private EF cardEffect = EF.None;
    private EL cardEToPlay = EL.None;
    private PW cardPToPlay = PW.None;
    private EF cardFToPlay = EF.None;

    private CardType card = CardType.None;
    private CardType cardToPlay = CardType.None;
    private bool waitToPlay = false;
	//private bool forPlayer = true;
	private string rev = "Reveal";
	private bool helpBox = false;

    private bool[] checks = new bool[] { true, true, true };

	public override void OnInspectorGUI()
	{
        DevTools dtc = (DevTools)target;

        GUI.backgroundColor = new Color(.8f, .8f, .8f);
        GUIStyle gst = new GUIStyle(EditorStyles.foldoutHeader);
        gst.fontStyle = FontStyle.Bold;
        gst.fixedWidth = EditorGUIUtility.currentViewWidth - 20f;

        checks[0] = EditorGUILayout.Foldout(checks[0], "Class attributes", gst);
        if (checks[0])
        {
            GUI.backgroundColor = new Color(1f, 1f, 1f);
            DrawDefaultInspector();
            EditorGUILayout.Space();
        }

        GUI.backgroundColor = new Color(.8f, .8f, .8f);
        //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        checks[1] = EditorGUILayout.Foldout(checks[1], "Instantiation attributes", gst);
        if (checks[1])
        {

            EditorGUILayout.LabelField(new GUIContent("Card", "Target Card you want to add/remove"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField(new GUIContent("Element", "x"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
            EditorGUILayout.LabelField(new GUIContent("Power", "x"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
            EditorGUILayout.LabelField(new GUIContent("Effect", "x"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();

            EL o1 = cardElement;
            PW o2 = cardPower;
            EF o3 = cardEffect;
            GUI.backgroundColor = new Color(1f, .9f, .6f);
            cardElement = (EL)EditorGUILayout.EnumPopup(cardElement, GUILayout.MinWidth(60f));
            GUI.backgroundColor = new Color(.75f, 1f, .95f);
            cardPower = (PW)EditorGUILayout.EnumPopup(cardPower, GUILayout.MinWidth(60f));
            GUI.backgroundColor = new Color(1f, .8f, 1f);
            cardEffect = (EF)EditorGUILayout.EnumPopup(cardEffect, GUILayout.MinWidth(60f));
            GUI.backgroundColor = new Color(.8f, .8f, .8f);
            if (o1 != cardElement)
            {
                cardPower = PW.None;
                cardEffect = EF.None;
                card = Translate(cardElement);
            }
            else
            if (o2 != cardPower)
            {
                cardElement = EL.None;
                cardEffect = EF.None;
                card = Translate(cardPower);
            }
            else
            if (o3 != cardEffect)
            {
                cardElement = EL.None;
                cardPower = PW.None;
                card = Translate(cardEffect);
            }

            GUILayout.EndVertical();

            //EditorGUILayout.Space();
            //EditorGUILayout.LabelField("Player /Enemy", GUILayout.MaxWidth(100f), GUILayout.MinWidth(70f));
            //forPlayer = EditorGUILayout.Toggle(forPlayer, GUILayout.Width(15f));
            GUILayout.EndHorizontal();

            //EditorGUILayout.EnumPopup(card, GUILayout.MinWidth(60f));

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(.4f, 1f, .4f);
            if (GUILayout.Button("Create card for Player", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.AddCardToHand(true, card);
            }

            GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
            if (GUILayout.Button("Create card for Enemy", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.AddCardToHand(false, card);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(0f, 1f, 1f);
            if (GUILayout.Button("Add card to Deck", GUILayout.Height(22f), GUILayout.MinWidth(120f)))
            {
                if (Application.isPlaying)
                    dtc.AddCardToDeck(card);
            }

            GUI.backgroundColor = new Color(1f, .7f, .3f);
            if (GUILayout.Button("Add card to Discard", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.AddCardToDiscard(card);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(.4f, 1f, .4f);
            if (GUILayout.Button("Remove card for Player", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.RemoveCard(true, card);
            }

            GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
            if (GUILayout.Button("Remove card for Enemy", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.RemoveCard(false, card);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(.4f, 1f, .4f);
            if (GUILayout.Button("Discard Player hand", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.DiscardHand(true);
            }

            GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
            if (GUILayout.Button("Discard Enemy hand", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.DiscardHand(false);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(.4f, 1f, .4f);
            if (GUILayout.Button("Clear Player Standby", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.ClearStandBy(true);
            }

            GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
            if (GUILayout.Button("Clear Enemy Standby", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.ClearStandBy(false);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(0f, 1f, 1f);
            if (GUILayout.Button("Clear Deck", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.ClearDeck();
            }

            GUI.backgroundColor = new Color(1f, .7f, .3f);
            if (GUILayout.Button("Clear Discard", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                    dtc.ClearDiscard();
            }
            GUILayout.EndHorizontal();



            EditorGUILayout.Space();

            GUI.backgroundColor = (rev == "Reveal") ? new Color(100f / 255f, 180f / 255f, 255f / 255f) : new Color(200f / 255f, 100f / 255f, 255f / 255f);
            if (GUILayout.Button(rev + " Enemy Cards", GUILayout.Height(22f), GUILayout.MinWidth(80f)))
            {
                if (Application.isPlaying)
                {
                    rev = (rev == "Reveal") ? "Hide" : "Reveal";
                    dtc.RevealEnemy();
                }
            }

            EditorGUILayout.Space();
        }

        //EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), new Color(0f, 0f, 0f));
        GUI.backgroundColor = new Color(.8f, .8f, .8f);
        //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //EditorGUILayout.LabelField("Gamestate attributes", EditorStyles.boldLabel);
        checks[2] = EditorGUILayout.Foldout(checks[2], "Gamestate attributes", gst);
        if (checks[2])
        {
            //GUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(.8f, .8f, .8f);
            EditorGUILayout.LabelField(new GUIContent("Forced Card", "Enemy MUST play this card (Even if not allowed)"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
            CardType old_card = cardToPlay;

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            EditorGUILayout.LabelField(new GUIContent("Element", "x"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
            EditorGUILayout.LabelField(new GUIContent("Power", "x"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
            EditorGUILayout.LabelField(new GUIContent("Effect", "x"), GUILayout.MaxWidth(100f), GUILayout.MinWidth(5f));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();

            EL q1 = cardEToPlay;
            PW q2 = cardPToPlay;
            EF q3 = cardFToPlay;

            //GUI.backgroundColor = new Color(1f, .9f, .6f);
            cardEToPlay = (EL)EditorGUILayout.EnumPopup(cardEToPlay, StyleCheck(cardEToPlay), GUILayout.MinWidth(60f));
            //GUI.backgroundColor = new Color(.75f, 1f, .95f);
            cardPToPlay = (PW)EditorGUILayout.EnumPopup(cardPToPlay, StyleCheck(cardPToPlay), GUILayout.MinWidth(60f));
            //GUI.backgroundColor = new Color(1f, .8f, 1f);
            cardFToPlay = (EF)EditorGUILayout.EnumPopup(cardFToPlay, StyleCheck(cardFToPlay), GUILayout.MinWidth(60f));

            GUI.backgroundColor = new Color(.8f, .8f, .8f);
            //cardToPlay = (CardType)EditorGUILayout.EnumPopup(cardToPlay, GUILayout.MinWidth(140f));
            if (q1 != cardEToPlay)
            {
                cardPToPlay = PW.None;
                cardFToPlay = EF.None;
                cardToPlay = Translate(cardEToPlay);
            }
            else
            if (q2 != cardPToPlay)
            {
                cardEToPlay = EL.None;
                cardFToPlay = EF.None;
                cardToPlay = Translate(cardPToPlay);
            }
            else
            if (q3 != cardFToPlay)
            {
                cardEToPlay = EL.None;
                cardPToPlay = PW.None;
                cardToPlay = Translate(cardFToPlay);
            }

            GUILayout.EndVertical();


            GUILayout.EndHorizontal();

            GUI.backgroundColor = new Color(.8f, .8f, .8f);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Block Enemy Actions", "The Enemy can't play cards while this is active"),
                GUILayout.MinWidth(70f));
            bool old_wait = waitToPlay;
            waitToPlay = EditorGUILayout.Toggle(waitToPlay, GUILayout.Width(15f));
            //GUILayout.FlexibleSpace();

            //GUI.backgroundColor = (Application.isPlaying && dtc.IsEnemyWaiting()) ? new Color(.82f, 1f, .4f) : new Color(.8f, .8f, .8f);

            if (GUILayout.Button(new GUIContent("Step", "Unblocks the Player for only 1 action"),
                GUILayout.Height(15f), GUILayout.MinWidth(30f)))
            {
                if (Application.isPlaying)
                {
                    if (!waitToPlay) waitToPlay = true;
                    dtc.StartCoroutine(dtc.SetWaitToPlayOneStep());
                }
            }
            GUI.backgroundColor = new Color(.8f, .8f, .8f);
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUIStyle s = new GUIStyle() { alignment = TextAnchor.MiddleLeft };
            EditorGUILayout.LabelField(new GUIContent("Refresh Enemy Play", "The Enemy tries to play a Power or Effect again"), s,
                GUILayout.Height(25f), GUILayout.MinWidth(70f));

            GUI.backgroundColor = new Color(1f, 1f, 0f);
            if (GUILayout.Button("PlayPowerOrEffect()",
                GUILayout.Height(25f), GUILayout.MinWidth(140f)))
            {
                if (Application.isPlaying)
                    dtc.RefreshEnemy();
            }
            GUILayout.EndHorizontal();

            GUI.backgroundColor = new Color(.8f, .8f, .8f);

            if (!Application.isPlaying)
            {
                if (waitToPlay == true || cardToPlay != CardType.None || card != CardType.None)
                    helpBox = true;
                waitToPlay = false;
                cardToPlay = CardType.None;
                card = CardType.None;
                cardElement = cardEToPlay = EL.None;
                cardPower = cardPToPlay = PW.None;
                cardEffect = cardFToPlay = EF.None;
            }
            else
                helpBox = false;

            if (helpBox)
                EditorGUI.HelpBox(EditorGUILayout.GetControlRect(false, 40f), "Run the game first to change options", MessageType.Error);

            //if (Application.isPlaying)
            //{
            //	if (dtc.IfCardPlayed(cardToPlay))
            //	{
            //		old_card = CardType.None;
            //		cardToPlay = CardType.None;
            //		dtc.SetForcedCard(CardType.None);
            //	}
            //}

            if (old_card != cardToPlay)
                if (Application.isPlaying)
                {
                    dtc.SetForcedCard(cardToPlay);
                }

            if (old_wait != waitToPlay)
                if (Application.isPlaying)
                    dtc.SetWaitToPlay(waitToPlay);
        }

		//GUI.backgroundColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
		//if (GUILayout.Button("Force Card", GUILayout.Height(25f), GUILayout.MinWidth(80f)))
		//{
		//	if (Application.isPlaying)
		//	{
		//		dtc.SetForcedCard(cardToPlay);
		//	}
		//}


	}

    private GUIStyle StyleCheck(EL c)
    {
        GUIStyle z = new GUIStyle(EditorStyles.popup);
        if (c == EL.None)
        {
            z.normal.textColor = new Color(0f, 0f, 0f);
            z.active.textColor = new Color(0f, 0f, 0f);
            z.focused.textColor = new Color(0f, 0f, 0f);
            GUI.backgroundColor = new Color(.8f, .8f, .8f);
        }
        else
        {
            z.normal.textColor = new Color(1f, 1f, 1f);
            z.fontStyle = FontStyle.Bold;
            z.active.textColor = new Color(1f, 1f, 1f);
            z.focused.textColor = new Color(1f, 1f, 1f);
            GUI.backgroundColor = new Color(.75f, 0f, 0f);
        }

        return z;
    }

    private GUIStyle StyleCheck(PW c)
    {
        GUIStyle z = new GUIStyle(EditorStyles.popup);
        if (c == PW.None)
        {
            z.normal.textColor = new Color(0f, 0f, 0f);
            z.active.textColor = new Color(0f, 0f, 0f);
            z.focused.textColor = new Color(0f, 0f, 0f);
            GUI.backgroundColor = new Color(.8f, .8f, .8f);
        }
        else
        {
            z.normal.textColor = new Color(1f, 1f, 1f);
            z.fontStyle = FontStyle.Bold;
            z.active.textColor = new Color(1f, 1f, 1f);
            z.focused.textColor = new Color(1f, 1f, 1f);
            GUI.backgroundColor = new Color(.75f, 0f, 0f);
        }

        return z;
    }

    private GUIStyle StyleCheck(EF c)
    {
        GUIStyle z = new GUIStyle(EditorStyles.popup);
        if (c == EF.None)
        {
            z.normal.textColor = new Color(0f, 0f, 0f);
            z.active.textColor = new Color(0f, 0f, 0f);
            z.focused.textColor = new Color(0f, 0f, 0f);
            GUI.backgroundColor = new Color(.8f, .8f, .8f);
        }
        else
        {
            z.normal.textColor = new Color(1f, 1f, 1f);
            z.fontStyle = FontStyle.Bold;
            z.active.textColor = new Color(1f, 1f, 1f);
            z.focused.textColor = new Color(1f, 1f, 1f);
            GUI.backgroundColor = new Color(.75f, 0f, 0f);
        }

        return z;
    }

    private CardType Translate(EL c)
    {
        return (CardType)c;
    }

    private CardType Translate(PW c)
    {
        return (c == PW.None) ? CardType.None : (CardType)((int)c + 6);
    }

    private CardType Translate(EF c)
    {
        return (c == EF.None) ? CardType.None : (CardType)((int)c + 32);
    }

    private enum EL
    {
        None,
        Water,
        Earth,
        Fire,
        Air,
        Lightning,
        Arcana
    }

    private enum PW
    {
        None,
        Water,
        Earth,
        Fire,
        Air,
        Lightning,

        WaterFire,
        WaterAir,
        WaterLightning,
        WaterEarth,
        FireAir,
        FireLightning,
        FireEarth,
        AirLightning,
        AirEarth,
        EarthLightning,

        WaterFireAir,
        LightningFireWater,
        WaterFireEarth,
        WaterAirLightning,
        WaterAirEarth,
        LightningWaterEarth,
        LightningFireAir,
        FireAirEarth,
        LightningFireEarth,
        LightningAirEarth,

        MegaPower
    }

    private enum EF
    {
        None,
        Intelligence,
        Portal,
        SuperGenius,
        Disintegration,
        BlackHole,
        Eclipse,
        FishingRod
    }
}
