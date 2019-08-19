using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDisableCall : MonoBehaviour
{
    public bool alsoDie;
    public UnityEvent whatToDo;

    void OnDisable()
    {
        whatToDo.Invoke();
        if (alsoDie)
            Destroy(gameObject);
    }
}
