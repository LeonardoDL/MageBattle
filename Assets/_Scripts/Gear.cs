using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gear : MonoBehaviour
{
    //public void RotateZ(float speed)
    //{
    //    transform.Rotate(new Vector3(0f, 0f, speed * Time.deltaTime));
    //}
    public GameObject[] gearOpen;
    public GameObject[] gearClose;
    private bool open = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (open)
            {
                foreach (GameObject g in gearOpen)
                    g.SetActive(false);

                foreach (GameObject g in gearClose)
                    g.SetActive(true);

                open = false;
            }
            else
            {
                foreach (GameObject g in gearOpen)
                    g.SetActive(true);

                foreach (GameObject g in gearClose)
                    g.SetActive(false);

                open = true;
            }
        }
    }
}
