using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseAnimation : MonoBehaviour
{
    public float force = 1000;
    Rigidbody rb;
    Rigidbody rbCam;
    Rigidbody[] rbs;
    Vector3[] directions;
    float antiGravity;
    Transform cam;
    float speed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
        StartCoroutine(DelayedStart());
        antiGravity = 0f;
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
        rbCam = GameObject.FindWithTag("MainCamera").AddComponent<Rigidbody>();
        cam = GameObject.FindWithTag("MainCamera").transform;
        Destroy(cam.GetComponent<Animator>());

        rb.mass = 2f;
        rb.detectCollisions = true;
        foreach (Rigidbody r in rbs)
        {
            r.detectCollisions = true;
            r.mass = 15f;
            r.AddTorque(Random.onUnitSphere * Random.Range(.5f,5f), ForceMode.Impulse);
            r.AddForce(Random.onUnitSphere * Random.Range(10f, 30f), ForceMode.Impulse);
            r.AddForce(transform.up * 40f, ForceMode.Impulse);
        }

        //rb.AddTorque(transform.right * 15f, ForceMode.Impulse);
        rbCam.AddForce(new Vector3(0f, 1f) * 10f, ForceMode.Impulse);

        yield return new WaitForSeconds(2f);
        //antiGravity = 0f;
        speed = 2f;
        yield return new WaitForSeconds(1f);
        //antiGravity = 10f;
        speed = 4f;
        yield return new WaitForSeconds(2f);
        rbCam.AddForce(new Vector3(0f, 1f) * 7f, ForceMode.Impulse);
        speed = 8f;
        antiGravity = 5f;
        yield return new WaitForSeconds(3f);
        rbCam.AddForce(new Vector3(0f, 1f) * 14f, ForceMode.Impulse);
        speed = 16f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(-transform.right * -10f);
        //rb.AddForce(new Vector3(0f,1f) * antiGravity);
        //rb.AddForce(transform.forward * 10f);
        //rb.AddForce(transform.up * 50f);
        foreach (Rigidbody r in rbs)
        {
            r.AddForce(transform.up * 40f);
        }

        //cam.Translate(new Vector3(0f,-1f,0f) * speed * Time.deltaTime);
    }
}
