using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeSceneManager : MonoBehaviour
{
    public Animator anim;
    private int scene;

    public void FadeScene(int scene)
    {
        this.scene = scene;
        if (scene == 2) //Tutorial
            Options.SetBool("Tutorial", true);
        else
            Options.SetBool("Tutorial", false);
        anim.SetTrigger("Fade");
    }

    public void OnFadeOutComplete()
    {
        if (scene == -1)
            Application.Quit();
        else
            SceneManager.LoadScene(scene);
    }
}
