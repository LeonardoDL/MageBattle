using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeBoard : MonoBehaviour
{
    public GameObject board;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = Instantiate(board, new Vector3(0f, 0f, 0f), Quaternion.identity);


        Transform to1 = FindAllChild(go, "ElementsAnimatorPlayer");
        Transform to2 = FindAllChild(go, "ElementsAnimatorEnemy");
        Animator[] aniPlayer = to1.GetComponentsInChildren<Animator>();
        Animator[] aniEnemy = to2.GetComponentsInChildren<Animator>();
        GetComponent<AnimationManager>().animPlayer = aniPlayer;
        GetComponent<AnimationManager>().animEnemy = aniEnemy;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (Time.time == 0)
    //            Time.timeScale = 1f;
    //        else
    //            Time.timeScale = 0f;
    //    }
    //}

    public Transform FindAllChild(GameObject g, string name)
    {
        Transform[] tt = g.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in tt) if (t.gameObject.name == name) return t;
        return null;
    }
}
