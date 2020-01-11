using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevTools : MonoBehaviour
{
    //[Header("Class attributes")]
    public GameState gameState;
    public WinCondition winCondition;
    public LastPlayed lastPlayed;
    public bool inTransition;

    [Header("PlayerPrefs")]
    public bool animate;
    public bool tutorial;
    public Difficulty difficulty;

    public CardType[] responses;
    private GameObject[] resp;

    //public CardType player;
    //public CardType enemy;
    //public CardType enemyEffect;

    private BoardManager board_m;
    private BoardManagerHelper board_mh;
    private AnimationManager animation_m;
	private EnemyManager enemy_m;

	void Start()
	{
		Transform manager = transform.parent;
		board_m = manager.GetComponent<BoardManager>();
        animation_m = manager.GetComponent<AnimationManager>();
		enemy_m = manager.GetComponent<EnemyManager>();
        animate = Options.GetBool("animate");
        tutorial = Options.GetBool("tutorial");
        difficulty = (Difficulty) PlayerPrefs.GetInt("difficulty", 0);

        responses = new CardType[0];
    }

    void Update()
    {
        gameState = BoardManager.curState;
        winCondition = BoardManager.curWinCondition;
        inTransition = BoardManager.isInTransition;
        lastPlayed = board_m.last;

        //if (board_m != null)
        //{
        //    player = board_m.playerCard;
        //    enemy = board_m.enemyCard;
        //    enemyEffect = board_m.enemyEffect;
        //}

        if (responses.Length != board_m.responseStack.Count)
        {
            resp = new GameObject[board_m.responseStack.Count];
            responses = new CardType[board_m.responseStack.Count];
            board_m.responseStack.CopyTo(resp, 0);

            for(int i = 0; i < responses.Length; i++)
                responses[i] = resp[i].GetComponent<CardInBoard>().type;
        }
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
			board_m.bmh.RemoveCardFromPlayer(c);
		else
			board_m.bmh.RemoveCardFromEnemy(c);
	}

	public void DiscardHand(bool forPlayer)
	{
		if (forPlayer)
            board_m.bmh.DiscardPlayerHand();
		else
			board_m.bmh.DiscardEnemyHand();
	}

	public void ClearStandBy(bool forPlayer)
	{
		if (forPlayer)
			board_m.bmh.ClearPlayerStandBy();
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

    //public bool IfCardPlayed(CardType c)
    //{
    //    if (enemy == c || enemyEffect == c)
    //        return true;
    //    return false;
    //}

    public bool IsEnemyWaiting()
    {
        return enemy_m.isWaiting;
    }

    public void RefreshEnemy()
    {
        //BoardManager.curState = GameState.EnemyEffectPhase;
        enemy_m.PlayPowerOrEffect();
    }

    public bool CheckPointers()
	{
		if (board_m == null || animation_m == null || enemy_m == null)
		{
			Debug.Log("Problems with pointers");
			return true;
		}
		return false;
	}

    public void AddSavingCardToPlayerTutorial()
    {
        if (CheckPointers()) return;
        board_m.deck.CreateCardForPlayer(CardType.MegaPowerP);
    }

    public void ClearDiscard()
    {
        board_m.discard.cards = new DeckList<CardType>();
        board_m.texts[6].text = "" + board_m.discard.Size();
    }

    public void AddCardToDiscard(CardType c)
    {
        if (c == CardType.None)
            return;
        board_m.discard.DiscardCard(c);
        board_m.texts[6].text = "" + board_m.discard.Size();
    }

    public void ClearDeck()
    {
        board_m.deck.cards = new DeckList<CardType>();
        board_m.texts[5].text = "" + board_m.deck.Size();
    }

    public void AddCardToDeck(CardType c)
    {
        if (c == CardType.None)
            return;
        board_m.deck.AddCard(c);
        board_m.texts[5].text = "" + board_m.deck.Size();
    }
}
