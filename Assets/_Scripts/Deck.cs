using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeckLoadMethod
{
    Random,
    OneOfEach,
    PreDetermined,
    Tutorial
}

public class Deck : MonoBehaviour
{
    public int deckSize = 20;
    public DeckList<CardType> cards;
    public List<CardType> cardsShow;
    public CardBuilder cardBuilder;
    public DeckLoadMethod method = DeckLoadMethod.PreDetermined;

    // Start is called before the first frame update
    void Start()
    {
        cards = new DeckList<CardType>();
        switch (method)
        {
            case DeckLoadMethod.Random:
                PopulateRandom();
                break;
            case DeckLoadMethod.OneOfEach:
                PopulateOneOfEach();
                break;
            case DeckLoadMethod.PreDetermined:
                PopulatePreDetermined();
                break;
            case DeckLoadMethod.Tutorial:
                PopulateTutorial();
                break;
        }

        //PopulatePreDetermined();
        //DebugAllDeck();
    }

    void Update()
    {
        int i = 0;
        for (; i < cards.Count; i++)
        {
            if (cardsShow.Count == i)
                cardsShow.Add(CardType.None);
            if (cards[i] != cardsShow[i])
                cardsShow[i] = cards[i];
        }
        for(; i < cardsShow.Count; i++)
            cardsShow.RemoveAt(i);
    }

    public void PopulateRandom()
    {
        for (int i = 0; i < (int)(deckSize * 0.4f); i++)
            cards.Push((CardType)Random.Range(1, 7));
        //Debug.Log((deckSize - cards.Count));
        for (int i = 0; i < (deckSize-cards.Count); i++)
            cards.Push((CardType)Random.Range(7, CardType.GetNames(typeof(CardType)).Length));
        cards.Shuffle();
    }

    public void PopulateOneOfEach()
    {
        for (int i = 1; i < CardType.GetNames(typeof(CardType)).Length; i++)
            cards.Push((CardType)i);

        cards.Shuffle();
    }

    public void PopulatePreDetermined()
    {
        // Cartas de elementos
        cards.Push(CardType.AirE, 6);
        cards.Push(CardType.EarthE, 6);
        cards.Push(CardType.FireE, 6);
        cards.Push(CardType.LightningE, 6);
        cards.Push(CardType.WaterE, 6);
        cards.Push(CardType.ArcanaE, 6);

        // Cartas de poderes unicos
        cards.Push(CardType.AirP, 1);
        cards.Push(CardType.EarthP, 1);
        cards.Push(CardType.FireP, 1);
        cards.Push(CardType.LightningP, 1);
        cards.Push(CardType.WaterP, 1);

        // Cartas de poderes duplos
        cards.Push(CardType.WaterFireP, 1);
        cards.Push(CardType.WaterAirP, 1);
        cards.Push(CardType.WaterLightningP, 1);
        cards.Push(CardType.WaterEarthP, 1);
        cards.Push(CardType.FireAirP, 1);
        cards.Push(CardType.FireLightningP, 1);
        cards.Push(CardType.FireEarthP, 1);
        cards.Push(CardType.AirLightningP, 1);
        cards.Push(CardType.AirEarthP, 1);
        cards.Push(CardType.EarthLightningP, 1);

        // Cartas de poderes triplos
        cards.Push(CardType.WaterFireAirP, 1);
        cards.Push(CardType.LightningFireWaterP, 1);
        cards.Push(CardType.WaterFireEarthP, 1);
        cards.Push(CardType.WaterAirLightningP, 1);
        cards.Push(CardType.WaterAirEarthP, 1);
        cards.Push(CardType.LightningWaterEarthP, 1);
        cards.Push(CardType.LightningFireAirP, 1);
        cards.Push(CardType.FireAirEarthP, 1);
        cards.Push(CardType.LightningFireEarthP, 1);
        cards.Push(CardType.LightningAirEarthP, 1);

        cards.Push(CardType.MegaPowerP, 5);

        // Cartas de Efeito
        cards.Push(CardType.Intelligence, 4);
        cards.Push(CardType.Portal, 5);
        cards.Push(CardType.SuperGenius, 3);
        cards.Push(CardType.Disintegration, 4);
        cards.Push(CardType.BlackHole, 3);
        cards.Push(CardType.Eclipse, 4);
        cards.Push(CardType.FishingRod, 5);

        cards.Shuffle();
    }

