using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discard : MonoBehaviour
{
    public DeckList<CardType> cards;

    public CardBuilder cardBuilder;

    // Start is called before the first frame update
    void Start()
    {
        cards = new DeckList<CardType>();
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
            cardBuilder.BuildCard(c, true);
        }
        catch (System.InvalidOperationException e)
        {
            Debug.Log("No cards! [Discard] " + e.ToString());
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
            Debug.Log("No cards! [Discard] " + e.ToString());
        }

        return c;
    }

    public void DrawHandPlayer(int quantity)
    {
        BoardManager.GetBoardManager().texts[1].text = "Player draws " + quantity + " cards from Discard";
        for (int i = 0; i < quantity; i++)
            DrawCardPlayer();
    }
}
