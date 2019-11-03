// Class PlayerActionController

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class PlayerActionController : MonoBehaviour
{
    private static PlayerActionController pac;
    [HideInInspector] public Deck deck;
    [HideInInspector] public Discard discard;
    [HideInInspector] public VictoryDeck victoryDeckPlayer;
    [HideInInspector] public VictoryDeck victoryDeckEnemy;
    [HideInInspector] public CardType playerCard;
	[HideInInspector] public CardType enemyCard;
    [HideInInspector] public CardType playerEffect;
    [HideInInspector] public CardType enemyEffect;
    [HideInInspector] public GameObject playerBoardCard = null;
    [HideInInspector] public GameObject enemyBoardCard = null;

    public Text[] texts;
    private EnemyManager enemy;
	private GameObject playerHand;
    private GameObject playerStandBy;

    void Start()
    {
        playerCard = CardType.None;
        enemyCard = CardType.None;
        playerEffect = CardType.None;
        enemyEffect = CardType.None;
        enemy = GetComponent<EnemyManager>();
        playerHand = GameObject.FindWithTag("Hand/PlayerHand");
        playerStandBy = GameObject.FindWithTag("StandBy");
        deck = GameObject.FindWithTag("Deck").GetComponent<Deck>();
        victoryDeckPlayer = GameObject.FindWithTag("VictoryDeck/Player").GetComponent<VictoryDeck>();
		victoryDeckEnemy  = GameObject.FindWithTag("VictoryDeck/Enemy").GetComponent<VictoryDeck>();
        StartCoroutine(WaitStart());
    }

    private IEnumerator WaitStart()
	{
		yield return new WaitForSeconds(.2f);

		discard = GameObject.FindWithTag("Discard").GetComponent<Discard>();
		DrawCards();
	}

    public Effect GetEffect() { return GetComponent<Effect>(); }
    public CardType GetPlayerCard() { return playerCard; }
	public CardType GetEnemyCard() { return enemyCard; }

    public bool PlayerHasWinnableCard() //This only works for arcana
    {
        CardInHand[] hand = playerHand.GetComponentsInChildren<CardInHand>();

        foreach (CardInHand cih in hand)
            if (cih.gameObject.GetComponent<Power>() != null)
                return true;

        foreach (CardInHand cih in hand)
        {
            Effect e = cih.gameObject.GetComponent<Effect>();
            if (e != null && e.cardType == CardType.Portal)
                return true;
        }

        return false;
    }

    public void DrawHandEnemy(int qnt)
    {
		enemy.DrawHandEnemy(qnt);
	}

    public void DrawHandEnemyFromDiscard(int qnt)
	{
		enemy.DrawHandEnemyFromDiscard(qnt);
	}

	public void DiscardPlayerHand()
	{
		//discardingHand = true;
		foreach (Transform t in playerHand.GetComponentInChildren<Transform>()){
			Power power = t.GetChild (0).gameObject.GetComponent<Power>();
			Effect effect = t.GetChild (0).gameObject.GetComponent<Effect>();
			if(power != null){
				discard.DiscardCard(power.cardType);
			}
			if(effect != null){
				discard.DiscardCard(effect.cardType);
			}
			Destroy(t.gameObject);
		}
        texts[6].text = "" + discard.Size();
        HideElementsFromAnimation(false);
    }

    public void DiscardEnemyHand()
	{
		enemy.DiscardHand();
        HideElementsFromAnimation(false);
    }

    public List<CardType> GetPlayerStandBy()
    {
		List<CardType> standBy = new List<CardType>();
		foreach (Transform t in playerStandBy.GetComponentInChildren<Transform>()){
			CardInStandBy cardInStandBy = t.GetChild (0).gameObject.GetComponent<CardInStandBy>();
			standBy.Add(cardInStandBy.card);
			Destroy(t.gameObject);
		}
		return standBy;
	}

    public void ClearPlayerStandBy()
	{
		foreach (CardInStandBy t in playerStandBy.GetComponentsInChildren<CardInStandBy>())
			Destroy(t.transform.parent.gameObject);
	}

    public int GetPlayerStandByCount()
	{
		List<CardType> standBy = new List<CardType>();
		foreach (Transform t in playerStandBy.GetComponentInChildren<Transform>())
		{
			CardInStandBy cardInStandBy = t.GetChild(0).gameObject.GetComponent<CardInStandBy>();
			standBy.Add(cardInStandBy.card);
			//Destroy(t.gameObject);
		}
		return standBy.Count;
	}

    // Retorna uma carta random da mão do player
	public CardType GetPlayerCardRandom()
    {
		//discardingHand = true;
		CardType randomCard = CardType.None;

		int index = Random.Range(0, playerHand.transform.childCount);
		Transform card = playerHand.transform.GetChild(index);

		Power power = card.GetChild (0).gameObject.GetComponent<Power>();
		Effect effect = card.GetChild (0).gameObject.GetComponent<Effect>();
		if(power != null){
			randomCard = power.cardType;
		}
		if(effect != null){
			randomCard = effect.cardType;
		}

		Destroy(card.gameObject);
		return randomCard;
	}

    public void RemoveCardFromPlayer(CardType c)
	{
		bool flag = true;
		foreach (Power p in playerHand.GetComponentsInChildren<Power>())
		{
			if (p.cardType == c)
			{
				flag = false;
				Destroy(p.transform.parent.gameObject);
				discard.DiscardCard(c);
				break;
			}
		}

		if (flag)
		foreach (Effect e in playerHand.GetComponentsInChildren<Effect>())
		{
			if (e.cardType == c)
			{
				flag = false;
				Destroy(e.transform.parent.gameObject);
				discard.DiscardCard(c);
				break;
			}
		}

		if (flag)
		foreach (CardInStandBy s in playerStandBy.GetComponentsInChildren<CardInStandBy>())
		{
			if (s.card == c)
			{
				flag = false;
				Destroy(s.transform.parent.gameObject);
				break;
			}
		}
	}

    public void RemoveCardFromEnemy(CardType c)
	{
		CardType x = enemy.RemoveCard(c);
		//if (x != CardType.None)
		//{
		//	discard.DiscardCard(x);
		//}
	}

    public void AddEnemyHand(CardType card)
	{
		enemy.AddCardToHand(card);
	}

    public CardType GetEnemyCardRandom(CardType c)
    {
		return enemy.GetCardFromHand(c);
	}

    public void AddPlayerHand(CardType card)
	{
		CardBuilder cardBuilder = deck.GetCardBuilder();
		cardBuilder.BuildCard(card, true);
	}

    public int GetPlayerHandSize(){ return playerHand.transform.childCount;}
	public int GetEnemyHandSize(){ return enemy.hand.Count;}

    public List<CardType> GetEnemyStandBy()
    {
		return enemy.GetStandby();
	}

    public int GetEnemyStandByCount()
	{
		return enemy.GetStandbyCount();
	}

    public void DrawCards()
	{
		int pHandSize = playerHand.transform.childCount;
		//Debug.Log(pHandSize);
		deck.DrawHandPlayer(7 - pHandSize);
		enemy.DrawHandEnemy(7 - enemy.hand.Count);

		int k = 0;
		while (k < 12)
		{
			if (playerHand.transform.childCount >= 7 && playerStandBy.transform.childCount <= 0)
			{
				Debug.Log("No Elements To Battle! [Player]");
				pac.texts[0].text = "Player has no\nElements To Battle!";
				DiscardPlayerHand();
				//foreach (Transform t in playerHand.GetComponentInChildren<Transform>())
				//    Destroy(t.gameObject);
				deck.DrawHandPlayer(7);
			}
			else
				break;

			k++;
		}

		k = 0;
		while (k < 12)
		{
			if (enemy.hand.Count >= 7 && enemy.standBy.Count <= 0)
			{
				Debug.Log("No Elements To Battle! [Enemy] ");
				if (pac.texts[0].text == "Player has no\nElements To Battle!")
					pac.texts[0].text = "No one has any\nElements To Battle!";
				else
					pac.texts[0].text = "Enemy has no\nElements To Battle!";

				//int t = enemy.hand.Count;
				//for (int i = 0; i < t; i++)
				//{
				//    enemy.hand.RemoveAt(0);
				//    deck.cardBuilder.RemoveCardFromHand();
				//    new WaitForSeconds(1f);
				//}
				DiscardEnemyHand();
				enemy.DrawHandEnemy(7);
			}
			else
				break;

			k++;
		}
        //! FIXME: endGame no longer is part of this class
		if (endGame)
		{
			//Debug.Log("Deck has " + deck.cards.Count + " cards");
			curState = GameState.EndGame;
			EndGame();
		}
		else
		{
			curState = GameState.PlayerPlayPhase;
		}

		texts[5].text = "" + deck.Size();
		texts[6].text = "" + discard.Size();

		passButton.SetActive(false);
	}

    public EnemyManager getEnemy() { return enemy; }
}