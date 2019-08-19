using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeUIWhenWinning : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public Image[] imgs;
    private LightUpWhenWinning luwNormal;
    private LightUpWhenWinning luwLight;
    //public Color winning;
    //public Color losing;

    private Color targetN;
    private Color targetL;
    private Color initial;

    void Start()
    {
        initial = targetN = targetL = tmp.color;
    }

    void Update()
    {
        if (luwNormal == null || luwLight == null)
        {
            //luw = GameObject.FindWithTag("Board").GetComponent<LightUpWhenWinning>();
            luwNormal = GameObject.FindWithTag("Slot/ElementPlayer").GetComponent<LightUpWhenWinning>();
            luwLight = GameObject.FindWithTag("Slot/EffectPlayer").GetComponent<LightUpWhenWinning>();

            return;
        }

        targetN = luwNormal.GetColor();
        targetL = luwLight.GetColor();
        if (BoardManager.curWinCondition == WinCondition.Victory)
        {
            tmp.text = "You are winning!";
        }

        if (BoardManager.curWinCondition == WinCondition.Loss)
        {
            tmp.text = "You are losing!";
        }
        if (BoardManager.curWinCondition == WinCondition.Draw)
        {
            targetN = targetL = initial;
            tmp.text = "";
        }
    }

    void FixedUpdate()
    {
        tmp.color = Color.Lerp(tmp.color, targetL, .1f);
        foreach (Image i in imgs)
            i.color = Color.Lerp(i.color, targetN, .1f);
    }
}
