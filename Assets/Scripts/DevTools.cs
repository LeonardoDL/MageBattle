using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevTools : MonoBehaviour
{
    [Header("Class attributes")]
    public GameState gameState;
    public WinCondition winCondition;

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

    void Update()
    {
        gameState = BoardManager.curState;
        winCondition = BoardManager.curWinCondition;
    }

    public void AddCardToHand(bool forPlayer, CardType c)
	{
		if (CheckPointers()) return;

		//Debug.Log(c.ToString() + " for the " + (forPlayer ? "Player" : "Enemy"));
		if (forPlayer)
			board_m.deck.CreateCardForPlayer(c);
		else
			enemy_m.AddCardForEnemy(c);
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
			board_m.ClearPlayerStandBy();
		else
			enemy_m.ClearStandBy();
	}
	
	public void RevealEnemy()
	{
		board_m.deck.cardBuilder.SwapSpritesEnemyHand();
		board_m.deck.cardBuilder.SwapSpritesEnemyStandBy();
		board_m.deck.cardBuilder.reveal = !board_m.deck.cardBuilder.reveal;
	}

	public void SetForcedCard(CardType c)
	{
		enemy_m.cardToPlay = c;
	}

    public void SetWaitToPlay(bool b)
    {
        enemy_m.isWaiting = b;
    }

    public IEnumerator SetWaitToPlayOneStep()
    {
        enemy_m.isWaiting = false;
        yield return new WaitForSeconds(.1f);
        enemy_m.isWaiting = true;
    }

    //To Do:
    //- Force Enemy to play a card
    //- Force Enemy to play multiple cards

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
