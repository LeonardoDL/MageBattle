using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public delegate void DifficultyDecision();

public class EnemyManager : MonoBehaviour
{
	[HideInInspector] public List<CardType> hand;
	[HideInInspector] public List<CardType> standBy;
	[HideInInspector] public bool Ipass;

	public Transform enemySlot;
	public Vector3 offset;
	//public bool isActive;
	[HideInInspector] public CardType cardToPlay = CardType.None;
	[HideInInspector] public bool isWaiting;
	[HideInInspector] public Difficulty difficulty;

	private BoardManager bm;
	private DifficultyDecision PowerOrEffect;
	private Deck deck;
	Dictionary<CardType, Power> powers;
	Dictionary<CardType, Effect> effects;

	// Armazena o estado para saber se acabou de jogar uma carta de inteligencia/genio como resposta ao player
	private bool justPlayed = false;

	// Start is called before the first frame update
	void Awake()
	{
		hand = new List<CardType>();
		standBy = new List<CardType>();
		deck = GameObject.FindWithTag("Deck").GetComponent<Deck>();

		powers = new Dictionary<CardType, Power>();
		effects = new Dictionary<CardType, Effect>();

		for (int i = 7; i < 33; i++)
		{
			Power p = gameObject.AddComponent<Power>();
			p.Init((CardType)i);
			powers.Add((CardType)i, p);
		}

		for (int i = 33; i < CardType.GetNames(typeof(CardType)).Length; i++)
		{
			Effect e = gameObject.AddComponent<Effect>();
			e.Init((CardType)i);
			effects.Add((CardType)i, e);
		}
	}

	void Start()
	{
		enemySlot = GameObject.FindWithTag("Slot/ElementEnemy").transform;
		bool tutorial = Options.GetBool("tutorial");
		if (!tutorial)
			difficulty = (Difficulty) PlayerPrefs.GetInt("difficulty", 0);
		else
			difficulty = Difficulty.Medium;

		switch (difficulty)
		{
			case Difficulty.Easy:
				PowerOrEffect = PowerOrEffectEasy;
				break;

			case Difficulty.Medium:
				PowerOrEffect = PowerOrEffectMedium;
				break;

			default:
				PowerOrEffect = PowerOrEffectMedium;
				break;
		}

		bm = BoardManager.GetBoardManager();
		//Debug.Log("tutorial is " + tutorial + " and diff is " + difficulty);
	}

	public void DrawHandEnemy(int quantity)
	{
		if (quantity < 0) quantity = 0;

		BoardManager.GetBoardManager().texts[2].text = "Enemy draws " + quantity + " cards";

		for (int i = 0; i < quantity; i++)
		{
			CardType c = deck.DrawCardEnemy();
			if (c != CardType.None)
				hand.Add(c);
		}

		PassElementsToStandBy();
	}

	public void PassElementsToStandBy()
	{
		List<CardType> list_ = new List<CardType>();
		foreach (CardType card in hand)
		{
			if (card == CardType.AirE || card == CardType.ArcanaE || card == CardType.EarthE ||
				card == CardType.FireE || card == CardType.LightningE || card == CardType.WaterE)
			{
				standBy.Add(card);
				list_.Add(card);
			}
		}
		foreach (CardType card in list_)
			hand.Remove(card);
	}
	
	public void DiscardHand(){
		BoardManager bm = BoardManager.GetBoardManager();
		foreach(CardType card in hand){
			deck.cardBuilder.RemoveCardFromHand(card);
			bm.discard.DiscardCard(card);
		}
		hand = new List<CardType>();
		bm.texts[6].text = "" + bm.discard.Size();

	}
	
	public void ClearStandBy()
	{
		BoardManager bm = BoardManager.GetBoardManager();
		foreach(CardType card in standBy)
			deck.cardBuilder.RemoveCardFromStandBy(card);
			
		standBy = new List<CardType>();
	}

	public void AddCardToHand(CardType card){  
		hand.Add(card);
		CardBuilder cardBuilder = deck.GetCardBuilder();
		cardBuilder.BuildCard(card, false);
	}

	public void AddCardForEnemy(CardType card)
	{
		if (card == CardType.None)
			return;

		if ((int)card > 0 && (int)card <= 6)
			standBy.Add(card);
		else
			hand.Add(card);

		CardBuilder cardBuilder = deck.GetCardBuilder();
		cardBuilder.BuildCard(card, false);
	}
	
	public CardType GetCardFromHand(CardType c)
	{  
		foreach (CardType f in hand)
			if (f == c)
			{
				hand.Remove(f);
				deck.cardBuilder.RemoveCardFromHand(f);
				return f;
			}

		return CardType.None;
	}

