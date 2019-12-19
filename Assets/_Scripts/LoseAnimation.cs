using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseAnimation : MonoBehaviour
{
    public float force = 20f;
    Rigidbody rb;
    Rigidbody[] rbs;

    float speed = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        enabled = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3.39f);

        enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddTorque(transform.right * force, ForceMode.Impulse);
        rb.AddForce(Vector3.up * -10f, ForceMode.Impulse);

        rbs = new Rigidbody[4];
        rbs[0] = GameObject.FindWithTag("Deck").AddComponent<Rigidbody>();
        rbs[1] = GameObject.FindWithTag("VictoryDeck/Player").AddComponent<Rigidbody>();
        rbs[2] = GameObject.FindWithTag("VictoryDeck/Enemy").AddComponent<Rigidbody>();
        rbs[3] = GameObject.FindWithTag("Discard").AddComponent<Rigidbody>();

        rb.detectCollisions = true;
        foreach (Rigidbody r in rbs)
        {
            r.detectCollisions = true;
            r.AddTorque(OnUnitHemisphere() * Random.Range(1f, 5f), ForceMode.Impulse);
            r.AddForce(OnUnitHemisphere() * Random.Range(.5f, 2f), ForceMode.Impulse);
            //r.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(.2f);
        speed = 3.2f;
        yield return new WaitForSeconds(8f);
        speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(transform.right * 10f);
        rb.AddForce(Vector3.up * speed);

        foreach (Rigidbody r in rbs)
        {
            r.AddForce(Vector3.up * (speed-0.2f));
        }
    }

    public Vector3 OnUnitHemisphere()
    {
        var ous = Random.onUnitSphere.normalized;
        ous.y = Mathf.Abs(ous.y);
        return ous;
    }
}
