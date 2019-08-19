using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    public GameState gameState;
    public WinCondition winCondition;
    public CardType playerElement;
    public CardType enemyElement;
    public CardType playerEffect;
    public CardType enemyEffect;
    public UnityEvent whatToDo;

    private BoardManager bm;
    // Start is called before the first frame update
    void Start()
    {
        bm = BoardManager.GetBoardManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (bm == null) bm = BoardManager.GetBoardManager();

        bool flag = true;

        if (gameState != GameState.None && BoardManager.curState != gameState)
            flag = false;

        if (winCondition != WinCondition.Draw && BoardManager.curWinCondition != winCondition)
            flag = false;

        if (playerElement != CardType.None && bm.playerCard != playerElement)
            flag = false;
        if (enemyElement != CardType.None && bm.enemyCard != enemyElement)
            flag = false;

        if (playerEffect != CardType.None && bm.playerEffect != playerEffect)
            flag = false;
        if (enemyEffect != CardType.None && bm.enemyEffect != enemyEffect)
            flag = false;

        if (flag)
            InvokeAndDie();
    }

    public void Die()
    {
        Destroy(this);
    }

    public void InvokeAndDie()
    {
        whatToDo.Invoke();
        Die();
    }
}