	public CardType RemoveCard(CardType c)
	{
		foreach (CardType f in hand)
			if (f == c)
			{
				hand.Remove(f);
				deck.cardBuilder.RemoveCardFromHand(f);
				return f;
			}

		foreach (CardType e in standBy)
			if (e == c)
			{
				standBy.Remove(e);
				deck.cardBuilder.RemoveCardFromStandBy(e);
				return CardType.None;
			}

		return CardType.None;
	}

	public List<CardType> GetStandby() {
		List<CardType> standByOld = standBy;
		BoardManager bm = BoardManager.GetBoardManager();
		foreach(CardType card in standBy){
			deck.cardBuilder.RemoveCardFromStandBy(card);
		}
		standBy = new List<CardType>();

		return standByOld;
	}

	public int GetStandbyCount()
	{
		return standBy.Count;
	}

	public void DrawHandEnemyFromDiscard(int quantity)
	{
		BoardManager.GetBoardManager().texts[2].text = "Enemy draws " + quantity + " cards";

		for (int i = 0; i < quantity; i++)
		{
			CardType c = BoardManager.GetBoardManager().discard.DrawCardEnemy();
			if (c != CardType.None)
				hand.Add(c);
		}
	}

	public void PlayElement()
	{
		CardType cardType = standBy[UnityEngine.Random.Range(0, standBy.Count)];

		if (standBy.Contains(cardToPlay))
			cardType = cardToPlay;

		standBy.Remove(cardType);
		Tuple<Sprite, GameObject> item = deck.cardBuilder.GetSpriteGameObject(cardType);

		GameObject g = Instantiate(item.Item2, enemySlot.position + offset, Quaternion.identity);
		g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.Item1;
		g.GetComponent<CardInBoard>().type = cardType;
		g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.ElementEnemy);

