using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpWhenWinning : MonoBehaviour
{
    public WinCondition whenToLight;
    private Color initial;
    private MeshRenderer mr;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        initial = mr.material.color;
    }

    void Update()
    {
        if (BoardManager.curWinCondition == whenToLight)
        {
            mr.material.color = new Color(initial.r + .3f, initial.g + .3f, initial.b + .3f);
        }
        else
            mr.material.color = initial;
    }
}
