using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeBoard : MonoBehaviour
{
    public GameObject board;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(board, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time == 0)
                Time.timeScale = 1f;
            else
                Time.timeScale = 0f;
        }
    }
}
