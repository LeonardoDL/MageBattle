using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discard : MonoBehaviour
{
    public DeckList<CardType> cards;

    private GameObject cardPlaceholder;
    private GameObject cardInHand;
    private GameObject panelHand;

    private Sprite[] pwSprites;
    private GameObject powerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        cards = new DeckList<CardType>();
    }

    void Awake()
    {
        Deck deck = GameObject.FindWithTag("Deck").GetComponent<Deck>();
        cardPlaceholder = deck.cardPlaceholder;
        cardInHand = deck.cardInHand;
        panelHand = deck.panelHand;

        pwSprites = deck.pwSprites;
        powerPrefab = deck.powerPrefab;
}

    public void DebugAllDeck()
    {
        foreach (CardType c in cards)
            Debug.Log(c.ToString());
    }
    
    public void DiscardCard(CardType c)
    {
        cards.Add(c);
    }

    public void Shuffle()
    {
        //Debug.Log("Before Shuffle " + cards.Count);
        //DebugAllDeck();
        cards.Shuffle();
        //Debug.Log("After Shuffle");
        //DebugAllDeck();
    }

    public void DrawCardPlayer()
    {
        CardType c = CardType.None;
        try
        {
            c = cards.Draw();
        }
        catch (System.InvalidOperationException e)
        {
            Debug.Log("No cards! [Discard]");
        }

        switch (c)
        {
            case CardType.WaterP:
                NewCardInHand(0).water = true;
                break;
            case CardType.EarthP:
                NewCardInHand(1).earth = true;
                break;
            case CardType.FireP:
                NewCardInHand(2).fire = true;
                break;
            case CardType.AirP:
                NewCardInHand(3).air = true;
                break;
            case CardType.LightningP:
                NewCardInHand(4).lightning = true;
                break;
        }
    }

    public CardType DrawCardEnemy()
    {
        CardType c = CardType.None;
        try
        {
            c = cards.Draw();
        }
        catch (System.InvalidOperationException e)
        {
            Debug.Log("No cards! [Discard]");
        }

        return c;
    }

    public void DrawHandPlayer(int quantity)
    {
        for (int i = 0; i < quantity; i++)
            DrawCardPlayer();
    }

    public Power NewCardInHand(int x)
    {
        GameObject cih = Instantiate(cardInHand, Instantiate(cardPlaceholder, panelHand.transform).transform);
        cih.GetComponent<CardInHand>().SetSpriteAndPrefab(pwSprites[x], powerPrefab);
        //cih.tag = "Card/Power";
        return cih.AddComponent<Power>();
    }
}
