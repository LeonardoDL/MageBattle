﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<CardType> hand;
    public List<CardType> standBy;

    public Transform enemySlot;
    public Vector3 offset;
    public bool isActive;

    private Deck deck;
    Dictionary<CardType, Power> powers;
    Dictionary<CardType, Effect> effects;

    // Start is called before the first frame update
    void Awake()
    {
        if (isActive)
        {
            hand = new List<CardType>();
            standBy = new List<CardType>();
            deck = GameObject.FindWithTag("Deck").GetComponent<Deck>();
        }

        powers = new Dictionary<CardType, Power>();
        effects = new Dictionary<CardType, Effect>();

        for (int i = 7; i < 33; i++)
        {
            Power p = gameObject.AddComponent<Power>();
            p.Init((CardType)i);
            powers.Add((CardType)i, p);
        }

        for (int i = 33; i < CardType.GetNames(typeof(CardType)).Length; i++)
        {
            Effect e = gameObject.AddComponent<Effect>();
            e.Init((CardType)i);
            effects.Add((CardType)i, e);
        }
    }

    //public void setEffects(IDictionary<CardType, EnemyEffect> effects){
    //    this.effects = effects;
    //}

    public void DrawHandEnemy(int quantity)
    {
        BoardManager.GetBoardManager().texts[2].text = "Enemy draws " + quantity + " cards";

        for (int i = 0; i < quantity; i++)
        {
            CardType c = deck.DrawCardEnemy();
            if (c != CardType.None)
                hand.Add(c);
        }

        PassElementsToStandBy();
    }

    public void PassElementsToStandBy()
    {
        List<CardType> list_ = new List<CardType>();
        foreach (CardType card in hand)
        {
            if (card == CardType.AirE || card == CardType.ArcanaE || card == CardType.EarthE ||
                card == CardType.FireE || card == CardType.LightningE || card == CardType.WaterE)
            {
                standBy.Add(card);
                list_.Add(card);
            }
        }
        foreach (CardType card in list_)
            hand.Remove(card);
    }

    public void DrawHandEnemyFromDiscard(int quantity)
    {
        BoardManager.GetBoardManager().texts[2].text = "Enemy draws " + quantity + " cards";

        for (int i = 0; i < quantity; i++)
        {
            CardType c = BoardManager.GetBoardManager().discard.DrawCardEnemy();
            if (c != CardType.None)
                hand.Add(c);
        }
    }

    public void PlayElement()
    {
        CardType cardType = standBy[UnityEngine.Random.Range(0, standBy.Count)];
        standBy.Remove(cardType);
        Tuple<Sprite, GameObject> item = deck.cardBuilder.GetSpriteGameObject(cardType);

        GameObject g = Instantiate(item.Item2, enemySlot.position + offset, Quaternion.identity);
        g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.Item1;
        g.GetComponent<CardInBoard>().type = cardType;
        g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.ElementEnemy);

        deck.cardBuilder.RemoveCardFromStandBy();
    }

    public void PlayCardDraw()
    {
        CardType c = CardType.None;

        if (hand.Contains(CardType.Intelligence))
            c = CardType.Intelligence;
        if (hand.Contains(CardType.SuperGenius))
            c = CardType.SuperGenius;

        if (c != CardType.None)
        {
            PlayEffect(c);
        }
    }

    public void PlayPowerOrEffect()
    {
        bool played = false;

        for (int i = 7; i < 33; i++) //Power
            if (hand.Contains((CardType)i))
            {
                played = true;
                switch (BoardManager.GetBoardManager().enemyCard)
                {
                    case CardType.WaterE:
                        if (powers[(CardType)i].water)
                            PlayPower((CardType)i);
                        break;

                    case CardType.EarthE:
                        if (powers[(CardType)i].water)
                            PlayPower((CardType)i);
                        break;

                    case CardType.FireE:
                        if (powers[(CardType)i].water)
                            PlayPower((CardType)i);
                        break;

                    case CardType.AirE:
                        if (powers[(CardType)i].water)
                            PlayPower((CardType)i);
                        break;

                    case CardType.LightningE:
                        if (powers[(CardType)i].water)
                            PlayPower((CardType)i);
                        break;

                    case CardType.ArcanaE:
                        if (powers[(CardType)i].water)
                            PlayPower((CardType)i);
                        break;
                        
                    default:
                        played = false;
                        break;
                }

                break;
            }

        if (!played)
            for (int i = 33; i < CardType.GetNames(typeof(CardType)).Length; i++) //Effect
                if (hand.Contains((CardType)i))
                {
                    PlayEffect((CardType)i);
                    break;
                }
    }

    public void PlayPower(CardType cardType)
    {
        hand.Remove(cardType);
        Tuple<Sprite, GameObject> item = deck.cardBuilder.GetSpriteGameObject(cardType);

        GameObject g = Instantiate(item.Item2, enemySlot.position + offset, Quaternion.identity);
        g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.Item1;
        g.GetComponent<CardInBoard>().type = cardType;
        g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.EffectEnemy);

        deck.cardBuilder.RemoveCardFromHand();
    }

    public void PlayEffect(CardType cardType)
    {
        hand.Remove(cardType);
        Tuple<Sprite, GameObject> item = deck.cardBuilder.GetSpriteGameObject(cardType);

        GameObject g = Instantiate(item.Item2, enemySlot.position + offset, Quaternion.identity);
        g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.Item1;
        g.GetComponent<CardInBoard>().execute = effects[cardType].execute;
        g.GetComponent<CardInBoard>().type = cardType;
        g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.EffectEnemy);

        deck.cardBuilder.RemoveCardFromHand();
    }
}
