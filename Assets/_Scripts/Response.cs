using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Response : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Cancel();
    }

    public void Cancel()
    {
        Debug.Log("Starting Resolve()");
        BoardManager.curState = GameState.EnemyResolutionPhase;
        BoardManager.GetBoardManager().Resolve();
    }
}
