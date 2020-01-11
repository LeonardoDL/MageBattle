using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    private static BoardManager bm;
    [HideInInspector] public BoardManagerHelper bmh;
    public Text[] texts;
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
    [HideInInspector] public LastPlayed last = LastPlayed.None;

    public GameObject playerButton;
    public GameObject enemyButton;
    public GameObject passButton;
    public AudioSource audioS;

    [HideInInspector] public bool endGame;
    public static bool isInTransition;

    [HideInInspector] public EnemyManager enemy;
    [HideInInspector] public GameObject playerHand;
    //public int playerHandSize;

    //public int discardSize;

    [HideInInspector] public GameObject playerStandBy;
    [HideInInspector] public Stack<GameObject> responseStack;

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

    public bool EasterEgg;

    void Start()
    {
        bm = this;
        curState = GameState.DrawPhase;
        curWinCondition = WinCondition.Draw;
        playerCard = CardType.None;
        enemyCard = CardType.None;
        playerEffect = CardType.None;
        enemyEffect = CardType.None;
        enemy = GetComponent<EnemyManager>();
        playerHand = GameObject.FindWithTag("Hand/PlayerHand");
        playerStandBy = GameObject.FindWithTag("StandBy");
        deck = GameObject.FindWithTag("Deck").GetComponent<Deck>();
        victoryDeckPlayer = GameObject.FindWithTag("VictoryDeck/Player").GetComponent<VictoryDeck>();
        victoryDeckEnemy = GameObject.FindWithTag("VictoryDeck/Enemy").GetComponent<VictoryDeck>();
        last = LastPlayed.None;
        responseStack = new Stack<GameObject>();


        StartCoroutine(WaitStart());
        //enemy.setEffects(deck.GetAllEffects());
    }

    private IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(.2f);

        discard = GameObject.FindWithTag("Discard").GetComponent<Discard>();
        bmh.WaitStart();
        DrawCards();
    }

    public static BoardManager GetBoardManager() { return bm; }
    public void SetBoardManagerHelper(BoardManagerHelper b) { bmh = b; }

    public Effect GetEffect() { return GetComponent<Effect>(); }

    public CardType GetPlayerCard() { return playerCard; }
    public CardType GetEnemyCard() { return enemyCard; }

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
                bm.texts[0].text = "Player has no\nElements To Battle!";
                bmh.DiscardPlayerHand();
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
                if (bm.texts[0].text == "Player has no\nElements To Battle!")
                    bm.texts[0].text = "No one has any\nElements To Battle!";
                else
                    bm.texts[0].text = "Enemy has no\nElements To Battle!";

                //int t = enemy.hand.Count;
                //for (int i = 0; i < t; i++)
                //{
                //    enemy.hand.RemoveAt(0);
                //    deck.cardBuilder.RemoveCardFromHand();
                //    new WaitForSeconds(1f);
                //}
                bmh.DiscardEnemyHand();
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
        CardInBoard cardIB = card.GetComponent<CardInBoard>();
        CardType cardType = cardIB.type;

        Debug.Log("Card played: " + cardType + ", during " + curState);

        isInTransition = false;

        //if ((cardType == CardType.Intelligence || cardType == CardType.SuperGenius) &&
        //    (curState == GameState.PlayerPlayPhase || curState == GameState.EnemyPlayPhase))
        //{
        //    card.GetComponentInChildren<CardInBoard>().execute?.Invoke();

        //    if (endGame)
        //    {
        //        //Debug.Log("Deck has " + deck.cards.Count + " cards");
        //        curState = GameState.EndGame;
        //        EndGame();
        //        yield break;
        //    }

        //    if (curState == GameState.PlayerPlayPhase)
        //    {
        //        last = LastPlayed.Player;
        //        while (enemy.isWaiting) yield return new WaitForSeconds(.1f);

        //        if (enemy.HasPlayableCardDraw())
        //        {
        //            curState = GameState.EnemyPlayPhase;
        //            enemy.PlayCardDraw();
        //        }
        //    }
        //    else
        //    {
        //        if (enemy.getJustPlayed())
        //        {
        //            curState = GameState.PlayerPlayPhase;
        //            last = LastPlayed.Enemy;
        //        }
        //    }

        //    yield break;
        //}

        switch (curState)
        {
            case GameState.PlayerPlayPhase:

                if (!passButton.activeSelf)
                    passButton.SetActive(true);

                playerCard = cardType;
                playerBoardCard = card;
                last = LastPlayed.Player;

                curState = GameState.EnemyPlayPhase;
                while (enemy.isWaiting) yield return new WaitForSeconds(.1f);
                enemy.PlayElement();

                break;

            case GameState.EnemyPlayPhase:
                texts[0].text = "";
                texts[1].text = "";
                texts[2].text = "";
                enemyCard = cardType;
                last = LastPlayed.Enemy;
                enemyBoardCard = card;
                curState = GameState.BattlePhase;
                StartCoroutine(Battle());
                break;

            case GameState.PlayerEffectPhase:

                playerEffect = cardType;
                last = LastPlayed.Player;

                bool wait = false;
                Effect.target = Target.None;

                if (cardIB.UI != null)
                {
                    wait = true;
                    cardIB.UI();
                }

                if (wait)
                    while (Effect.target == Target.None)
                        yield return new WaitForEndOfFrame();

                //Iniciar Response
                cardIB.target = Effect.target;
                Effect.target = Target.None;
                responseStack.Push(card);
                curState = GameState.EnemyResponsePhase;

                enemy.PlayResponse();

                yield break;

            case GameState.EnemyEffectPhase:

                enemyEffect = cardType;
                last = LastPlayed.Enemy;

                Effect.target = Target.None;
                cardIB.UI?.Invoke();

                //Iniciar Response
                Debug.Log("EnemyEffect " + Effect.target);
                cardIB.target = Effect.target;
                Effect.target = Target.None;
                responseStack.Push(card);
                curState = GameState.PlayerResponsePhase;

                bmh.StopTime();

                break;

            case GameState.PlayerResponsePhase:

                playerEffect = cardType;
                last = LastPlayed.Player;

                wait = false;
                Effect.target = Target.None;

                if (cardIB.UI != null)
                {
                    wait = true;
                    cardIB.UI();
                }

                if (wait)
                    while (Effect.target == Target.None)
                        yield return new WaitForEndOfFrame();

                //Iniciar Response
                cardIB.target = Effect.target;
                Effect.target = Target.None;
                responseStack.Push(card);
                curState = GameState.EnemyResponsePhase;

                enemy.PlayResponse();

                yield break;

            case GameState.EnemyResponsePhase:

                enemyEffect = cardType;
                last = LastPlayed.Enemy;

                Effect.target = Target.None;
                cardIB.UI?.Invoke();

                //Iniciar Response
                Debug.Log("EnemyResponse " + Effect.target);
                cardIB.target = Effect.target;
                Effect.target = Target.None;
                responseStack.Push(card);
                curState = GameState.PlayerResponsePhase;

                bmh.StopTime();

                break;

            case GameState.PlayerResolutionPhase:

                if (card.GetComponentInChildren<CardInBoard>().execute != null)
                {
                    Debug.Log("Player: Entered Resolution Phase as Effect + " + cardIB.target);
                    Effect.target = cardIB.target;
                    card.GetComponentInChildren<CardInBoard>().execute(); //Execução do efeito

                    while (Effect.waitUI)
                        yield return new WaitForEndOfFrame();

                    if (endGame)
                    {
                        //Debug.Log("Deck has " + deck.cards.Count + " cards");
                        curState = GameState.EndGame;
                        EndGame();
                        yield break;
                    }

                    if (responseStack.Count > 0)
                    {
                        curState = GameState.EnemyResolutionPhase;
                        Resolve();
                        yield break;
                    }
                    else
                    {
                        if (curWinCondition == WinCondition.Loss)
                            curState = GameState.PlayerEffectPhase;
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
                }
                else
                {
                    Debug.Log("Player: Entered Resolution Phase as Power");
                    curWinCondition = WinCondition.Victory;
                    texts[0].text = "Player used a Power";

                    if (responseStack.Count > 0)
                    {
                        curState = GameState.EnemyResolutionPhase;
                        Resolve();
                        yield break;
                    }
                    else
                        curState = GameState.EnemyEffectPhase;

                    while (enemy.isWaiting) yield return new WaitForSeconds(.1f);
                    enemy.PlayPowerOrEffect();
                }

                break;

            case GameState.EnemyResolutionPhase:

                if (card.GetComponentInChildren<CardInBoard>().execute != null)
                {
                    Debug.Log("Enemy: Entered Resolution Phase as Effect + " + cardIB.target);
                    Effect.target = cardIB.target;
                    card.GetComponentInChildren<CardInBoard>().execute();

                    if (endGame)
                    {
                        //Debug.Log("Deck has " + deck.cards.Count + " cards");
                        curState = GameState.EndGame;
                        EndGame();
                        yield break;
                    }

                    if (responseStack.Count > 0)
                    {
                        curState = GameState.PlayerResolutionPhase;

                        Resolve();
                        yield break;
                    }
                    else
                    {
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
                }
                else
                {
                    Debug.Log("Enemy: Entered Resolution Phase as Power");
                    curWinCondition = WinCondition.Loss;
                    texts[0].text = "Enemy used a Power";
                    if (responseStack.Count > 0)
                    {
                        curState = GameState.PlayerResolutionPhase;
                        Resolve();
                        yield break;
                    }
                    else
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

    public void Resolve() {
        StartCoroutine(ResolveParallel()); }

    public IEnumerator ResolveParallel()
    {
        if (curState == GameState.PlayerResolutionPhase)
            responseStack.Peek().GetComponent<CardInBoard>().Activate(SlotsOnBoard.EffectPlayer);
        if (curState == GameState.EnemyResolutionPhase)
            responseStack.Peek().GetComponent<CardInBoard>().Activate(SlotsOnBoard.EffectEnemy);

        yield return new WaitForSeconds(5f);

        Debug.Log("Resolving: " + responseStack.Count);
        if (responseStack.Count == 0)
            yield break;

        StartCoroutine(CardPlayed(responseStack.Pop()));
    }

    public void EndRound()
    {
        if (isInTransition)
            return;

        if (curState == GameState.EndGame)
            return;

        if (curState == GameState.EnemyPlayPhase || curState == GameState.EnemyEffectPhase)
            return;

        isInTransition = true;
        StartCoroutine(EndRoundRoutine());
    }

    public IEnumerator EndRoundRoutine()
    {
        last = LastPlayed.None;
        curState = GameState.ClearPhase;

        yield return new WaitForSeconds(.5f);

        VictoryDeck vp = victoryDeckPlayer;
        VictoryDeck ve = victoryDeckEnemy;

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

        Discard d = discard;
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

        //Debug.Break();
        yield return new WaitForSeconds(.5f);
        isInTransition = false;

        curWinCondition = WinCondition.Draw;
        playerCard = CardType.None;
        enemyCard = CardType.None;
        DrawCards();
    }

    public void EndGame()
    {
        VictoryDeck vp = victoryDeckPlayer;
        VictoryDeck ve = victoryDeckEnemy;
        int pScore = vp.GetScore();
        int eScore = ve.GetScore();

        GetComponent<AnimationManager>().SetAllElements(false);

        texts[0].text = "";
        texts[1].text = "";
        texts[2].text = "";
        texts[7].gameObject.SetActive(true);

        GetComponent<AnimationManager>().Fade(true);
        Destroy(deck.cardBuilder.panelStandBy);
        Destroy(deck.cardBuilder.panelStandByEnemy);
        HideElementsFromAnimation(true);

        if (pScore > eScore)
        {
            texts[7].text = "End of Game\nPlayer won: " + pScore + " x " + eScore;
            texts[7].color = new Color(0f, 1f, 0f);
        }

        if (pScore < eScore)
        {
            texts[7].text = "End of Game\nEnemy won: " + eScore + " x " + pScore;
            texts[7].color = new Color(1f, 0f, 0f);
            // Ativa EasterEgg gamedev, só usar em build pro grupo, não publicar!!!

            if (EasterEgg)
            {
                //Transform board = GameObject.FindWithTag("Board").transform;
                LoseAnimation la = FindObjectOfType<LoseAnimation>();
                Debug.Log("LoseAnimation found in " + la.gameObject.name);
                audioS.Play();
                la.Init();
            }
        }

        if (pScore == eScore)
        {
            texts[7].text = "End of Game\nDraw: " + pScore + " x " + eScore;
            texts[7].color = new Color(1f, 1f, 1f);
        }

        curWinCondition = WinCondition.Draw;
        passButton.SetActive(false);

    }

    public void setButtonEnemyPlayer(bool activate) {
        playerButton.SetActive(activate);
        enemyButton.SetActive(activate);
        GetComponent<AnimationManager>().Fade(activate);
        HideElementsFromAnimation(activate);
    }

    public void setButtonEnemyPlayerPartial(bool playerB, bool enemyB, bool animF)
    {
        playerButton.SetActive(playerB);
        enemyButton.SetActive(enemyB);
        GetComponent<AnimationManager>().Fade(animF);
        HideElementsFromAnimation(animF);
    }

    public void HidePassButton(bool hide) {
        passButton.SetActive(!hide);
        GetComponent<AnimationManager>().FadePartial(hide, hide, false);
        HideElementsFromAnimation(hide);
    }

    public void HideElementsFromAnimation(bool hide)
    {
        foreach (CardInBoard cib in FindObjectsOfType<CardInBoard>())
        {
            cib.HiddenFromAnimation(hide);
        }

        //if (playerBoardCard != null)
        //    playerBoardCard.GetComponentInChildren<CardInBoard>().HiddenFromAnimation(hide);
        //if (enemyBoardCard != null)
        //    enemyBoardCard.GetComponentInChildren<CardInBoard>().HiddenFromAnimation(hide);
    }

    void Update()
    {
        //Peek();
    }
}
