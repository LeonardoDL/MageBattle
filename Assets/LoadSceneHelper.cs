using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadSceneHelper : MonoBehaviour
{
    public GameObject loading;
    public GameObject clickToContinue;
    public UnityEvent loadScene;

    [Header("Concept Arts")]
    public Image arts;
    public Sprite[] sprites;
    private int current = -1;

    void Start()
    {
        StartCoroutine(WaitStart());
        InvokeRepeating("RandomArt", 1f, 4f);
    }

    public void ChangeLoadingToClick()
    {
        loading.SetActive(false);
        clickToContinue.SetActive(true);
    }

    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(1f);
        loadScene.Invoke();
    }

    void RandomArt()
    {
        if (current == -1)
            current = 0;
        else
        {
            int t = Random.Range(0, sprites.Length);
            if (t == current)
                current = t + 1;
            else
                current = t;
            if (current == sprites.Length)
                current = 0;
            arts.sprite = sprites[current];
        }
    }
}
