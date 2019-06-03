using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public int deckSize = 20;
    public DeckList<CardType> cards;
    public CardBuilder cardBuilder;

    // Start is called before the first frame update
    void Start()
    {
        cards = new DeckList<CardType>();
        PopulatePreDetermined();
        //DebugAllDeck();
    }

    public void PopulateRandom()
    {
        for (int i = 0; i < deckSize; i++)
            cards.Push((CardType)Random.Range(1, CardType.GetNames(typeof(CardType)).Length));
    }

    public void PopulateOneOfEach()
    {
        for (int i = 1; i < CardType.GetNames(typeof(CardType)).Length; i++)
            cards.Push((CardType)i);

        cards.Shuffle();
    }

    public void PopulatePreDetermined()
    {
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

        cards.Push(CardType.Intelligence, 20);
        cards.Push(CardType.Portal, 20);
        cards.Push(CardType.SuperGenius, 20);

        cards.Shuffle();
    }

    public void DebugAllDeck()
    {
        foreach (CardType c in cards)
            Debug.Log(c.ToString());
    }
    
    public void DrawHandPlayer(int quantity)
    {
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
            cardBuilder.BuildCard(c);
        }
        catch (System.InvalidOperationException e)
        {
            Debug.Log("No cards! [Deck]");
            BoardManager.GetBoardManager().endGame = true;
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
            Debug.Log("No cards! [Deck]");
            BoardManager.GetBoardManager().endGame = true;
            //c = GameObject.FindWithTag("Discard").GetComponent<Discard>().DrawCardEnemy();
        }

        return c;
    }
}
