using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public bool _animate = false;

    public Toggle toggle;
    public Dropdown dropdown;

    public void SetAnimate(bool b)
    {
        SetBool("animate", b);
    }

    public void SetDifficulty(int i)
    {
        PlayerPrefs.SetInt("difficulty", i);
        Debug.Log("difficulty = " + i);
    }

    void Start()
    {
        //for (int i = 0; i < toggles.Length; i++)
        //    toggles[i].isOn = GetBool(variables[i]);
        toggle.isOn = GetBool("animate");
        dropdown.value = PlayerPrefs.GetInt("difficulty");
    }

    void Update()
    {
        //_animate = GetBool("animate");
    }

    public static bool GetBool(string s)
    {
        int x = PlayerPrefs.GetInt(s, 1);
        bool b = (x == 1 ? true : false);
        //Debug.Log(s + " got with " + b);
        return b;
    }

    public static void SetBool(string s, bool b)
    {
        PlayerPrefs.SetInt(s, (b ? 1 : 0));
        //Debug.Log(s + " set with " + GetBool(s));
    }
}
