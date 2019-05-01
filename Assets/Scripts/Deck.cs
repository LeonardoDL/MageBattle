using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public int deckSize = 20;
    public DeckList<CardType> cards;

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


    // Start is called before the first frame update
    void Start()
    {
        cards = new DeckList<CardType>();
        PopulatePreDetermined();
        //DebugAllDeck();
    }

    public void PopulateRandom()
    {
        for (int i = 0; i < deckSize; i++)
            cards.Push((CardType)Random.Range(1, CardType.GetNames(typeof(CardType)).Length));
    }

    public void PopulateOneOfEach()
    {
        for (int i = 1; i < CardType.GetNames(typeof(CardType)).Length; i++)
            cards.Push((CardType)i);

        cards.Shuffle();
    }

    public void PopulatePreDetermined()
    {
        cards.Push(CardType.AirE, 6);
        cards.Push(CardType.EarthE, 6);
        cards.Push(CardType.FireE, 50);
        cards.Push(CardType.LightningE, 6);
        cards.Push(CardType.WaterE, 6);
        cards.Push(CardType.ArcanaE, 6);

        // Cartas de poderes unicos
        cards.Push(CardType.AirP, 1);
        cards.Push(CardType.EarthP, 1);
        cards.Push(CardType.FireP, 1);
        cards.Push(CardType.LightningP, 1);
        cards.Push(CardType.WaterP, 1);

        // Cartas de poderes duplos
        cards.Push(CardType.WaterFireP, 1);
        cards.Push(CardType.WaterAirP, 1);
        cards.Push(CardType.WaterLightningP, 1);
        cards.Push(CardType.WaterEarthP, 1);
        cards.Push(CardType.FireAirP, 1);
        cards.Push(CardType.FireLightningP, 1);
        cards.Push(CardType.FireEarthP, 1);
        cards.Push(CardType.AirLightningP, 1);
        cards.Push(CardType.AirEarthP, 1);
        cards.Push(CardType.EarthLightningP, 1);

        // Cartas de poderes triplos
        cards.Push(CardType.WaterFireAirP, 1);
        cards.Push(CardType.LightningFireWaterP, 1);
        cards.Push(CardType.WaterFireEarthP, 1);
        cards.Push(CardType.WaterAirLightningP, 1);
        cards.Push(CardType.WaterAirEarthP, 1);
        cards.Push(CardType.LightningWaterEarthP, 1);
        cards.Push(CardType.LightningFireAirP, 1);
        cards.Push(CardType.FireAirEarthP, 1);
        cards.Push(CardType.LightningFireEarthP, 1);
        cards.Push(CardType.LightningAirEarthP, 1);

        cards.Push(CardType.MegaPowerP, 5);

        cards.Push(CardType.Intelligence, 50);






        cards.Shuffle();
    }

    public void DebugAllDeck()
    {
        foreach (CardType c in cards)
            Debug.Log(c.ToString());
    }

    public void DrawCardPlayer()
    {
        CardType c = CardType.None;
        try
        {
            c = cards.Draw();
        }
        catch (System.InvalidOperationException e)
        {
            Debug.Log("No cards! [Deck]");
            BoardManager.GetBoardManager().endGame = true;
            //GameObject.FindWithTag("Discard").GetComponent<Discard>().DrawCardPlayer();
        }


        Power powerCard;
        Effect effectCard;
         
        switch (c)
        {
        
            case CardType.WaterE:
                NewCardInStandBy(0);
                break;
            case CardType.EarthE:
                NewCardInStandBy(1);
                break;
            case CardType.FireE:
                NewCardInStandBy(2);
                break;
            case CardType.AirE:
                NewCardInStandBy(3);
                break;
            case CardType.LightningE:
                NewCardInStandBy(4);
                break;
            case CardType.ArcanaE:
                NewCardInStandBy(5);
                break;
            case CardType.WaterP:
                powerCard = NewPowerInHand(0);
                powerCard.water = true;
                powerCard.cardType = CardType.WaterP;
                break;
            case CardType.EarthP:
                powerCard = NewPowerInHand(1);
                powerCard.earth = true;
                powerCard.cardType = CardType.EarthP;
                break;
            case CardType.FireP:
                powerCard = NewPowerInHand(2);
                powerCard.fire = true;
                powerCard.cardType = CardType.FireP;
                break;
            case CardType.AirP:
                powerCard = NewPowerInHand(3);
                powerCard.air = true;
                powerCard.cardType = CardType.AirP;
                break;
            case CardType.LightningP:
                powerCard = NewPowerInHand(4);
                powerCard.lightning = true;
                powerCard.cardType = CardType.LightningP;
                break;
            case CardType.WaterFireP:
                powerCard = NewPowerInHand(5);
                powerCard.water = true;
                powerCard.fire = true;
                powerCard.cardType = CardType.WaterFireP;
                break;
            case CardType.WaterAirP:
                powerCard = NewPowerInHand(6);
                powerCard.water = true;
                powerCard.air = true;
                powerCard.cardType = CardType.WaterAirP;
                break;
            case CardType.WaterLightningP:
                powerCard = NewPowerInHand(7);
                powerCard.water = true;
                powerCard.lightning = true;
                powerCard.cardType = CardType.WaterLightningP;
                break;
            case CardType.WaterEarthP:
                powerCard = NewPowerInHand(8);
                powerCard.water = true;
                powerCard.earth = true;
                powerCard.cardType = CardType.WaterEarthP;
                break;
            case CardType.FireAirP:
                powerCard = NewPowerInHand(9);
                powerCard.air = true;
                powerCard.fire = true;
                powerCard.cardType = CardType.FireAirP;
                break;
            case CardType.FireLightningP:
                powerCard = NewPowerInHand(10);
                powerCard.lightning = true;
                powerCard.fire = true;
                powerCard.cardType = CardType.FireLightningP;
                break;
            case CardType.FireEarthP:
                powerCard = NewPowerInHand(11);
                powerCard.earth = true;
                powerCard.fire = true;
                powerCard.cardType = CardType.FireEarthP;
                break;
            case CardType.AirLightningP:
                powerCard = NewPowerInHand(12);
                powerCard.air = true;
                powerCard.lightning = true;
                powerCard.cardType = CardType.AirLightningP;
                break;
            case CardType.AirEarthP:
                powerCard = NewPowerInHand(13);
                powerCard.air = true;
                powerCard.earth = true;
                powerCard.cardType = CardType.AirEarthP;
                break;
            case CardType.EarthLightningP:
                powerCard = NewPowerInHand(14);
                powerCard.earth = true;
                powerCard.lightning = true;
                powerCard.cardType = CardType.EarthLightningP;
                break;
            case CardType.WaterFireAirP:
                powerCard = NewPowerInHand(15);
                powerCard.water = true;
                powerCard.fire = true;
                powerCard.air = true;
                powerCard.cardType = CardType.WaterFireAirP;
                break;
            case CardType.LightningFireWaterP:
                powerCard = NewPowerInHand(16);
                powerCard.lightning = true;
                powerCard.fire = true;
                powerCard.water = true;
                powerCard.cardType = CardType.LightningFireWaterP;
                break;
            case CardType.WaterFireEarthP:
                powerCard = NewPowerInHand(17);
                powerCard.water = true;
                powerCard.fire = true;
                powerCard.earth = true;
                powerCard.cardType = CardType.WaterFireEarthP;
                break;
            case CardType.WaterAirLightningP:
                powerCard = NewPowerInHand(18);
                powerCard.water = true;
                powerCard.air = true;
                powerCard.lightning = true;
                powerCard.cardType = CardType.WaterAirLightningP;
                break;
            case CardType.WaterAirEarthP:
                powerCard = NewPowerInHand(19);
                powerCard.water = true;
                powerCard.air = true;
                powerCard.earth = true;
                powerCard.cardType = CardType.WaterAirEarthP;
                break;
            case CardType.LightningWaterEarthP:
                powerCard = NewPowerInHand(20);
                powerCard.lightning = true;
                powerCard.water = true;
                powerCard.earth = true;
                powerCard.cardType = CardType.LightningWaterEarthP;
                break;
            case CardType.LightningFireAirP:
                powerCard = NewPowerInHand(21);
                powerCard.lightning = true;
                powerCard.fire = true;
                powerCard.air = true;
                powerCard.cardType = CardType.LightningFireAirP;
                break;
            case CardType.FireAirEarthP:
                powerCard = NewPowerInHand(22);
                powerCard.fire = true;
                powerCard.air = true;
                powerCard.earth = true;
                powerCard.cardType = CardType.FireAirEarthP;
                break;
            case CardType.LightningFireEarthP:
                powerCard = NewPowerInHand(23);
                powerCard.lightning = true;
                powerCard.fire = true;
                powerCard.earth = true;
                powerCard.cardType = CardType.LightningFireEarthP;
                break;
            case CardType.LightningAirEarthP:
                powerCard = NewPowerInHand(24);
                powerCard.lightning = true;
                powerCard.air = true;
                powerCard.earth = true;
                powerCard.cardType = CardType.LightningAirEarthP;
                break;
            case CardType.MegaPowerP:
                powerCard = NewPowerInHand(25);
                powerCard.lightning = true;
                powerCard.air = true;
                powerCard.earth = true;
                powerCard.water = true;
                powerCard.fire = true;
                powerCard.cardType = CardType.MegaPowerP;
                break;
            case CardType.Intelligence:
                 effectCard = NewEffectInHand(0);
                 effectCard.execute = ExecuteIntelligence;
                 effectCard.cardType = CardType.Intelligence;
                 break;
        }
    }

    public CardType DrawCardEnemy()
    {
        CardType c = CardType.None;
        try
        {
            c = cards.Draw();
        }
        catch (System.InvalidOperationException e)
        {
            Debug.Log("No cards! [Deck]");
            BoardManager.GetBoardManager().endGame = true;
            //c = GameObject.FindWithTag("Discard").GetComponent<Discard>().DrawCardEnemy();
        }

        return c;
    }

    public void DrawHandPlayer(int quantity)
    {
        BoardManager.GetBoardManager().texts[1].text = "Player draws " + quantity + " cards";
        for (int i = 0; i < quantity; i++)
            DrawCardPlayer();
    }

    //public CardType[] DrawHandEnemy(int quantity)
    //{
    //    CardType[] c = new CardType[quantity];
    //    for (int i = 0; i < quantity; i++)
    //        c[i] = DrawCardEnemy();
    //    return c;
    //}

    public Power NewPowerInHand(int x)
    {
        GameObject cih = Instantiate(cardInHand, Instantiate(cardPlaceholder, panelHand.transform).transform);
        cih.GetComponent<CardInHand>().SetSpriteAndPrefab(pwSprites[x], powerPrefab);
        //cih.tag = "Card/Power";
        return cih.AddComponent<Power>();
    }


    public Effect NewEffectInHand(int x)
    {
        GameObject cih = Instantiate(cardInHand, Instantiate(cardPlaceholder, panelHand.transform).transform);
        cih.GetComponent<CardInHand>().SetSpriteAndPrefab(efSprites[x], effectPrefab);
        //cih.tag = "Card/Power";
        return cih.AddComponent<Effect>();
    }

    public void NewCardInStandBy(int x)
    {
        GameObject cis = Instantiate(cardInStandBy, Instantiate(cardPlaceholder, panelStandBy.transform).transform);
        cis.GetComponent<CardInStandBy>().SetSpriteAndPrefab(elSprites[x], cardPrefabs[x]);
    }

    public void ExecuteIntelligence(bool isPlayer){
        if(isPlayer)
        {
            DrawHandPlayer(2);
        } else
        {
            Debug.Log("Inimigo usou inteligência(pun intended)");
            BoardManager.GetBoardManager().DrawHandEnemy(2);
        }
    }

    public IDictionary<CardType, EnemyEffect> GetAllEffects(){
        IDictionary<CardType, EnemyEffect> effects = new Dictionary<CardType, EnemyEffect>();
        EnemyEffect effect; 

        effect = new EnemyEffect();
        effect.execute = ExecuteIntelligence;
        effects.Add(CardType.Intelligence, effect); 
        return effects;
    }
}
