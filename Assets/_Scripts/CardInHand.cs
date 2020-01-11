using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInHand : MonoBehaviour
{
    private Sprite sprite;
    private Image img;
    private CardType type;

    //public GameObject[] cardPrefabs;
    private GameObject cardPrefab;
    private float zValue = 12f;

    [HideInInspector]
    public bool moveCard = false;
    private Vector3 offset;

    void Update()
    {
        if (moveCard)
            transform.position = Input.mousePosition - offset;
        else
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 8f);

        offset = Vector3.Lerp(offset, Vector3.zero, Time.deltaTime * 5f);
    }

    public bool GetMoveCard()
    {
        return moveCard;
    }

    public void SetSpriteAndPrefab(Sprite s, GameObject p)
    {
        sprite = s;
        cardPrefab = p;
        offset = Vector3.zero;
        img = GetComponent<Image>();
        img.sprite = s;
        //cardPrefab.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = s;
    }


    public void Bind()
    {
        moveCard = !moveCard;
        offset = Input.mousePosition - transform.position;
        //Debug.Log("pos and localPos");
        //Debug.Log(transform.position);
        //Debug.Log(transform.localPosition);
    }

    public void Bind(bool value)
    {
        moveCard = value;
        offset = Input.mousePosition - transform.position;
    }

    public void Summon()
    {
        BoardManager.isInTransition = true;
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zValue));
        GameObject g = Instantiate(cardPrefab, new Vector3(v.x,4f,v.z), Quaternion.identity);

        SetUpOfSummonedCard(g.transform.GetChild(1).gameObject);
        Destroy(transform.parent.gameObject);

        BoardManager.GetBoardManager().bmh.ResumeTime();
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "Hand/PlayerHand")
        {
            if (BoardManager.isInTransition || BoardManager.curState == GameState.EnemyEffectPhase || BoardManager.curState == GameState.EnemyResponsePhase || BoardManager.curState == GameState.EnemyPlayPhase
                || BoardManager.curState == GameState.EndGame)
            {
                moveCard = false;
                return;
            }

            Power p = GetComponent<Power>();
            if (p != null)
            {
                type = p.cardType;
                if ((BoardManager.curState == GameState.PlayerEffectPhase || BoardManager.curState == GameState.PlayerResponsePhase) && Correspondence(BoardManager.GetBoardManager().GetPlayerCard(), p))
                {
                    Summon();
                }
                else
                    moveCard = false;
                //Se o poder for do mesmo elemento que o do slot
                //Fazer o summon()
                //Senao cancelar 
            }
            
            Effect effect = GetComponent<Effect>();

            if (effect != null)
            {
                type = effect.cardType;
                if (effect.isPlayable())
                {
                    if (BoardManager.curState == GameState.EnemyPlayPhase)
                        BoardManager.curState = GameState.PlayerPlayPhase;
                    if (BoardManager.curState == GameState.EnemyEffectPhase)
                        BoardManager.curState = GameState.PlayerEffectPhase;
                    Summon();
                }
                else
                    moveCard = false;

                //switch (effect.cardType)
                //{
                //    case CardType.Intelligence:
                //        Summon();
                //        break;

                //    case CardType.Portal:
                //        //BoardManager.curState = GameState.PlayerEffectPhase;
                //        Summon();
                //        break;

                //    case CardType.SuperGenius:
                //        Summon();
                //        break;

                //    default:
                //        moveCard = false;
                //        break;
                //}
            }
        }
    }

    public bool Correspondence(CardType c, Power p)
    {
        switch (c)
        {
            case CardType.AirE:
                if (p.air)
                    return true;
                break;
            case CardType.EarthE:
                if (p.earth)
                    return true;
                break;
            case CardType.FireE:
                if (p.fire)
                    return true;
                break;
            case CardType.LightningE:
                if (p.lightning)
                    return true;
                break;
            case CardType.WaterE:
                if (p.water)
                    return true;
                break;
            case CardType.ArcanaE:
                if (p.air || p.earth || p.fire || p.lightning || p.water)
                    return true;
                break;
        }

        return false;
    }

    public void SetUpOfSummonedCard(GameObject g)
    {
        g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;
        g.GetComponent<CardInBoard>().type = type;
        Effect e = GetComponent<Effect>();
        if (e != null)
        {
            g.GetComponent<CardInBoard>().execute = e.execute;
            if (e.UI != null)
                g.GetComponent<CardInBoard>().UI = e.UI;
            else
                g.GetComponent<CardInBoard>().UI = null;
        }
    }

    public void AssignType()
    {
        CardInBoard c = GetComponent<CardInBoard>();
        if (tag == "Card/Element")
        {
            type = cardPrefab.GetComponent<CardInBoard>().type;
            return;
        }

        Power p = GetComponent<Power>();
        if(p != null)
        {
            if (p.air)
            {
                type = CardType.AirP;
            }
            if (p.earth)
                type = CardType.EarthP;
            if (p.fire)
                type = CardType.FireP;
            if (p.lightning)
                type = CardType.LightningP;
            if (p.water)
                type = CardType.WaterP;
        }
    }
}
