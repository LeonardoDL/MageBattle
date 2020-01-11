using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeStopEffect : MonoBehaviour
{
    public Color color;
    public float speed = .05f;

    private Color initial;
    private Image image;

    private Color target;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        //image.enabled = false;
        initial = image.color;
        target = initial;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale < .5f)
        {
            //image.enabled = true;
            target = color;
        }
        else
            target = initial;

        //if (Vector4.Distance(target, image.color) < .01f)
        //    image.enabled = false;

        image.color = Color.Lerp(image.color, target, speed);
    }
}
