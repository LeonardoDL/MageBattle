using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBuilder : MonoBehaviour
{
    public GameObject cardPlaceholder;
    public GameObject cardInHand;
    public GameObject cardInStandBy;
    public GameObject panelHand;
    public GameObject panelStandBy;

    public Sprite[] elSprites;
    public Sprite[] pwSprites;
    public Sprite[] efSprites;

    public GameObject[] cardPrefabs;
    public GameObject powerPrefab;

    public GameObject effectPrefab;

    private Dictionary<CardType, Tuple<Sprite, GameObject>> elementsDictionary;
    private Dictionary<CardType, Tuple<Sprite, GameObject>> powersDictionary;
    private Dictionary<CardType, Tuple<Sprite, GameObject>> effectsDictionary;


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
    }

    public void BuildCard(CardType c)
    {
        try
        {
            Tuple<Sprite, GameObject> element = elementsDictionary[c];
            GameObject cis = Instantiate(cardInStandBy, Instantiate(cardPlaceholder, panelStandBy.transform).transform);
            cis.GetComponent<CardInStandBy>().SetSpriteAndPrefab(element.Item1, element.Item2);
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
        catch { Debug.Log("Ai fudeu"); }
    }
}
