using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpWhenWinning : MonoBehaviour
{
    public Color[] waterColor;
    public Color[] earthColor;
    public Color[] fireColor;
    public Color[] airColor;
    public Color[] lightningColor;
    public Color[] arcanaColor;

    private int which;
    private Color initial;
    private MeshRenderer mr;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        initial = mr.material.color;

        if (tag == "Board")
        {
            which = 0;
            PassUpColors();
        }
        if (tag.StartsWith("Slot/Element"))
            which = 1;
        if (tag.StartsWith("Slot/Effect"))
            which = 2;
    }

    void Update()
    {
        //PassUpColors();

        mr.material.color = Color.Lerp( mr.material.color,
                                        new Color(GetColor().r, GetColor().g, GetColor().b),
                                        .05f);
    }

    private Color GetColor()
    {
        if (BoardManager.curWinCondition == WinCondition.Victory)
            return SelectColor(BoardManager.GetBoardManager().playerCard, which);

        if (BoardManager.curWinCondition == WinCondition.Loss)
            return SelectColor(BoardManager.GetBoardManager().enemyCard, which + (SameElements()?3:0));

        return initial;
    }

    private Color SelectColor(CardType ct, int i)
    {
        switch (ct)
        {
            case CardType.WaterE:       return waterColor[i];
            case CardType.EarthE:       return earthColor[i];
            case CardType.FireE:        return fireColor[i];
            case CardType.AirE:         return airColor[i];
            case CardType.LightningE:   return lightningColor[i];
            case CardType.ArcanaE:      return arcanaColor[i];

            default:  return Color.black;
        }
    }

    private void PassUpColors()
    {
        foreach (LightUpWhenWinning lw in GetComponentsInChildren<LightUpWhenWinning>())
        {
            lw.waterColor = waterColor;
            lw.earthColor = earthColor;
            lw.fireColor = fireColor;
            lw.airColor = airColor;
            lw.lightningColor = lightningColor;
            lw.arcanaColor = arcanaColor;
        }
    }

    private bool SameElements()
    {
        return BoardManager.GetBoardManager().playerCard == BoardManager.GetBoardManager().enemyCard;
    }
}