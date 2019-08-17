using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBuilder : MonoBehaviour
{
	[Header("Player References")]
	public GameObject panelHand;
	public GameObject panelStandBy;

	[Header("Player Prefabs")]
	public GameObject cardPlaceholder;
	public GameObject cardInHand;
	public GameObject cardInStandBy;
	public GameObject[] cardPrefabs;
	public GameObject powerPrefab;
	public GameObject effectPrefab;

	[Header("Enemy References")]
	public GameObject panelHandEnemy;
	public GameObject panelStandByEnemy;

	[Header("Enemy Prefabs")]
	public GameObject cardPlaceholderEnemy;
	public GameObject cardInHandEnemy;
	public GameObject cardInStandByEnemy;
	public GameObject[] cardPrefabsEnemy;
	public GameObject powerPrefabEnemy;
	public GameObject effectPrefabEnemy;

	public Sprite cardBack;
	public Sprite[] elSprites;
	public Sprite[] pwSprites;
	public Sprite[] efSprites;

	private Dictionary<CardType, Tuple<Sprite, GameObject>> elementsDictionary;
	private Dictionary<CardType, Tuple<Sprite, GameObject>> powersDictionary;
	private Dictionary<CardType, Tuple<Sprite, GameObject>> effectsDictionary;

	private Dictionary<CardType, Tuple<Sprite, GameObject>> elementsDictionaryEnemy;
	private Dictionary<CardType, Tuple<Sprite, GameObject>> powersDictionaryEnemy;
	private Dictionary<CardType, Tuple<Sprite, GameObject>> effectsDictionaryEnemy;
	
	
	[HideInInspector] public bool reveal = false;


	// Start is called before the first frame update
	void Start()
	{
		elementsDictionary = new Dictionary<CardType, Tuple<Sprite, GameObject>>();
		powersDictionary = new Dictionary<CardType, Tuple<Sprite, GameObject>>();
		effectsDictionary = new Dictionary<CardType, Tuple<Sprite, GameObject>>();

		for (int i = 1, j = 0; i < 7; i++, j++)
			elementsDictionary.Add((CardType) i, new Tuple<Sprite, GameObject>(elSprites[j], cardPrefabs[j]));

		for (int i = 7, j = 0; i < 33; i++, j++)
			powersDictionary.Add((CardType) i, new Tuple<Sprite, GameObject>(pwSprites[j], powerPrefab));

		for (int i = 33, j = 0; i < CardType.GetNames(typeof(CardType)).Length; i++, j++)
			effectsDictionary.Add((CardType) i, new Tuple<Sprite, GameObject>(efSprites[j], effectPrefab));



		elementsDictionaryEnemy = new Dictionary<CardType, Tuple<Sprite, GameObject>>();
		powersDictionaryEnemy = new Dictionary<CardType, Tuple<Sprite, GameObject>>();
		effectsDictionaryEnemy = new Dictionary<CardType, Tuple<Sprite, GameObject>>();

		for (int i = 1, j = 0; i < 7; i++, j++)
			elementsDictionaryEnemy.Add((CardType)i, new Tuple<Sprite, GameObject>(elSprites[j], cardPrefabsEnemy[j]));

		for (int i = 7, j = 0; i < 33; i++, j++)
			powersDictionaryEnemy.Add((CardType)i, new Tuple<Sprite, GameObject>(pwSprites[j], powerPrefabEnemy));

		for (int i = 33, j = 0; i < CardType.GetNames(typeof(CardType)).Length; i++, j++)
			effectsDictionaryEnemy.Add((CardType)i, new Tuple<Sprite, GameObject>(efSprites[j], effectPrefabEnemy));
	}

	public void BuildCard(CardType c, bool isPlayer)
	{
        if (isPlayer)
		{
			try
			{
				Tuple<Sprite, GameObject> element = elementsDictionary[c];
				GameObject cis = Instantiate(cardInStandBy, Instantiate(cardPlaceholder, panelStandBy.transform).transform);
				cis.GetComponent<CardInStandBy>().SetSpriteAndPrefab(element.Item1, element.Item2);
				cis.GetComponent<CardInStandBy>().SetType(c);
				return;
			}
			catch { }

			try
			{
				Tuple<Sprite, GameObject> power = powersDictionary[c];
				GameObject cih = Instantiate(cardInHand, Instantiate(cardPlaceholder, panelHand.transform).transform);
				cih.GetComponent<CardInHand>().SetSpriteAndPrefab(power.Item1, power.Item2);
				cih.AddComponent<Power>().Init(c);
				return;
			}
			catch { }

			try
			{
				Tuple<Sprite, GameObject> effect = effectsDictionary[c];
				GameObject cih = Instantiate(cardInHand, Instantiate(cardPlaceholder, panelHand.transform).transform);
				cih.GetComponent<CardInHand>().SetSpriteAndPrefab(effect.Item1, effect.Item2);
				cih.AddComponent<Effect>().Init(c);
				return;
			}
			catch { Debug.Log("Ai fudeu " + c + " " + isPlayer); }
		}
		else
		{
			try
			{
				Tuple<Sprite, GameObject> element = elementsDictionaryEnemy[c];
				GameObject cis = Instantiate(cardInStandByEnemy, Instantiate(cardPlaceholderEnemy, panelStandByEnemy.transform).transform);
				
				if (reveal)
				{
					cis.GetComponent<Image>().sprite = element.Item1;
					cis.GetComponent<SpriteHandler>().cardFront = cardBack;
					cis.GetComponent<SpriteHandler>().card = c;
					return;
				}
				
				cis.GetComponent<Image>().sprite = cardBack;
				cis.GetComponent<SpriteHandler>().cardFront = element.Item1;
				cis.GetComponent<SpriteHandler>().card = c;
				return;
			}
			catch { }

			try
			{
				Tuple<Sprite, GameObject> power = powersDictionaryEnemy[c];
				GameObject cih = Instantiate(cardInHandEnemy, Instantiate(cardPlaceholderEnemy, panelHandEnemy.transform).transform);
				
				if (reveal)
				{
					cih.GetComponent<Image>().sprite = power.Item1;
					cih.GetComponent<SpriteHandler>().cardFront = cardBack;
					cih.GetComponent<SpriteHandler>().card = c;
					return;
				}
				
				cih.GetComponent<Image>().sprite = cardBack;
                if (AnimationManager.faded == true)
                {
                    cih.GetComponent<Image>().enabled = false;
                    cih.GetComponent<Animator>().SetBool("Focus", true);
                }
                Debug.Log("This is inside card builder " + AnimationManager.faded);
                cih.GetComponent<SpriteHandler>().cardFront = power.Item1;
				cih.GetComponent<SpriteHandler>().card = c;
				return;
			}
			catch { }

			try
			{
				Tuple<Sprite, GameObject> effect = effectsDictionaryEnemy[c];
				GameObject cih = Instantiate(cardInHandEnemy, Instantiate(cardPlaceholderEnemy, panelHandEnemy.transform).transform);
				
				if (reveal)
				{
					cih.GetComponent<Image>().sprite = effect.Item1;
					cih.GetComponent<SpriteHandler>().cardFront = cardBack;
					cih.GetComponent<SpriteHandler>().card = c;
					return;
				}
				
				cih.GetComponent<Image>().sprite = cardBack;
                if (AnimationManager.faded == true)
                {
                    cih.GetComponent<Image>().enabled = false;
                    cih.GetComponent<Animator>().SetBool("Focus", true);
                }
                
                cih.GetComponent<SpriteHandler>().cardFront = effect.Item1;
				cih.GetComponent<SpriteHandler>().card = c;
				return;
			}
			catch { Debug.Log("Ai fudeu " + c + " " + isPlayer); }
		}
	}

	public Tuple<Sprite,GameObject> GetSpriteGameObject(CardType c)
	{
		try
		{
			return elementsDictionaryEnemy[c];
		}
		catch { }

		try
		{
			return powersDictionaryEnemy[c];
		}
		catch { }

		try
		{
			return effectsDictionaryEnemy[c];
		}
		catch { Debug.Log("Ai fudeu"); }

		return null;
	}

	public void RemoveCardFromHand(CardType c)
	{
		foreach (SpriteHandler s in panelHandEnemy.GetComponentsInChildren<SpriteHandler>())
			if (s.card == c)
			{
				DestroyImmediate(s.transform.parent.gameObject);
				return;
			}
			
		// if (panelHandEnemy.transform.childCount > 0)
		// {
			// DestroyImmediate(panelHandEnemy.transform.GetChild(0).gameObject);
		// }
	}

	public void RemoveCardFromStandBy(CardType c)
	{
		foreach (SpriteHandler s in panelStandByEnemy.GetComponentsInChildren<SpriteHandler>())
			if (s.card == c)
			{
				DestroyImmediate(s.transform.parent.gameObject);
				return;
			}
			
		// if (panelStandByEnemy.transform.childCount > 0)
		// {
			// DestroyImmediate(panelStandByEnemy.transform.GetChild(0).gameObject);
		// }
	}
	
	public void SwapSpritesEnemyHand()
	{
		foreach (SpriteHandler s in panelHandEnemy.GetComponentsInChildren<SpriteHandler>())
			s.Swap();
	}
	
	public void SwapSpritesEnemyStandBy()
	{
		foreach (SpriteHandler s in panelStandByEnemy.GetComponentsInChildren<SpriteHandler>())
			s.Swap();
	}
}
