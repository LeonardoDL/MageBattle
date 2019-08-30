using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discard : MonoBehaviour
{
    public List<CardType> cardsShow;
    public DeckList<CardType> cards;

    public CardBuilder cardBuilder;

    // Start is called before the first frame update
    void Start()
    {
        cards = new DeckList<CardType>();
        cardsShow = new List<CardType>();
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
        for (; i < cardsShow.Count; i++)
            cardsShow.RemoveAt(i);
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
            c = cards.DrawRandom();
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
            c = cards.DrawRandom();
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

    public int Size(){
        return cards.Count;
    }
}
