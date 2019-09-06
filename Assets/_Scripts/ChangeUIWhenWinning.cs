using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeUIWhenWinning : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public Image[] imgs;
    public EnemyManager em;

    private BoardManager bm;
    private LightUpWhenWinning luwNormal;
    private LightUpWhenWinning luwLight;
    private float targetSize = 1f;
    //public Color winning;
    //public Color losing;

    private Color targetN;
    private Color targetL;
    private Color initial;
    private float cSize = 1f;

    void Start()
    {
        initial = targetN = targetL = tmp.color;
        bm = BoardManager.GetBoardManager();
    }

    void Update()
    {
        if (bm == null)
            bm = BoardManager.GetBoardManager();

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
            if (em.Ipass && bm.last == LastPlayed.Enemy)
            {
                targetN = Color.white;
                targetSize = 1.3f;
            }
        }
        else
            targetSize = 1f;

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
        {
            i.color = Color.Lerp(i.color, targetN, .1f);
            cSize = Mathf.Lerp(cSize, targetSize, .1f);
            i.rectTransform.localScale = Vector3.one * cSize;
        }
    }
}
