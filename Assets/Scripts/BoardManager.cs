using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    private static BoardManager bm;
    public Text[] texts;
    [HideInInspector] public Deck deck;
    [HideInInspector] public Discard discard;

    public Transform enemySlot;
    public GameObject[] cardPrefabs;
    public GameObject powerPrefab;
    public Sprite[] sprites;

    [HideInInspector] public CardType playerCard;
    [HideInInspector] public CardType enemyCard;
    [HideInInspector] public GameObject playerBoardCard;
    [HideInInspector] public GameObject enemyBoardCard;

    [HideInInspector] public bool endGame;
    [HideInInspector] public static bool isInTransition;

    private EnemyManager enemy;
    private GameObject playerHand;
    private GameObject playerStandBy;

    public GameState currentState = GameState.None;
    public static GameState curState = GameState.None;
    public static WinCondition curWinCondition = WinCondition.Draw;

    public void Peek() { currentState = curState; }

    void Start()
    {
        bm = this;
        curState = GameState.DrawPhase;
        playerCard = CardType.None;
        enemyCard = CardType.None;
        enemy = GetComponent<EnemyManager>();
        playerHand = GameObject.FindWithTag("Hand");
        playerStandBy = GameObject.FindWithTag("StandBy");
        deck = GameObject.FindWithTag("Deck").GetComponent<Deck>();
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
    public CardType GetPlayerCard() { return playerCard; }
    public CardType GetEnemyCard() { return enemyCard; }

    public void DrawHandEnemy(int qnt){
        enemy.DrawHandEnemy(qnt);
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
                foreach (Transform t in playerHand.GetComponentInChildren<Transform>())
                    Destroy(t.gameObject);
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
                Debug.Log("No Elements To Battle! [Enemy]");
                for (int i = 0; i < enemy.hand.Count; i++)
                    enemy.hand.RemoveAt(0);
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
        
            if(enemy.hasCard(CardType.Intelligence)){
                enemy.SummonEffect(CardType.Intelligence);
            }

            curState = GameState.PlayerPlayPhase;
        }

        texts[5].text = "" + deck.cards.Count;
        int asas = discard.cards.Count;
        texts[6].text = "" + asas;
    }

    public void CardPlayed(GameObject card)
    {
        CardType cardType = card.GetComponent<CardInBoard>().type;
        isInTransition = false;

        if (cardType == CardType.Intelligence || cardType == CardType.SuperGenius)
        {
            card.GetComponentInChildren<CardInBoard>().execute?.Invoke();
            return;
        }
        
        switch (curState)
            {
                case GameState.PlayerPlayPhase:
                    playerCard = cardType;
                    playerBoardCard = card;
                    enemy.SummonElement(cardPrefabs, enemySlot);
                    curState = GameState.EnemyPlayPhase;
                    break;

                case GameState.EnemyPlayPhase:
                    texts[0].text = "";
                    texts[1].text = "";
                    texts[2].text = "";
                    enemyCard = cardType;
                    enemyBoardCard = card;
                    curState = GameState.BattlePhase;
                    Battle();
                    break;

                case GameState.PlayerEffectPhase:
                    curWinCondition = WinCondition.Victory;
                    card.GetComponentInChildren<CardInBoard>().execute?.Invoke();
                    enemy.SummonPower(sprites, powerPrefab, enemySlot);
                    curState = GameState.EnemyEffectPhase;
                    break;

                case GameState.EnemyEffectPhase:
                    curWinCondition = WinCondition.Loss;
                    Debug.Log(card.name);
                    card.GetComponentInChildren<CardInBoard>().execute?.Invoke();
                    curState = GameState.PlayerEffectPhase;
                    break;
            }
    }

    public void Battle()
    {
        if (curState != GameState.BattlePhase)
            return;

        switch (CheckWin(playerCard, enemyCard))
        {
            case WinCondition.Victory:

                curWinCondition = WinCondition.Victory;
                curState = GameState.EnemyEffectPhase;
                enemy.SummonPower(sprites, powerPrefab, enemySlot);
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

        texts[0].fontSize = 40;
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

    void Update()
    {
        Peek();
    }
}
