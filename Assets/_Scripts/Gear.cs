using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public void RotateZ(float speed)
    {
        transform.Rotate(new Vector3(0f, 0f, speed * Time.deltaTime));
    }
}
