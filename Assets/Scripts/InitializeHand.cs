using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeHand : MonoBehaviour
{
    public int quantity = 15;
    public GameObject cardPlaceholder;
    public GameObject cardInHand;
    public GameObject cardInStandBy;
    public GameObject panelStandBy;

    public Sprite[] elSprites;
    public Sprite[] pwSprites;
    public GameObject[] cardPrefabs;
    public GameObject powerPrefab;

    void Start()
    {
        for (int i = 0; i < quantity; i++)
        {
            int x = Random.Range(0, elSprites.Length + pwSprites.Length);
            if (x < elSprites.Length)
                NewCardInStandBy(x);
            else
                NewCardInHand(x - elSprites.Length);
        }
    }
    
    public void NewCardInHand(int x)
    {
        //GameObject ph = Instantiate(cardPlaceholder, transform);
        GameObject cih = Instantiate(cardInHand, Instantiate(cardPlaceholder, transform).transform);
        //powerPrefab.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = pwSprites[x];
        cih.GetComponent<CardInHand>().SetSpriteAndPrefab(pwSprites[x], powerPrefab);
        cih.tag = "Card/Power";
        Power p = cih.AddComponent<Power>();
        switch (x)
        {
            case 0:
                p.water = true;
                break;
            case 1:
                p.fire = true;
                break;
            case 2:
                p.air = true;
                break;
            case 3:
                p.lightning = true;
                break;
            case 4:
                p.earth = true;
                break;
            case 5:
                p.water = true;
                p.fire = true;
                break;
            case 6:
                p.water = true;
                p.air = true;
                break;
            case 7:
                p.water = true;
                p.lightning = true;
                break;
            case 8:
                p.water = true;
                p.earth = true;
                break;
            case 9:
                p.fire = true;
                p.air = true;
                break;
            case 10:
                p.fire = true;
                p.lightning = true;
                break;
            case 11:
                p.earth = true;
                p.fire = true;
                break;
            case 12:
                p.lightning = true;
                p.air = true;
                break;
            case 13:
                p.air = true;
                p.earth = true;
                break;
            case 14:
                p.lightning = true;
                p.earth = true;
                break;
        }
    }

    public void NewCardInStandBy(int x)
    {
        GameObject cis = Instantiate(cardInStandBy, Instantiate(cardPlaceholder, panelStandBy.transform).transform);
        cis.GetComponent<CardInStandBy>().SetSpriteAndPrefab(elSprites[x], cardPrefabs[x]);
    }
}
