using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTriggerAdvanced : MonoBehaviour
{
    public int cardsPlayerHand = -1;
    public bool lookForFuckUp = false;
    public TutorialTriggerAdvanced[] activateThese;
    public GameObject[] watchTheseObjects;
    public UnityEvent whatToDo;

    private BoardManager bm;
    private bool turnedOn = false;
    private bool wait = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(.2f);

        bm = BoardManager.GetBoardManager();
        wait = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (wait) return;

        if (bm == null || bm.bmh == null) { bm = BoardManager.GetBoardManager(); return; }

        if (bm.bmh.GetPlayerHandSize() == cardsPlayerHand)
            InvokeAndDie();

        if (lookForFuckUp)
        {
            if (bm.playerCard == CardType.ArcanaE && bm.playerEffect == CardType.Disintegration)
            {
                if (!bm.bmh.PlayerHasWinnableCard())
                    InvokeAndDie();
                else
                {
                    foreach (TutorialTriggerAdvanced tta in activateThese)
                        Destroy(tta);
                    Die();
                }
            }
        }

        if (watchTheseObjects != null)
        {
            if (!turnedOn)
            {
                bool flag = false;
                foreach (GameObject g in watchTheseObjects)
                    if (g.activeSelf)
                        flag = true;

                if (flag == true)
                    turnedOn = true;
            }
            else
            {
                bool flag = false;
                foreach (GameObject g in watchTheseObjects)
                    if (!g.activeSelf)
                        flag = true;

                if (flag == true)
                    InvokeAndDie();
            }
        }
    }

    public void Die()
    {
        Destroy(this);
    }

    public void InvokeAndDie()
    {
        if (activateThese.Length > 0)
            foreach (TutorialTriggerAdvanced tta in activateThese)
                tta.enabled = true;

        whatToDo.Invoke();
        Die();
    }
}
