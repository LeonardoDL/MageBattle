using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<CardType> hand;
    public List<CardType> standBy;
    public bool isActive;

    private Deck deck;
    IDictionary<CardType, EnemyEffect> effects;

    // Start is called before the first frame update
    void Awake()
    {
        if (isActive)
        {
            hand = new List<CardType>();
            standBy = new List<CardType>();
            deck = GameObject.FindWithTag("Deck").GetComponent<Deck>();
        }
    }

    public void setEffects(IDictionary<CardType, EnemyEffect> effects){
        this.effects = effects;
    }

    public void DrawHandEnemy(int quantity)
    {
        BoardManager.GetBoardManager().texts[2].text = "Enemy draws " + quantity + " cards";

        for (int i = 0; i < quantity; i++)
        {
            CardType c = deck.DrawCardEnemy();
            if (c != CardType.None)
                hand.Add(c);
        }

        PassElementsToStandBy();
    }

    public bool hasCard(CardType cardType){
        foreach (CardType card in hand)
        {
           if(card == cardType)
            return true;
        }
        return false;
    }

    public void SummonEffect(CardType cardType){
         switch (cardType)
                {
                 case CardType.Intelligence:
                    effects[CardType.Intelligence].execute(false);
                    break;
                 default:
                    break;
                }
    }

    public void SummonElement(GameObject[] cardPrefabs, Transform enemySlot)
    {
        GameObject cardPrefab = null;
        CardType card_chosen = standBy[Random.Range(0, standBy.Count)];
        standBy.Remove(card_chosen);
        
        switch (card_chosen)
        {
            case CardType.AirE:
                cardPrefab = cardPrefabs[0];
                break;
            case CardType.ArcanaE:
                cardPrefab = cardPrefabs[1];
                break;
            case CardType.EarthE:
                cardPrefab = cardPrefabs[2];
                break;
            case CardType.FireE:
                cardPrefab = cardPrefabs[3];
                break;
            case CardType.LightningE:
                cardPrefab = cardPrefabs[4];
                break;
            case CardType.WaterE:
                cardPrefab = cardPrefabs[5];
                break; 
        }
        if (cardPrefab != null)
        {
            Instantiate(cardPrefab, enemySlot.position + new Vector3(0f, 0f, 5f), Quaternion.identity).GetComponent<CardInBoard>().Activate(SlotsOnBoard.ElementEnemy);
            return;
        }
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

    public void SummonPower(Sprite[] sprites, GameObject powerPrefab, Transform enemySlot)
    {
        Sprite s = null;
        // <CardType , Sprite Index> O primeiro elemento da hashtable é o tipo da carta e o segundo é o index da imagem do elemento
        IDictionary<CardType, Sprite> powersToWin = new Dictionary<CardType, Sprite>();
          
        // Não importa o tipo da carta do inimigo em jogo, jogar mega poder sempre será uma opção caso tenha em mãos.
        powersToWin.Add(CardType.MegaPowerP, sprites[25]); 

        switch (BoardManager.GetBoardManager().enemyCard)
        {
            case CardType.AirE:

                powersToWin.Add(CardType.AirP, sprites[0]); 
                powersToWin.Add(CardType.WaterAirP, sprites[6]);
                powersToWin.Add(CardType.FireAirP, sprites[9]);  
                powersToWin.Add(CardType.AirEarthP, sprites[13]);
                powersToWin.Add(CardType.AirLightningP, sprites[12]);                

                powersToWin.Add(CardType.WaterFireAirP, sprites[15]);
                powersToWin.Add(CardType.WaterAirLightningP, sprites[18]);
                powersToWin.Add(CardType.WaterAirEarthP, sprites[19]);  
                powersToWin.Add(CardType.LightningFireAirP, sprites[21]);
                powersToWin.Add(CardType.FireAirEarthP, sprites[22]);
                powersToWin.Add(CardType.LightningAirEarthP, sprites[24]);


                break;

            case CardType.EarthE:

                powersToWin.Add(CardType.EarthP, sprites[1]);  
                powersToWin.Add(CardType.EarthLightningP, sprites[14]);    
                powersToWin.Add(CardType.AirEarthP, sprites[13]);    
                powersToWin.Add(CardType.FireEarthP, sprites[11]);    
                powersToWin.Add(CardType.WaterEarthP, sprites[8]);

                powersToWin.Add(CardType.WaterAirEarthP, sprites[19]);
                powersToWin.Add(CardType.FireAirEarthP, sprites[22]);
                powersToWin.Add(CardType.LightningAirEarthP, sprites[24]);
                powersToWin.Add(CardType.WaterFireEarthP, sprites[17]);
                powersToWin.Add(CardType.LightningWaterEarthP, sprites[20]);
                powersToWin.Add(CardType.LightningFireEarthP, sprites[23]);

                break;

            case CardType.FireE:

                powersToWin.Add(CardType.FireP, sprites[2]);    
                powersToWin.Add(CardType.WaterFireP, sprites[5]);
                powersToWin.Add(CardType.FireAirP, sprites[9]);    
                powersToWin.Add(CardType.FireEarthP, sprites[11]);    
                powersToWin.Add(CardType.FireLightningP, sprites[10]);   

                powersToWin.Add(CardType.WaterFireAirP, sprites[15]);
                powersToWin.Add(CardType.LightningFireAirP, sprites[21]);
                powersToWin.Add(CardType.FireAirEarthP, sprites[22]);
                powersToWin.Add(CardType.WaterFireEarthP, sprites[17]);
                powersToWin.Add(CardType.LightningFireEarthP, sprites[23]);
                powersToWin.Add(CardType.LightningFireWaterP, sprites[16]);


                break;

            case CardType.LightningE:

                powersToWin.Add(CardType.LightningP, sprites[3]); 
                powersToWin.Add(CardType.AirLightningP, sprites[12]);
                powersToWin.Add(CardType.EarthLightningP, sprites[14]);    
                powersToWin.Add(CardType.FireLightningP, sprites[10]);    
                powersToWin.Add(CardType.WaterLightningP, sprites[7]);    

                powersToWin.Add(CardType.WaterAirLightningP, sprites[18]);
                powersToWin.Add(CardType.LightningFireAirP, sprites[21]);
                powersToWin.Add(CardType.LightningAirEarthP, sprites[24]);
                powersToWin.Add(CardType.LightningWaterEarthP, sprites[20]);
                powersToWin.Add(CardType.LightningFireEarthP, sprites[23]);
                powersToWin.Add(CardType.LightningFireWaterP, sprites[16]);

                break;

            case CardType.WaterE:

                powersToWin.Add(CardType.WaterP, sprites[4]);    
                powersToWin.Add(CardType.WaterFireP, sprites[5]);
                powersToWin.Add(CardType.WaterAirP, sprites[6]);    
                powersToWin.Add(CardType.WaterEarthP, sprites[8]);    
                powersToWin.Add(CardType.WaterLightningP, sprites[7]); 

                powersToWin.Add(CardType.WaterFireAirP, sprites[15]);
                powersToWin.Add(CardType.WaterFireEarthP, sprites[17]);
                powersToWin.Add(CardType.WaterAirLightningP, sprites[18]);
                powersToWin.Add(CardType.WaterAirEarthP, sprites[19]); 
                powersToWin.Add(CardType.LightningWaterEarthP, sprites[20]);
                powersToWin.Add(CardType.LightningFireWaterP, sprites[16]);

                break;

            case CardType.ArcanaE:
                
                powersToWin.Add(CardType.AirP, sprites[0]);    
                powersToWin.Add(CardType.EarthP, sprites[1]);    
                powersToWin.Add(CardType.FireP, sprites[2]);    
                powersToWin.Add(CardType.LightningP, sprites[3]);    
                powersToWin.Add(CardType.WaterP, sprites[4]); 

                powersToWin.Add(CardType.WaterFireP, sprites[5]);
                powersToWin.Add(CardType.WaterAirP, sprites[6]);
                powersToWin.Add(CardType.WaterLightningP, sprites[7]); 
                powersToWin.Add(CardType.WaterEarthP, sprites[8]);    
                powersToWin.Add(CardType.FireAirP, sprites[9]);    
                powersToWin.Add(CardType.FireLightningP, sprites[10]);    
                powersToWin.Add(CardType.FireEarthP, sprites[11]);    
                powersToWin.Add(CardType.AirLightningP, sprites[12]);
                powersToWin.Add(CardType.AirEarthP, sprites[13]);    
                powersToWin.Add(CardType.EarthLightningP, sprites[14]);  

                powersToWin.Add(CardType.WaterFireAirP, sprites[15]);
                powersToWin.Add(CardType.LightningFireWaterP, sprites[16]);
                powersToWin.Add(CardType.WaterFireEarthP, sprites[17]);
                powersToWin.Add(CardType.WaterAirLightningP, sprites[18]);
                powersToWin.Add(CardType.WaterAirEarthP, sprites[19]);  
                powersToWin.Add(CardType.LightningWaterEarthP, sprites[20]);
                powersToWin.Add(CardType.LightningFireAirP, sprites[21]);
                powersToWin.Add(CardType.FireAirEarthP, sprites[22]);
                powersToWin.Add(CardType.LightningFireEarthP, sprites[23]);
                powersToWin.Add(CardType.LightningAirEarthP, sprites[24]);  


                break;
        }


        foreach (KeyValuePair<CardType, Sprite> item in powersToWin) 
        {
            if (hand.Contains(item.Key))
            {
                GameObject g = Instantiate(powerPrefab, enemySlot.position + new Vector3(0f, 0f, 5f), Quaternion.identity);
                hand.Remove(item.Key);
                g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.Value;
                g.GetComponent<CardInBoard>().type = item.Key;
                g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.EffectEnemy);
                break;
            }
         }
       
    }
}