    public void PopulateTutorial()
    {
        cards.Push(CardType.FishingRod, 1);
        cards.Push(CardType.Portal, 1);
        cards.Push(CardType.MegaPowerP, 1);

        cards.Push(CardType.EarthE, 1);
        cards.Push(CardType.LightningE, 1);
        cards.Push(CardType.AirE, 1);
        cards.Push(CardType.WaterFireP, 1);
        cards.Push(CardType.MegaPowerP, 1);
        cards.Push(CardType.Portal, 1);
        cards.Push(CardType.SuperGenius, 1);
        cards.Push(CardType.Intelligence, 1);
        cards.Push(CardType.ArcanaE, 1);

        cards.Push(CardType.LightningFireWaterP, 1);
        cards.Push(CardType.FireE, 1);
        cards.Push(CardType.FireAirP, 1);
        cards.Push(CardType.FireLightningP, 1);

        cards.Push(CardType.ArcanaE, 1);

        cards.Push(CardType.FireE, 1);
        cards.Push(CardType.FireE, 1);
        cards.Push(CardType.FireE, 1);
        cards.Push(CardType.FireE, 1);

        cards.Push(CardType.Disintegration, 1);
        cards.Push(CardType.BlackHole, 1);
        cards.Push(CardType.Eclipse, 1);
        cards.Push(CardType.FishingRod, 1);
        cards.Push(CardType.WaterE, 1);


        cards.Push(CardType.FireE, 1);
        cards.Push(CardType.FireP, 1);
        cards.Push(CardType.WaterEarthP, 1);
        cards.Push(CardType.LightningFireAirP, 1);
        cards.Push(CardType.WaterP, 1);
        cards.Push(CardType.EarthLightningP, 1);
        cards.Push(CardType.WaterFireP, 1);

        cards.Push(CardType.AirE, 1);
        cards.Push(CardType.AirP, 1);
        cards.Push(CardType.FireAirP, 1);
        cards.Push(CardType.WaterAirEarthP, 1);
        cards.Push(CardType.MegaPowerP, 1);
        cards.Push(CardType.WaterLightningP, 1);
        cards.Push(CardType.WaterFireP, 1);
    }

    public void DebugAllDeck()
    {
        foreach (CardType c in cards)
            Debug.Log(c.ToString());
    }
    
    public void DrawHandPlayer(int quantity)
    {
        if (quantity < 0) quantity = 0;

        BoardManager.GetBoardManager().texts[1].text = "Player draws " + quantity + " cards";
        for (int i = 0; i < quantity; i++)
            DrawCardPlayer();
    }

    public void DrawCardPlayer()
    {
        CardType c = CardType.None;
        try
        {
            c = cards.Draw();
            cardBuilder.BuildCard(c, true);
        }
        catch (System.InvalidOperationException e)
        {
            Debug.Log("No cards! [Deck] " + e.ToString());
            BoardManager.GetBoardManager().endGame = true;
        }
    }

    public CardType DrawCardEnemy()
    {
        CardType c = CardType.None;
        try
        {
            c = cards.Draw();
            cardBuilder.BuildCard(c, false);
        }
        catch (System.InvalidOperationException e)
        {
            Debug.Log("No cards! [Deck] " + e.ToString());
            BoardManager.GetBoardManager().endGame = true;
        }

        return c;
    }

    public void CreateCardForPlayer(CardType c)
    {
        if (c != CardType.None)
            cardBuilder.BuildCard(c, true);
    }

    public void CreateCardForEnemy(CardType c)
    {
        cardBuilder.BuildCard(c, false);
    }

    public void AddCard(CardType card)
    {
        cards.Push(card);
    }

     public void Shuffle()
    {
        cards.Shuffle();
    }

    public CardBuilder GetCardBuilder(){
        return cardBuilder;
    }

    public int Size(){
        return cards.Count;
    }
}
