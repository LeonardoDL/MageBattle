using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetDifficulty : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        bool tut = Options.GetBool("tutorial");
        GetComponent<TextMeshProUGUI>().text = (tut ? "Difficulty: Tutorial" : "Difficulty: " + (Difficulty)PlayerPrefs.GetInt("difficulty", 0));
    }
}
