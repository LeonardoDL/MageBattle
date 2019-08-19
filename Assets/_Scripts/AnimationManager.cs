using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public static bool faded = false;

    //public Transform graph;
    public bool animate = true;
    public float changeSpeed = .15f;

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
        animate = Options.GetBool("animate");

        cam = Camera.main.GetComponent<Animator>();
        handPanel = GameObject.FindGameObjectWithTag("Hand/PlayerHand");
        handEnemyPanel = GameObject.FindGameObjectWithTag("Hand/EnemyHand");
        hand = handPanel.transform.parent.GetComponent<Animator>();
        //animators = handPanel.GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        //_faded = faded;
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
        if (!animate)
            return;

        if (cam.GetBool("Focus") || hand.GetBool("Focus"))
            UnfocusAnimation();
        else
        {
            faded = true;
            cam.SetBool("Focus", true);
            Fade(true);
        }
    }

    public void Fade(bool activate)
    {
        hand.SetBool("Focus", activate);
        FadePlayerCards(activate);
        FadeEnemyCards(activate);
    }

    public void FadePlayerCards(bool activate)
    {
        foreach (Animator a in handPanel.GetComponentsInChildren<Animator>())
        {
            a.SetBool("Focus", activate);
            a.gameObject.GetComponent<Image>().enabled = true;
        }
    }

    public void FadeEnemyCards(bool activate)
    {
        foreach (Animator a in handEnemyPanel.GetComponentsInChildren<Animator>())
        {
            a.SetBool("Focus", activate);
            a.gameObject.GetComponent<Image>().enabled = true;
        }
    }

    public void FadePartial(bool actNumbers, bool actCardsP, bool actCardsE)
    {
        faded = actNumbers || actCardsP || actCardsE;
        hand.SetBool("Focus", actNumbers);
        
        FadePlayerCards(actCardsP);
        FadeEnemyCards(actCardsE);
    }

    public void UnfocusAnimation()
    {
        faded = false;
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
                switch (a.name)
                {
                    case "Arcana":
                        a.SetLayerWeight(1, Mathf.Lerp(a.GetLayerWeight(1), 0.7f, changeSpeed * Time.deltaTime));
                        a.SetLayerWeight(2, Mathf.Lerp(a.GetLayerWeight(2), 0f, changeSpeed * Time.deltaTime));
                        a.SetLayerWeight(3, Mathf.Lerp(a.GetLayerWeight(3), 0f, changeSpeed * Time.deltaTime));
                        break;

                    case "Earth":
                        a.SetLayerWeight(1, Mathf.Lerp(a.GetLayerWeight(1), 0.5f, changeSpeed * Time.deltaTime));
                        a.SetLayerWeight(2, Mathf.Lerp(a.GetLayerWeight(2), 0.3f, changeSpeed * Time.deltaTime));
                        break;

                    case "Air":
                        a.SetLayerWeight(1, Mathf.Lerp(a.GetLayerWeight(1), 0.0f, changeSpeed * Time.deltaTime));
                        a.SetLayerWeight(2, Mathf.Lerp(a.GetLayerWeight(2), 0.2f, changeSpeed * Time.deltaTime));
                        break;

                    default:
                        a.SetLayerWeight(1, Mathf.Lerp(a.GetLayerWeight(1), 0.5f, changeSpeed * Time.deltaTime));
                        a.SetLayerWeight(2, Mathf.Lerp(a.GetLayerWeight(2), 0.5f, changeSpeed * Time.deltaTime));
                        break;
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
                a.SetLayerWeight(3, Mathf.Lerp(a.GetLayerWeight(3), 1f, changeSpeed * Time.deltaTime));
            }

            a.SetLayerWeight(1, Mathf.Lerp(a.GetLayerWeight(1), 1f, changeSpeed * Time.deltaTime));
            a.SetLayerWeight(2, Mathf.Lerp(a.GetLayerWeight(2), 1f, changeSpeed * Time.deltaTime));
        }
    }
}
