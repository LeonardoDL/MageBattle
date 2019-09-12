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
            Options.SetBool("tutorial", true);
        else
            Options.SetBool("tutorial", false);

        anim.SetTrigger("Fade");
    }

    public void OnFadeOutComplete()
    {
        if (scene == -1)
            Application.Quit();
        //else if (scene == 3)
        //    LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("SampleScene"));
        else
            SceneManager.LoadScene(scene);
    }

    private IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        GameObject.Find("LoadingScreen").GetComponent<LoadSceneHelper>().ChangeLoadingToClick();

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
    }

    public void LoadSceneAsyncStart(int scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }
}
