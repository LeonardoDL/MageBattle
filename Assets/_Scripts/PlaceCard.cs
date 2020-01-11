using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCard : MonoBehaviour
{
    public float speed = 11f;
    public Transform point;

    private GameObject go;
    private Quaternion targetRot;
    private Queue<GameObject> queue;

    void Start()
    {
        queue = new Queue<GameObject>();
    }

    public void PlaceOnSlot(GameObject g)
    {
        if (go == null)
        {
            go = g;
            go.GetComponent<Rigidbody>().useGravity = false;

            float f1 = Random.Range(-15f, 15f);
            float f2 = Random.Range(-15f, 15f);
            targetRot = Quaternion.FromToRotation(Vector3.zero, new Vector3(0f, (f1 + f2) / 2, 0f));
        }
        else
            queue.Enqueue(g);
    }

    void Update()
    {
        if (go == null)
            return;

        //go.transform.position = Vector3.Lerp(point.position, go.transform.position, Time.deltaTime * 0.1f);
        go.transform.position = Vector3.MoveTowards(go.transform.position, point.transform.position, Time.deltaTime * speed);
        go.transform.rotation = Quaternion.Lerp(go.transform.rotation, targetRot, Time.deltaTime * 6f);

        if (Vector3.Distance(point.position, go.transform.position) <= 0.15f)
        {
            go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            go.GetComponent<Rigidbody>().velocity = Vector3.zero;
            go.GetComponent<Rigidbody>().useGravity = true;
            //go.name = go.name + "1";

            if (tag.StartsWith("Slot/"))
            {
                //go.name = "AOSJOAIUHSI";
                foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
                {
                    if (t.gameObject.tag == "Particles")
                    {
                        //t.gameObject.name = "QWERTYUIOP";
                        t.gameObject.SetActive(true);
                    }
                }

                //if (!tag.StartsWith("Slot/Portal"))
                if (tag == "Slot/Stack" || tag.StartsWith("Slot/Element"))
                {
                    StartCoroutine(BoardManager.GetBoardManager().CardPlayed(go));
                    go.GetComponent<CardInBoard>().enabled = true;
                    ParticleSystem ps = go.GetComponentInChildren<ParticleSystem>(true);
                    if (ps != null)
                        ps.gameObject.SetActive(true);
                }
            }
            else
            {
                if (go.transform.parent != null)
                    Destroy(go.transform.parent.gameObject);
                else
                    Destroy(go);
            }

            if (queue.Count == 0)
                go = null;
            else
            {
                go = queue.Dequeue();
                if (go == null)
                {
                    return;
                }

                go.GetComponent<Rigidbody>().useGravity = false;
                float f1 = Random.Range(-15f, 15f);
                float f2 = Random.Range(-15f, 15f);
                targetRot = Quaternion.FromToRotation(Vector3.zero, new Vector3(0f, (f1 + f2) / 2, 0f));
            }
        }
    }
}
