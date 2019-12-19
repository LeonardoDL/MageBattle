using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManagerHelper : MonoBehaviour
{
    private BoardManager bg;
    private Deck deck;
    private Discard discard;
    private GameObject playerHand;
    private GameObject playerStandBy;
    private EnemyManager enemy;
    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<BoardManager>();
        bg.SetBoardManagerHelper(this);
    }

    public void WaitStart()
    {
        deck = bg.deck;
        discard = bg.discard;
        playerHand = bg.playerHand;
        playerStandBy = bg.playerStandBy;
        enemy = bg.enemy;
    }

    public bool PlayerHasWinnableCard() //This only works for arcana
    {
        CardInHand[] hand = playerHand.GetComponentsInChildren<CardInHand>();

        foreach (CardInHand cih in hand)
            if (cih.gameObject.GetComponent<Power>() != null)
                return true;

        foreach (CardInHand cih in hand)
        {
            Effect e = cih.gameObject.GetComponent<Effect>();
            if (e != null && e.cardType == CardType.Portal)
                return true;
        }

        return false;
    }

    public void DrawHandEnemy(int qnt)
    {
        enemy.DrawHandEnemy(qnt);
    }

    public void DrawHandEnemyFromDiscard(int qnt)
    {
        enemy.DrawHandEnemyFromDiscard(qnt);
    }

    public void DiscardPlayerHand()
    {
        //discardingHand = true;
        foreach (Transform t in playerHand.GetComponentInChildren<Transform>())
        {
            Power power = t.GetChild(0).gameObject.GetComponent<Power>();
            Effect effect = t.GetChild(0).gameObject.GetComponent<Effect>();
            if (power != null)
            {
                discard.DiscardCard(power.cardType);
            }
            if (effect != null)
            {
                discard.DiscardCard(effect.cardType);
            }
            Destroy(t.gameObject);
        }
        bg.texts[6].text = "" + discard.Size();
        bg.HideElementsFromAnimation(false);
    }

    public void DiscardEnemyHand()
    {
        enemy.DiscardHand();
        bg.HideElementsFromAnimation(false);
    }

    // Retorna as cartas de espera do player e as exclui do campo
    public List<CardType> GetPlayerStandBy()
    {
        List<CardType> standBy = new List<CardType>();
        foreach (Transform t in playerStandBy.GetComponentInChildren<Transform>())
        {
            CardInStandBy cardInStandBy = t.GetChild(0).gameObject.GetComponent<CardInStandBy>();
            standBy.Add(cardInStandBy.card);
            Destroy(t.gameObject);
        }
        return standBy;
    }

    public void ClearPlayerStandBy()
    {
        foreach (CardInStandBy t in playerStandBy.GetComponentsInChildren<CardInStandBy>())
            Destroy(t.transform.parent.gameObject);
    }

    public int GetPlayerStandByCount()
    {
        List<CardType> standBy = new List<CardType>();
        foreach (Transform t in playerStandBy.GetComponentInChildren<Transform>())
        {
            CardInStandBy cardInStandBy = t.GetChild(0).gameObject.GetComponent<CardInStandBy>();
            standBy.Add(cardInStandBy.card);
            //Destroy(t.gameObject);
        }
        return standBy.Count;
    }

    // Retorna uma carta random da mão do player
    public CardType GetPlayerCardRandom()
    {
        //discardingHand = true;
        CardType randomCard = CardType.None;

        int index = Random.Range(0, playerHand.transform.childCount);
        Transform card = playerHand.transform.GetChild(index);

        Power power = card.GetChild(0).gameObject.GetComponent<Power>();
        Effect effect = card.GetChild(0).gameObject.GetComponent<Effect>();
        if (power != null)
        {
            randomCard = power.cardType;
        }
        if (effect != null)
        {
            randomCard = effect.cardType;
        }

        Destroy(card.gameObject);
        return randomCard;
    }

    public void RemoveCardFromPlayer(CardType c)
    {
        bool flag = true;
        foreach (Power p in playerHand.GetComponentsInChildren<Power>())
        {
            if (p.cardType == c)
            {
                flag = false;
                Destroy(p.transform.parent.gameObject);
                discard.DiscardCard(c);
                break;
            }
        }

        if (flag)
            foreach (Effect e in playerHand.GetComponentsInChildren<Effect>())
            {
                if (e.cardType == c)
                {
                    flag = false;
                    Destroy(e.transform.parent.gameObject);
                    discard.DiscardCard(c);
                    break;
                }
            }

        if (flag)
            foreach (CardInStandBy s in playerStandBy.GetComponentsInChildren<CardInStandBy>())
            {
                if (s.card == c)
                {
                    flag = false;
                    Destroy(s.transform.parent.gameObject);
                    break;
                }
            }
    }

    public void RemoveCardFromEnemy(CardType c)
    {
        CardType x = enemy.RemoveCard(c);
        //if (x != CardType.None)
        //{
        //	discard.DiscardCard(x);
        //}
    }

    public void AddEnemyHand(CardType card)
    {
        enemy.AddCardToHand(card);
    }

    public CardType GetEnemyCardRandom(CardType c)
    {
        return enemy.GetCardFromHand(c);
    }

    public void AddPlayerHand(CardType card)
    {
        CardBuilder cardBuilder = deck.GetCardBuilder();
        cardBuilder.BuildCard(card, true);
    }

    public int GetPlayerHandSize() { return playerHand.transform.childCount; }
    public int GetEnemyHandSize() { return enemy.hand.Count; }


    public List<CardType> GetEnemyStandBy()
    {
        return enemy.GetStandby();
    }

    public int GetEnemyStandByCount()
    {
        return enemy.GetStandbyCount();
    }


}
