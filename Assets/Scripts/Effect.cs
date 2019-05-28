using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public delegate bool Execute();
    public Execute execute;

    public CardType cardType;

    public void Init(CardType c)
    {
        cardType = c;
        switch (c)
        {
            case CardType.Intelligence:
                execute = Intelligence;
            break;

            case CardType.Portal:
                execute = Portal;
            break;

            case CardType.SuperGenius:
                execute = SuperGenius;
                break;
        }
    }

    public bool Intelligence()
    {
        BoardManager bm = BoardManager.GetBoardManager();
        bm.deck.DrawHandPlayer(2);
        return true;
    }

    public bool Portal()
    {
        BoardManager bm = BoardManager.GetBoardManager();
        bm.deck.DrawHandPlayer(2);
        return true;
    }

    public bool SuperGenius()
    {
        BoardManager bm = BoardManager.GetBoardManager();
        bm.deck.DrawHandPlayer(2);
        return true;
    }
}
