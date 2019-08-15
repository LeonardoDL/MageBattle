using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeElementsOnMenu : MonoBehaviour
{
    public float time = 4f;
    public Color[] colors;
    [HideInInspector] public Animator[] animPlayer; //Inicializado no Initialize Board
    private int element = -1;
    private Color initial;
    private MeshRenderer mr;
    private float waitTime = 1f;
    private float curTime = -3f;

    void Start()
    {
        animPlayer = GetComponentsInChildren<Animator>();
        mr = GetComponent<MeshRenderer>();
        initial = mr.material.color;
        StartCoroutine(ChangeElement());
    }

    public IEnumerator ChangeElement()
    {
        while (true)
        {
            if (element == -1)
                element = Random.Range(0, animPlayer.Length);
            else
            {
                animPlayer[element].SetBool("Show", false);
                yield return new WaitForSeconds(1f);
                element++;
                if (element >= animPlayer.Length)
                    element = 0;
            }

            switch (element)
            {
                case 0: waitTime = .1f; break;
                case 1: waitTime = .2f; break;
                case 2: waitTime = 1.5f; break;
                case 3: waitTime = 1f; break;
                case 4: waitTime = .5f; break;
                case 5: waitTime = .1f; break;
            }

            curTime = Time.time;
            animPlayer[element].SetBool("Show", true);

            yield return new WaitForSeconds(time);
        }
    }

    void FixedUpdate()
    {
        if (Time.time < curTime + waitTime)
            return;

        mr.material.color = Color.Lerp(mr.material.color,
                                        new Color(GetColor().r, GetColor().g, GetColor().b),
                                        .1f);
    }

    private Color GetColor()
    {
        return colors[element];
    }
}
