using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffWhenEmpty : MonoBehaviour
{
    private ParticleSystem[] ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentsInChildren<ParticleSystem>();
    }

    public void PlayParticles()
    {
        foreach (ParticleSystem p in ps)
            p.Play();
    }

    public void StopParticles()
    {
        foreach (ParticleSystem p in ps)
        {
            if (p.gameObject.name == "MagicCircle")
            {
                //Debug.Log(p.gameObject.name + " x");
                continue;
            }
            p.Stop();
        }
    }
}
