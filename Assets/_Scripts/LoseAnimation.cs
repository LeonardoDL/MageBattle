using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseAnimation : MonoBehaviour
{
    public float force = 1000;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        rb.AddTorque(transform.right * force);

        GameObject.FindWithTag("Deck").AddComponent<Rigidbody>();
        GameObject.FindWithTag("VictoryDeck/Player").AddComponent<Rigidbody>();
		GameObject.FindWithTag("VictoryDeck/Enemy").AddComponent<Rigidbody>();
		GameObject.FindWithTag("Discard").AddComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(transform.right * 20);
    }
}
