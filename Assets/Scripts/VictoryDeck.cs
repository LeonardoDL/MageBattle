using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDeck : MonoBehaviour
{
    public bool isPlayer;
    private int score = 0;
    public int GetScore() { return score; }

    public void AddCard(CardType c)
    {
        switch (c)
        {
            case CardType.AirE:
                score++;
                break;
            case CardType.EarthE:
                score++;
                break;
            case CardType.FireE:
                score++;
                break;
            case CardType.LightningE:
                score++;
                break;
            case CardType.WaterE:
                score++;
                break;
            case CardType.ArcanaE:
                score += 2;
                break;
        }

        if (isPlayer)
            BoardManager.GetBoardManager().texts[3].text = "" + score;
        else
            BoardManager.GetBoardManager().texts[4].text = "" + score;
    }
}
