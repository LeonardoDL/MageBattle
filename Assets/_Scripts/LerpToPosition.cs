using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToPosition : MonoBehaviour
{
    public Vector3 target;
    public HingeJoint hj;
    private float speed = 0.6f;

    void Update()
    {
        hj.connectedAnchor = new Vector3(
            Mathf.Lerp(hj.connectedAnchor.x, target.x, Time.deltaTime * speed),
            0f,
            Mathf.Lerp(hj.connectedAnchor.z, target.z, Time.deltaTime * speed));
        speed = speed * 1.2f;
    }
}
