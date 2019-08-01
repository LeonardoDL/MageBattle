using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public bool animate = true;

    [HideInInspector] public Animator[] animPlayer; //Inicializado no Initialize Board
    [HideInInspector] public Animator[] animEnemy; //Inicializado no Initialize Board

    private Animator cam;
    private Animator hand;
    private GameObject handPanel;
    private GameObject handEnemyPanel;
    //private Animator[] animators;

    private CardType player = CardType.None;
    private CardType enemy = CardType.None;

    void Start()
    {
        cam = Camera.main.GetComponent<Animator>();
        handPanel = GameObject.FindGameObjectWithTag("Hand/PlayerHand");
        handEnemyPanel = GameObject.FindGameObjectWithTag("Hand/EnemyHand");
        hand = handPanel.transform.parent.GetComponent<Animator>();
        //animators = handPanel.GetComponentsInChildren<Animator>();
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
            Fade(true);
        }
    }

    public void Fade(bool activate)
    {
        hand.SetBool("Focus", activate);

        foreach (Animator a in handPanel.GetComponentsInChildren<Animator>())
            a.SetBool("Focus", activate);

        foreach (Animator a in handEnemyPanel.GetComponentsInChildren<Animator>())
            a.SetBool("Focus", activate);
    }

    public void UnfocusAnimation()
    {
        cam.SetBool("Focus", false);
        Fade(false);
    }

    public void MakeWeaker(CardType c, bool forPlayer)
    {
        string eName = c.ToString();
        string name = eName.Remove(eName.Length-1);
        //Debug.Log(name);

        Animator[] ani;
        if (forPlayer)
            ani = animPlayer;
        else
            ani = animEnemy;

        foreach (Animator a in ani)
        {
            if (a.name == name)
            {

                if (a.name == "Arcana")
                {
                    a.SetLayerWeight(1, Mathf.Lerp(a.GetLayerWeight(1), 0.7f, .1f));
                    a.SetLayerWeight(2, Mathf.Lerp(a.GetLayerWeight(2), 0f, .1f));
                    a.SetLayerWeight(3, Mathf.Lerp(a.GetLayerWeight(3), 0f, .1f));
                }
                else
                {
                    a.SetLayerWeight(1, Mathf.Lerp(a.GetLayerWeight(1), 0.5f, .1f));
                    a.SetLayerWeight(2, Mathf.Lerp(a.GetLayerWeight(2), 0.5f, .1f));
                }
            }
            
        }
    }

    public void MakeStronger(CardType c, bool forPlayer)
    {
        string eName = c.ToString();
        string name = eName.Remove(eName.Length - 1, 1);

        Animator[] ani;
        if (forPlayer)
            ani = animPlayer;
        else
            ani = animEnemy;

        foreach (Animator a in ani)
        {
            if (a.name == "Arcana")
            {
                a.SetLayerWeight(1, Mathf.Lerp(a.GetLayerWeight(1), 1f, .1f));
                a.SetLayerWeight(2, Mathf.Lerp(a.GetLayerWeight(2), 1f, .1f));
                a.SetLayerWeight(3, Mathf.Lerp(a.GetLayerWeight(3), 1f, .1f));
            }
            else
            {
                a.SetLayerWeight(1, Mathf.Lerp(a.GetLayerWeight(1), 1f, .1f));
                a.SetLayerWeight(2, Mathf.Lerp(a.GetLayerWeight(2), 1f, .1f));
            }
        }
    }
}
