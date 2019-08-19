using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditAnimation : MonoBehaviour
{
    public float time;
    public GameObject next;

    void OnEnable()
    {
        StartCoroutine(WaitTimeAndNext());
    }

    public void DisableChildren()
    {
        foreach (Animator a in GetComponentsInChildren<Animator>())
            a.SetBool("Fade", true);
    }

    public IEnumerator WaitTimeAndNext()
    {
        yield return new WaitForSeconds(time);
        DisableChildren();
  
        yield return new WaitForSeconds(1f);
        if (next != null)
            next.SetActive(true);
        gameObject.SetActive(false);
    }
}
