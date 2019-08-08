using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
	private static BoardManager bm;
	public Text[] texts;
	[HideInInspector] public Deck deck;
	[HideInInspector] public Discard discard;

	public VictoryDeck victoryDeckPlayer;
	public VictoryDeck victoryDeckEnemy;

	[HideInInspector] public CardType playerCard;
	[HideInInspector] public CardType enemyCard;
    [HideInInspector] public GameObject playerBoardCard;
	[HideInInspector] public GameObject enemyBoardCard;

    public GameObject playerButton;
	public GameObject enemyButton;
	public GameObject passButton;

	[HideInInspector] public bool endGame;
	[HideInInspector] public static bool isInTransition;

	private EnemyManager enemy;
	private GameObject playerHand;
	//public int playerHandSize;

	//public int discardSize;

	private GameObject playerStandBy;

	//public GameState currentState = GameState.None;
	public static GameState curState = GameState.None;

	//public WinCondition currentWinCondition = WinCondition.Draw;
	public static WinCondition curWinCondition = WinCondition.Draw;

	//public bool discardingHand = false;

	//public void Peek() {
	//	currentState = curState;
	//	currentWinCondition = curWinCondition;
	//	playerHandSize = GetPlayerHandSize();
	//	if (discard != null)
	//		discardSize = discard.Size();
	// }

	void Start()
	{
		bm = this;
		curState = GameState.DrawPhase;
		playerCard = CardType.None;
		enemyCard = CardType.None;
        enemy = GetComponent<EnemyManager>();
		playerHand = GameObject.FindWithTag("Hand/PlayerHand");
		playerStandBy = GameObject.FindWithTag("StandBy");
		deck = GameObject.FindWithTag("Deck").GetComponent<Deck>();
		victoryDeckPlayer = GameObject.FindWithTag("VictoryDeck/Player").GetComponent<VictoryDeck>();
		victoryDeckEnemy  = GameObject.FindWithTag("VictoryDeck/Enemy").GetComponent<VictoryDeck>();
		StartCoroutine(WaitStart());
		//enemy.setEffects(deck.GetAllEffects());
	}

	private IEnumerator WaitStart()
	{
		yield return new WaitForSeconds(.1f);

		discard = GameObject.FindWithTag("Discard").GetComponent<Discard>();
		DrawCards();
	}

	public static BoardManager GetBoardManager() { return bm; }

	public Effect GetEffect() { return GetComponent<Effect>(); }

	public CardType GetPlayerCard() { return playerCard; }
	public CardType GetEnemyCard() { return enemyCard; }

	public void DrawHandEnemy(int qnt){
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
    }

	public void DiscardEnemyHand()
	{
		enemy.DiscardHand();
	}

	// Retorna as cartas de espera do player e as exclui do campo
	public List<CardType> GetPlayerStandBy() {
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
	public CardType GetPlayerCardRandom() {
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
		if (x != CardType.None)
		{
			discard.DiscardCard(x);
		}
	}

	public void AddEnemyHand(CardType card)
	{
		enemy.AddCardToHand(card);
	}

	public CardType GetEnemyCardRandom(CardType c) {
		return enemy.GetCardFromHand(c);
	}

	public void AddPlayerHand(CardType card)
	{
		CardBuilder cardBuilder = deck.GetCardBuilder();
		cardBuilder.BuildCard(card, true);
	}

	public int GetPlayerHandSize(){ return playerHand.transform.childCount;}
	public int GetEnemyHandSize(){ return enemy.hand.Count;}


	public List<CardType> GetEnemyStandBy() {
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
				bm.texts[0].text = "Player has no Elements To Battle!";
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
				if (bm.texts[0].text == "Player has no Elements To Battle!")
					bm.texts[0].text = "No one has any Elements To Battle!";
				else
					bm.texts[0].text = "Enemy has no Elements To Battle!";

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

	public IEnumerator CardPlayed(GameObject card)
	{
		CardType cardType = card.GetComponent<CardInBoard>().type;
		Debug.Log("Card played: " + cardType + ", during " + curState);

		isInTransition = false;

		if ((cardType == CardType.Intelligence || cardType == CardType.SuperGenius) &&
			(curState == GameState.PlayerPlayPhase || curState == GameState.EnemyPlayPhase))
		{
			card.GetComponentInChildren<CardInBoard>().execute?.Invoke();

			if (curState == GameState.PlayerPlayPhase)
			{
               while (enemy.isWaiting) yield return new WaitForSeconds(.1f);
               enemy.PlayCardDraw();
			}
			else {
				if (enemy.getJustPlayed()){
					curState = GameState.PlayerPlayPhase;
				 }
			}
			
			yield break;
		}
		
		switch (curState)
		{
			case GameState.PlayerPlayPhase:

				if (!passButton.activeSelf)
					passButton.SetActive(true);

				playerCard = cardType;
				playerBoardCard = card;

                curState = GameState.EnemyPlayPhase;
                while (enemy.isWaiting) yield return new WaitForSeconds(.1f);
                enemy.PlayElement();

				break;

			case GameState.EnemyPlayPhase:
				texts[0].text = "";
				texts[1].text = "";
				texts[2].text = "";
				enemyCard = cardType;
				enemyBoardCard = card;
				curState = GameState.BattlePhase;
				StartCoroutine(Battle());
				break;

			case GameState.PlayerEffectPhase:

				if (card.GetComponentInChildren<CardInBoard>().execute != null)
				{
					card.GetComponentInChildren<CardInBoard>().execute();

					if (curWinCondition == WinCondition.Loss)
					{
						curState = GameState.PlayerEffectPhase;
					}
					else
					{
						if (curWinCondition == WinCondition.Victory)
						{
							curState = GameState.EnemyEffectPhase;

							while (enemy.isWaiting) yield return new WaitForSeconds(.1f);
							enemy.PlayPowerOrEffect();
						}
					}
				}
				else
				{
					curWinCondition = WinCondition.Victory;
					curState = GameState.EnemyEffectPhase;

					while (enemy.isWaiting) yield return new WaitForSeconds(.1f);
					enemy.PlayPowerOrEffect();
				}

				break;

			case GameState.EnemyEffectPhase:

				if (card.GetComponentInChildren<CardInBoard>().execute != null)
				{
					card.GetComponentInChildren<CardInBoard>().execute();

					if (curWinCondition == WinCondition.Victory)
					{
						curState = GameState.EnemyEffectPhase;

						yield return new WaitForSeconds(1f); //Espera 1 seg antes de jogar em seguida

                        while (enemy.isWaiting)
                            yield return new WaitForSeconds(.1f);

						enemy.PlayPowerOrEffect();
					}
					else
					{
						if (curWinCondition == WinCondition.Loss)
							curState = GameState.PlayerEffectPhase;
					}
				}
				else
				{
					curWinCondition = WinCondition.Loss;
					curState = GameState.PlayerEffectPhase;
				}

				break;
		}
	}

	public IEnumerator Battle()
	{
		if (curState != GameState.BattlePhase)
			yield break; ;

		switch (CheckWin(playerCard, enemyCard))
		{
			case WinCondition.Victory:

				curWinCondition = WinCondition.Victory;
				curState = GameState.EnemyEffectPhase;
				while (enemy.isWaiting) yield return new WaitForSeconds(.1f);
				enemy.PlayPowerOrEffect();
				//Debug.Log("Player played " + playerCard + " and won against " + enemyCard);
				break;

			case WinCondition.Loss:

				curWinCondition = WinCondition.Loss;
				curState = GameState.PlayerEffectPhase;
				//Debug.Log("Player played " + playerCard + " and lost against " + enemyCard);
				break;

			case WinCondition.Draw:

				curWinCondition = WinCondition.Draw;
				curState = GameState.PlayerEffectPhase;
				//Debug.Log("Both players played " + playerCard + " and it was a draw");
				break;
		}
	}

	public WinCondition CheckWin(CardType a, CardType b)
	{
		switch (a)
		{
			case CardType.WaterE:
				switch (b)
				{
					case CardType.WaterE:
						return WinCondition.Draw;
					case CardType.EarthE:
						return WinCondition.Loss;
					case CardType.FireE:
						return WinCondition.Victory;
					case CardType.AirE:
						return WinCondition.Victory;
					case CardType.LightningE:
						return WinCondition.Loss;
					case CardType.ArcanaE:
						return WinCondition.Loss;
				}

				return WinCondition.Victory;

			case CardType.EarthE:
				switch (b)
				{
					case CardType.WaterE:
						return WinCondition.Victory;
					case CardType.EarthE:
						return WinCondition.Draw;
					case CardType.FireE:
						return WinCondition.Victory;
					case CardType.AirE:
						return WinCondition.Loss;
					case CardType.LightningE:
						return WinCondition.Loss;
					case CardType.ArcanaE:
						return WinCondition.Loss;
				}

				return WinCondition.Victory;

			case CardType.FireE:
				switch (b)
				{
					case CardType.WaterE:
						return WinCondition.Loss;
					case CardType.EarthE:
						return WinCondition.Loss;
					case CardType.FireE:
						return WinCondition.Draw;
					case CardType.AirE:
						return WinCondition.Victory;
					case CardType.LightningE:
						return WinCondition.Victory;
					case CardType.ArcanaE:
						return WinCondition.Loss;
				}

				return WinCondition.Victory;

			case CardType.AirE:
				switch (b)
				{
					case CardType.WaterE:
						return WinCondition.Loss;
					case CardType.EarthE:
						return WinCondition.Victory;
					case CardType.FireE:
						return WinCondition.Loss;
					case CardType.AirE:
						return WinCondition.Draw;
					case CardType.LightningE:
						return WinCondition.Victory;
					case CardType.ArcanaE:
						return WinCondition.Loss;
				}

				return WinCondition.Victory;

			case CardType.LightningE:
				switch (b)
				{
					case CardType.WaterE:
						return WinCondition.Victory;
					case CardType.EarthE:
						return WinCondition.Victory;
					case CardType.FireE:
						return WinCondition.Loss;
					case CardType.AirE:
						return WinCondition.Loss;
					case CardType.LightningE:
						return WinCondition.Draw;
					case CardType.ArcanaE:
						return WinCondition.Loss;
				}

				return WinCondition.Victory;

			case CardType.ArcanaE:
				switch (b)
				{
					case CardType.WaterE:
						return WinCondition.Victory;
					case CardType.EarthE:
						return WinCondition.Victory;
					case CardType.FireE:
						return WinCondition.Victory;
					case CardType.AirE:
						return WinCondition.Victory;
					case CardType.LightningE:
						return WinCondition.Victory;
					case CardType.ArcanaE:
						return WinCondition.Draw;
				}

				return WinCondition.Victory;
		}

		return WinCondition.Victory;
	}

	public void EndRound()
	{
		if (curState == GameState.EndGame)
			return;

		curState = GameState.ClearPhase;
		VictoryDeck vp = GameObject.FindWithTag("VictoryDeck/Player").GetComponent<VictoryDeck>();
		VictoryDeck ve = GameObject.FindWithTag("VictoryDeck/Enemy").GetComponent<VictoryDeck>();

		if (curWinCondition == WinCondition.Victory)
		{
			texts[0].text = "Player Won This Round";
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Card/Element"))
			{
				go.GetComponent<CardInBoard>().Activate(SlotsOnBoard.VictoryDeckPlayer);
				vp.AddCard(go.GetComponent<CardInBoard>().type);
			}
		}
		if (curWinCondition == WinCondition.Loss)
		{
			texts[0].text = "Enemy Won This Round";
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Card/Element"))
			{
				go.GetComponent<CardInBoard>().Activate(SlotsOnBoard.VictoryDeckEnemy);
				ve.AddCard(go.GetComponent<CardInBoard>().type);
			}

		}
		if (curWinCondition == WinCondition.Draw)
			texts[0].text = "Draw This Round";

		Discard d = GameObject.FindWithTag("Discard").GetComponent<Discard>();
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Card/Power"))
		{
			d.DiscardCard(go.GetComponent<CardInBoard>().type);
			go.GetComponent<CardInBoard>().Activate(SlotsOnBoard.Discard);
		}

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Card/Effect"))
		{
			d.DiscardCard(go.GetComponent<CardInBoard>().type);
			go.GetComponent<CardInBoard>().Activate(SlotsOnBoard.Discard);
		}

		d.Shuffle();
		d.Shuffle();

		curWinCondition = WinCondition.Draw;
		playerCard = CardType.None;
		enemyCard = CardType.None;
		DrawCards();
	}

	public void EndGame()
	{
		VictoryDeck vp = GameObject.FindWithTag("VictoryDeck/Player").GetComponent<VictoryDeck>();
		VictoryDeck ve = GameObject.FindWithTag("VictoryDeck/Enemy").GetComponent<VictoryDeck>();
		int pScore = vp.GetScore();
		int eScore = ve.GetScore();

		//texts[0].fontSize = 40;
		texts[1].text = "";
		texts[2].text = "";

		if (pScore > eScore)
		{
			texts[0].text = "End of Game\nPlayer won: " + pScore + " x " + eScore;
			texts[0].color = new Color(0f, 1f, 0f);
		}

		if (pScore < eScore)
		{
			texts[0].text = "End of Game\nEnemy won: " + eScore + " x " + pScore;
			texts[0].color = new Color(1f, 0f, 0f);
		}

		if (pScore == eScore)
		{
			texts[0].text = "End of Game\nDraw: " + pScore + " x " + eScore;
			texts[0].color = new Color(1f, 1f, 1f);
		}
	}

	public void setButtonEnemyPlayer(bool activate){
		playerButton.SetActive (activate);
		enemyButton.SetActive (activate);
		GetComponent<AnimationManager>().Fade(activate);
        HideElementsFromAnimation();
    }

	public void setButtonEnemyPlayerPartial(bool playerB, bool enemyB, bool animF)
	{
		playerButton.SetActive(playerB);
		enemyButton.SetActive(enemyB);
		GetComponent<AnimationManager>().Fade(animF);
        HideElementsFromAnimation();
    }

	public void HidePassButton(bool hide){
		//passButton.SetActive (!hide);
		GetComponent<AnimationManager>().FadePartial(hide, hide, false);
        HideElementsFromAnimation();
    }

    public void HideElementsFromAnimation()
    {
        playerBoardCard.GetComponentInChildren<CardInBoard>().HiddenFromAnimation();
        enemyBoardCard.GetComponentInChildren<CardInBoard>().HiddenFromAnimation();
    }

	void Update()
	{
		//Peek();
	}

}
