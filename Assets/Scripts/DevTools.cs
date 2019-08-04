using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    private BoardManager board_m;
    private AnimationManager animation_m;
    private EnemyManager enemy_m;

    private GameObject playerHand;
    private GameObject enemyHand;

    void Start()
    {
        Transform manager = transform.parent;
        board_m = manager.GetComponent<BoardManager>();
        animation_m = manager.GetComponent<AnimationManager>();
        enemy_m = manager.GetComponent<EnemyManager>();
    }

    public void AddCardToHand(bool forPlayer, CardType c)
    {
        if (CheckPointers()) return;

        //Debug.Log(c.ToString() + " for the " + (forPlayer ? "Player" : "Enemy"));
        if (forPlayer)
            board_m.deck.CreateCardForPlayer(c);
        else
            board_m.deck.CreateCardForEnemy(c);
    }

    public void RemoveCard(bool forPlayer, CardType c)
    {
        //Debug.Log(c.ToString() + " removed from the " + (forPlayer ? "Player" : "Enemy"));
        if (forPlayer)
            board_m.RemoveCardFromPlayer(c);
        else
            board_m.RemoveCardFromEnemy(c);
    }

    public void DiscardHand(bool forPlayer)
    {
        if (forPlayer)
            board_m.DiscardPlayerHand();
        else
            board_m.DiscardEnemyHand();
    }

    public void ClearStandBy(bool forPlayer)
    {
        if (forPlayer)
            Debug.Log("Clear Player's StandBy to do");
        else
            Debug.Log("Clear Enemy's StandBy to do");
    }

    //To Do:
    //- Clear Hands
    //- Clear Stand-Bies
    //- Force Enemy to play a card
    //- Force Enemy to play multiple cards
    //- Reveal Opponent's Hand
    //- Reveal Opponent's Stand-By

    public bool CheckPointers()
    {
        if (board_m == null || animation_m == null || enemy_m == null)
        {
            Debug.Log("Problems with pointers");
            return true;
        }
        return false;
    }
}
