using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public bool animate = true;

    [HideInInspector] public Animator[] animPlayer;
    [HideInInspector] public Animator[] animEnemy;

    private Animator cam;
    private Animator hand;
    private GameObject handPanel;
    private GameObject handEnemyPanel;

    private CardType player = CardType.None;
    private CardType enemy = CardType.None;

    void Start()
    {
        cam = Camera.main.GetComponent<Animator>();
        handPanel = GameObject.FindGameObjectWithTag("Hand/PlayerHand");
        handEnemyPanel = GameObject.FindGameObjectWithTag("Hand/EnemyHand");
        hand = handPanel.transform.parent.GetComponent<Animator>();
    }

    void Update()
    {
        if (animPlayer == null || animEnemy == null || animate == false)
            return;

        CardType old_player = BoardManager.GetBoardManager().GetPlayerCard();
        CardType old_enemy = BoardManager.GetBoardManager().GetEnemyCard();

        if (old_player != player)
        {
            if (player != CardType.None)
                animPlayer[(int)player - 1].SetBool("Show", false);

            player = old_player;
            if (player != CardType.None)
                animPlayer[(int)player - 1].SetBool("Show", true);
        }

        if (old_enemy != enemy)
        {
            if (enemy != CardType.None)
                animEnemy[(int)enemy - 1].SetBool("Show", false);

            enemy = old_enemy;
            if (enemy != CardType.None)
                animEnemy[(int)enemy - 1].SetBool("Show", true);
        }

    }

    public void FocusAnimation()
    {
        if (cam.GetBool("Focus") || hand.GetBool("Focus"))
            UnfocusAnimation();
        else
        {
            cam.SetBool("Focus", true);
            hand.SetBool("Focus", true);

            foreach (Animator a in handPanel.GetComponentsInChildren<Animator>())
                a.SetBool("Focus", true);

            foreach (Animator a in handEnemyPanel.GetComponentsInChildren<Animator>())
                a.SetBool("Focus", true);
        }
    }

    public void UnfocusAnimation()
    {
        cam.SetBool("Focus", false);
        hand.SetBool("Focus", false);

        foreach (Animator a in handPanel.GetComponentsInChildren<Animator>())
            a.SetBool("Focus", false);

        foreach (Animator a in handEnemyPanel.GetComponentsInChildren<Animator>())
            a.SetBool("Focus", false);
    }
}