		deck.cardBuilder.RemoveCardFromStandBy(cardType);
	}

	public bool getJustPlayed()
    {
		bool temp = justPlayed;
		justPlayed = false;
		return temp;
	}

	public void PlayCardDraw()
	{
		if (HasPlayableCardDraw())
		{
			CardType c = CardType.None;

			if (hand.Contains(CardType.SuperGenius))
				c = CardType.SuperGenius;

			if (hand.Contains(CardType.Intelligence))
				c = CardType.Intelligence;

			if (effects[c].isPlayable())
			{
				justPlayed = true;
				PlayEffect(c);
			}
		}
	}

	public bool HasPlayableCardDraw()
	{
		CardType c = CardType.None;

		if (hand.Contains(CardType.SuperGenius) && effects[CardType.SuperGenius].isPlayable())
			c = CardType.SuperGenius;

		if (hand.Contains(CardType.Intelligence) && effects[CardType.Intelligence].isPlayable())
			c = CardType.Intelligence;

		if (c != CardType.None)
			return true;

		return false;
	}

	public void PlayPowerOrEffect()
	{
		Ipass = false;
		//Debug.Log("PlayPowerOrEffect");

		if (hand.Contains(cardToPlay))
		{
			if ((int)cardToPlay > 32)
				PlayEffect(cardToPlay);
			else
				PlayPower(cardToPlay);

			return;
		}

		PowerOrEffect();
	}

	public void PowerOrEffectEasy()
	{
		if (!HasPlayablePowerEasy() && !HasPlayableEffectEasy())
		{
			BoardManager.curState = GameState.PlayerEffectPhase;
			Ipass = true;
		}
	}

	public void PowerOrEffectMedium()
	{
		if (Random.Range(0, 10) < 5)
		{
			if (!HasPlayablePowerMedium() && !HasPlayableEffectMedium())
			{
				BoardManager.curState = GameState.PlayerResponsePhase;
				Ipass = true;
			}
		}
		else
		{
			if (!HasPlayableEffectMedium() && !HasPlayablePowerMedium())
			{
				BoardManager.curState = GameState.PlayerResponsePhase;
				Ipass = true;
			}
		}
	}

	public bool HasPlayablePowerEasy()
	{
		for (int i = 7; i < 33; i++) //Power
		{
			if (hand.Contains((CardType)i))
			{
				switch (BoardManager.GetBoardManager().enemyCard)
				{
					case CardType.WaterE:
						if (powers[(CardType)i].water)
						{
							PlayPower((CardType)i);
							return true;
						}
						break;

					case CardType.EarthE:
						if (powers[(CardType)i].earth)
						{
							PlayPower((CardType)i);
							return true;
						}
						break;

					case CardType.FireE:
						if (powers[(CardType)i].fire)
						{
							PlayPower((CardType)i);
							return true;
						}
						break;

					case CardType.AirE:
						if (powers[(CardType)i].air)
						{
							PlayPower((CardType)i);
							return true;
						}
						break;

					case CardType.LightningE:
						if (powers[(CardType)i].lightning)
						{
							PlayPower((CardType)i);
							return true;
						}
						break;

					case CardType.ArcanaE:

						PlayPower((CardType)i);
						return true;
				}
			}
		}

		//Debug.Log("No playable power");
		return false;
	}

	public bool HasPlayablePowerMedium()
	{
		if (IsCardOnResponseStack("Power") || IsCardOnResponseStack(CardType.Portal))
			return false;
		return HasPlayablePowerEasy();
	}

	public void PlayPower(CardType cardType)
	{
		hand.Remove(cardType);
		Tuple<Sprite, GameObject> item = deck.cardBuilder.GetSpriteGameObject(cardType);

		GameObject g = Instantiate(item.Item2, enemySlot.position + offset, Quaternion.identity);
		g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.Item1;
		g.GetComponent<CardInBoard>().type = cardType;
		g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.Stack);

		deck.cardBuilder.RemoveCardFromHand(cardType);
	}

	public bool HasPlayableEffectEasy()
	{
		Debug.Log("Easy");
		DeckList<CardType> dc = new DeckList<CardType>();

		foreach (CardType c in hand)
			dc.Push(c);

		dc.Shuffle();

		foreach (CardType c in dc) //Effect
			if (hand.Contains((CardType)c) && (int)c >= 33)
			{
				Debug.Log("I have " + (CardType)c);

				if (effects[(CardType)c].isPlayable())
				{
					PlayEffect((CardType)c);
					return true;
				}
			}

		Debug.Log("No playable Effect");
		return false;
	}
	
	public bool HasPlayableEffectMedium()
	{
		Debug.Log("Medium");
		for (int i = 33; i < CardType.GetNames(typeof(CardType)).Length; i++) //Effect
			if (hand.Contains((CardType)i))
			{
				//Debug.Log("I have " + (CardType)i);

				if (effects[(CardType)i].isPlayable())
				{
					if (IsCardOnResponseStack((CardType)i, true))
						return false;

					PlayEffect((CardType)i);
					return true;
				}
			}

		//Debug.Log("No playable Effect");
		return false;
	}

	public void PlayEffect(CardType cardType)
	{
		hand.Remove(cardType);
		Tuple<Sprite, GameObject> item = deck.cardBuilder.GetSpriteGameObject(cardType);

		GameObject g = Instantiate(item.Item2, enemySlot.position + offset, Quaternion.identity);
		g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.Item1;
		g.GetComponent<CardInBoard>().execute = effects[cardType].execute;
		g.GetComponent<CardInBoard>().UI = effects[cardType].UI;
		g.GetComponent<CardInBoard>().type = cardType;
		g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.Stack);

		deck.cardBuilder.RemoveCardFromHand(cardType);
	}

	public void PlayResponse()
	{
		if (bm == null) bm = BoardManager.GetBoardManager();

		PlayPowerOrEffect();
		if (Ipass)
		{
			BoardManager.curState = GameState.PlayerResolutionPhase;
			bm.Resolve();
		}
	}

	public bool IsCardOnResponseStack(CardType c)
	{
		GameObject[] cards = bm.responseStack.ToArray();
		for (int i = 0; i < cards.Length; i++)
			if (cards[i].GetComponent<CardInBoard>().type == c)
				return true;
		return false;
	}

    public bool IsCardOnResponseStack(CardType c, bool own)
    {
        GameObject[] cards = bm.responseStack.ToArray();
        int i;

        if (own) i = (cards.Length % 2 == 0) ? 0 : 1; //If I have to look for my (enemy) card
        else     i = (cards.Length % 2 == 0) ? 1 : 0; //If I have to look for the player card

        for (; i < cards.Length; i += 2)
            if (cards[i].GetComponent<CardInBoard>().type == c)
                return true;
        return false;
    }

    public bool IsCardOnResponseStack(string s)
	{
		GameObject[] cards = bm.responseStack.ToArray();
		if (s == "Power")
		{
			for (int i = 0; i < cards.Length; i++)
				if (cards[i].GetComponent<CardInBoard>().execute == null)
					return true;
		}

		if (s == "Effect")
		{
			for (int i = 0; i < cards.Length; i++)
				if (cards[i].GetComponent<CardInBoard>().execute != null)
					return true;
		}

		return false;
	}
}
