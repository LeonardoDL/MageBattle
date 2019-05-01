using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator cam;
    private Animator hand;
    private GameObject handPanel;

    void Start()
    {
        cam = Camera.main.GetComponent<Animator>();
        handPanel = GameObject.FindGameObjectWithTag("Hand");
        hand = handPanel.transform.parent.GetComponent<Animator>();
    }

    public void FocusAnimation()
    {
        if (cam.GetBool("Focus") || hand.GetBool("Focus"))
            UnfocusAnimation();
        else
        {
            cam.SetBool("Focus", true);
            hand.SetBool("Focus", true);

            foreach (Animator a in handPanel.GetComponentsInChildren<Animator>())
                a.SetBool("Focus", true);
        }
    }

    public void UnfocusAnimation()
    {
        cam.SetBool("Focus", false);
        hand.SetBool("Focus", false);

        foreach (Animator a in handPanel.GetComponentsInChildren<Animator>())
            a.SetBool("Focus", false);
    }
}
