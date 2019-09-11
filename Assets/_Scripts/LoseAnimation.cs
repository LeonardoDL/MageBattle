using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseAnimation : MonoBehaviour
{
    public float force = 1000;
    Rigidbody rb;
    Rigidbody[] rbs;
    Vector3[] directions;
    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2f);

        enabled = true;
        rb = transform.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        rb.AddTorque(transform.right * force);

        rbs = new Rigidbody[4];
        rbs[0] = GameObject.FindWithTag("Deck").AddComponent<Rigidbody>();
        rbs[1] = GameObject.FindWithTag("VictoryDeck/Player").AddComponent<Rigidbody>();
        rbs[2] = GameObject.FindWithTag("VictoryDeck/Enemy").AddComponent<Rigidbody>();
        rbs[3] = GameObject.FindWithTag("Discard").AddComponent<Rigidbody>();

        rb.detectCollisions = true;
        foreach (Rigidbody r in rbs)
        {
            r.detectCollisions = true;
            r.mass = 15f;
            r.AddTorque(Random.onUnitSphere * Random.Range(.5f,5f), ForceMode.Impulse);
            r.AddForce(Random.onUnitSphere * Random.Range(.5f, 5f), ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(transform.right * 10f);
        foreach (Rigidbody r in rbs)
        {
            r.AddForce(-transform.up * 15f);
        }
    }
}
