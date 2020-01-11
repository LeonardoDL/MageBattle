using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool Execute();
public delegate bool IsPlayable();

public class Effect : MonoBehaviour
{
    public Execute execute;
    public Execute UI;
    public IsPlayable isPlayable;
    public CardType cardType;

    private static CardType selection = CardType.None;
    public static Target target;
    public static bool waitUI;

    public void Init(CardType c)
    {
        cardType = c;
        switch (c)
        {
            case CardType.Intelligence:
                execute = Intelligence;
                isPlayable = IntelligenceI;
            break;

            case CardType.Portal:
                execute = Portal;
                isPlayable = PortalI;
                break;

            case CardType.SuperGenius:
                execute = SuperGenius;
                isPlayable = SuperGeniusI;
                break;
            case CardType.Disintegration:
                execute = Disintegration;
                UI = DisintegrationUI;
                isPlayable = DisintegrationI;
                break;
            case CardType.BlackHole:
                execute = BlackHole;
                isPlayable = BlackHoleI;
                break;
            case CardType.Eclipse:
                execute = Eclipse;
                UI = EclipseUI;
                isPlayable = EclipseI;
                break;
            case CardType.FishingRod:
                execute = FishingRod;
                isPlayable = FishingRodI;
            break;
        }
    }

    public bool Intelligence()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        bm.texts[1].text = ""; //Gambiarra
        bm.texts[2].text = ""; //Gambiarra

        if (BoardManager.curState == GameState.PlayerPlayPhase || BoardManager.curState == GameState.PlayerResolutionPhase)
        {
            bm.texts[0].text = "Player used Intelligence";
            bm.deck.DrawHandPlayer(2);
        }

        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyResolutionPhase)
        {
            bm.texts[0].text = "Enemy used Intelligence";
            bm.bmh.DrawHandEnemy(2);
        }

        return true;
    }

    public bool Portal()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        //BoardManager.curState = GameState.EnemyPlayPhase;
        bm.playerBoardCard.GetComponent<CardInBoard>().Activate(SlotsOnBoard.ElementEnemyPortal);
        bm.enemyBoardCard.GetComponent<CardInBoard>().Activate(SlotsOnBoard.ElementPlayerPortal);

        GameObject temp = bm.playerBoardCard;
        bm.playerBoardCard = bm.enemyBoardCard;
        bm.enemyBoardCard = temp;

        CardType c = bm.playerCard;
        bm.playerCard = bm.enemyCard;
        bm.enemyCard = c;

        if (BoardManager.curWinCondition == WinCondition.Loss)
        {
            bm.texts[0].text = "Player used Portal";
            BoardManager.curWinCondition = WinCondition.Victory;
        }
        else
        if (BoardManager.curWinCondition == WinCondition.Victory)
        {
            bm.texts[0].text = "Enemy used Portal";
            BoardManager.curWinCondition = WinCondition.Loss;
        }

        return true;
    }

    public bool SuperGenius()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (BoardManager.curState == GameState.PlayerPlayPhase || BoardManager.curState == GameState.PlayerResolutionPhase)
        {
            bm.texts[0].text = "Player used Super Genius";
            bm.discard.DrawHandPlayer(3);
        }
        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyResolutionPhase)
        {
            bm.texts[0].text = "Enemy used Super Genius";
            bm.bmh.DrawHandEnemyFromDiscard(3);
        }

        return true;
    }

    public bool DisintegrationUI()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (IsPlayerERPhase())
        {
            bm.setButtonEnemyPlayerPartial((bm.bmh.GetPlayerHandSize() > 0), (bm.bmh.GetEnemyHandSize() > 0), true);
            bm.texts[0].text = "Player used Disintegration";
        }
        if (IsEnemyERPhase())
        {
            target = Target.Player;
            bm.texts[0].text = "Enemy used Disintegration";
        }

        return true;
    }

    public bool Disintegration()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (target == Target.Player)
        {
            bm.bmh.DiscardPlayerHand();
        }

        if (target == Target.Enemy)
        {
            bm.bmh.DiscardEnemyHand();
        }

        target = Target.None;

        return true;
    }

    public bool BlackHole()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (IsPlayerERPhase())
            bm.texts[0].text = "Player used Black Hole";
        if (IsEnemyERPhase())
            bm.texts[0].text = "Enemy used Black Hole";

        List<CardType> standBy;
        
        standBy = bm.bmh.GetEnemyStandBy();
               
        standBy.AddRange( bm.bmh.GetPlayerStandBy() );

        foreach(CardType card in standBy)
            bm.deck.AddCard(card);
        
        bm.deck.Shuffle();

        return true;
    }

    public bool EclipseUI()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        //Enemy
        if (IsEnemyERPhase())
        {
            if (bm.bmh.GetEnemyStandByCount() > bm.bmh.GetPlayerStandByCount())
                target = Target.Enemy;
            else
                target = Target.Player;

            bm.texts[0].text = "Enemy used Eclipse";
            return true;
        }

        //Player
        if (IsPlayerERPhase())
        {
            bm.setButtonEnemyPlayerPartial((bm.bmh.GetPlayerStandByCount() > 0), (bm.bmh.GetEnemyStandByCount() > 0), true);
            bm.texts[0].text = "Player used Eclipse";
        }

        return true;
    }

    public bool Eclipse()
    {
        BoardManager bm = BoardManager.GetBoardManager();
        List<CardType> standBy = null;

        if (target == Target.None)
            return false;

        if (target == Target.Player)
            standBy = bm.bmh.GetPlayerStandBy();           
        if (target == Target.Enemy)
            standBy = bm.bmh.GetEnemyStandBy();
        
        //Player
        if (BoardManager.curState == GameState.PlayerResolutionPhase)
        {
            foreach(CardType card in standBy)
                bm.victoryDeckPlayer.AddCard(card);
        }

        //Enemy
        if (BoardManager.curState == GameState.EnemyResolutionPhase)
        {
            foreach(CardType card in standBy)
                bm.victoryDeckEnemy.AddCard(card);
        }

        return true;
    }

    public void FishingRodSelection(CardType c)
    {
        BoardManager bm = BoardManager.GetBoardManager();
        bm.HidePassButton(false);
        CardType card;
        card = bm.bmh.GetEnemyCardRandom(c);
        bm.bmh.AddPlayerHand(card);
        bm.texts[0].text = "Player used Fishing Rod";
        EnemyPointerHandler.activatePointer(false);
        waitUI = false;
    }

    public bool FishingRod()
    {
        waitUI = true;
        BoardManager bm = BoardManager.GetBoardManager();
        CardType card;

        //Player
        if (BoardManager.curState == GameState.PlayerResolutionPhase)
        {
            EnemyPointerHandler.activatePointer(true);
            selection = CardType.FishingRod;
            bm.HidePassButton(true);
        }

        //Enemy
        if (BoardManager.curState == GameState.EnemyResolutionPhase)
        {
            bm.texts[0].text = "Enemy used Fishing Rod";
            card = bm.bmh.GetPlayerCardRandom();
            bm.bmh.AddEnemyHand(card);

            return true;
        }

        return true;
    }

    public void Selection(bool isPlayer)
    {
        if (isPlayer)
            target = Target.Player;
        else
            target = Target.Enemy;

        BoardManager.GetBoardManager().setButtonEnemyPlayer(false);
    }

    public bool IntelligenceI()
    {
        BoardManager bm = BoardManager.GetBoardManager();
         // Verificação do Player
        if (BoardManager.curState == GameState.PlayerPlayPhase)
            return true;

        if (BoardManager.curState == GameState.PlayerResponsePhase)
            return true;

        if (BoardManager.curState == GameState.PlayerEffectPhase){
            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw)
                return false;
       
            return true;
        }

        // Verificação do Enemy  
        if (BoardManager.curState == GameState.EnemyPlayPhase)
            return true;

        if (BoardManager.curState == GameState.EnemyResponsePhase)
            return true;

        if(BoardManager.curState == GameState.EnemyEffectPhase)
        {
            if (BoardManager.curWinCondition == WinCondition.Loss || BoardManager.curWinCondition == WinCondition.Draw)
                return false; 
            return true;
        }

        return false;
    }

    public bool PortalI()
    {
        // Verificação do Player
        if (BoardManager.curState == GameState.PlayerResponsePhase)
            return true;

        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {
            if (BoardManager.curWinCondition == WinCondition.Victory ||
                BoardManager.curWinCondition == WinCondition.Draw)
                return false;

            return true;
        }

        // Verificação do Enemy
        if (BoardManager.curState == GameState.EnemyResponsePhase)
            return true;

        if (BoardManager.curState == GameState.EnemyEffectPhase)
        {
            if (BoardManager.curWinCondition == WinCondition.Loss ||
                BoardManager.curWinCondition == WinCondition.Draw)
                return false;

            return true;
        }

        return false;
    }

    public bool SuperGeniusI()
    {
        BoardManager bm = BoardManager.GetBoardManager();
         // Verificação do Player
        if (BoardManager.curState == GameState.PlayerPlayPhase){
                if(bm.discard.Size() < 1 )
                    return false;
                return true;
        }

        if (BoardManager.curState == GameState.PlayerResponsePhase)
            return true;

        if (BoardManager.curState == GameState.PlayerEffectPhase){
            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw)
                return false;

            if(bm.discard.Size() < 1 )
                return false;
        
        return true;
        }
        
        // Verificação do Enemy
        if (BoardManager.curState == GameState.EnemyPlayPhase){
                if(bm.discard.Size() < 1 )
                    return false;
                return true;
        }

        if (BoardManager.curState == GameState.EnemyResponsePhase)
            return true;

        if (BoardManager.curState == GameState.EnemyEffectPhase){
            if (BoardManager.curWinCondition == WinCondition.Loss || BoardManager.curWinCondition == WinCondition.Draw)
                return false;

            if(bm.discard.Size() < 1 )
                return false;
        
            return true;
        }

        return false;
    }

    public bool DisintegrationI()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        // Verificação do Player
        if (BoardManager.curState == GameState.PlayerResponsePhase)
        {
            if (bm.bmh.GetEnemyHandSize() <= 0 && bm.bmh.GetPlayerHandSize() <= 1)
                return false;
            return true;
        }

        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {
            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw)
                return false;

            if (bm.bmh.GetEnemyHandSize() <= 0 && bm.bmh.GetPlayerHandSize() <= 1)
                return false;

            return true;
        }

        // Verificação do Enemy
        if (BoardManager.curState == GameState.EnemyResponsePhase)
        {
            if (bm.bmh.GetEnemyHandSize() <= 0 && bm.bmh.GetPlayerHandSize() <= 1)
                return false;
            return true;
        }

        if (BoardManager.curState == GameState.EnemyEffectPhase)
        {
            if (BoardManager.curWinCondition == WinCondition.Loss || BoardManager.curWinCondition == WinCondition.Draw )
                return false;

            if (bm.bmh.GetPlayerHandSize() <= 0) //&& bm.GetEnemyHandSize() <= 0)
                return false;

            return true;
        }

        return false;
            
    }

    public bool BlackHoleI()
    {
        BoardManager bm = BoardManager.GetBoardManager();
        // Verificação do Player
        if (BoardManager.curState == GameState.PlayerResponsePhase)
        {
            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;
            return true;
        }

        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {

            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw){
                return false;
            }

            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;

            return true;
        }

        // Verificação do Enemy
        if (BoardManager.curState == GameState.EnemyResponsePhase)
        {
            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;
            return true;
        }

        if (BoardManager.curState == GameState.EnemyEffectPhase){
          
            if (BoardManager.curWinCondition == WinCondition.Loss || BoardManager.curWinCondition == WinCondition.Draw){
                return false;
            }

            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;

            //if (bm.GetEnemyStandByCount() > 5 || bm.GetPlayerStandByCount() > 5) //Uma decisao mais inteligente
                //return true;

            //if (bm.GetEnemyStandByCount() > bm.GetPlayerStandByCount()) //Uma decisao mais inteligente
                //return false;

            return true;
        }

        return false;
    }

    public bool EclipseI()
    {
        BoardManager bm = BoardManager.GetBoardManager();
         // Verificação do Player
        
        if (BoardManager.curState == GameState.PlayerResponsePhase)
        {
            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;

            return true;
        }

        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {

            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw){
                return false;
            }

            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;

            return true;
        }

        // Verificação do Enemy
        if (BoardManager.curState == GameState.EnemyResponsePhase)
        {
            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;

            return true;
        }

        if (BoardManager.curState == GameState.EnemyEffectPhase){
          
            if (BoardManager.curWinCondition == WinCondition.Loss || BoardManager.curWinCondition == WinCondition.Draw){
                return false;
            }

            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;

            return true;
        }

        return false;
    }

    public bool FishingRodI()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        // Verificação do Player
        if (BoardManager.curState == GameState.PlayerResponsePhase)
        {
            if (bm.bmh.GetEnemyHandSize() <= 0)
                return false;
            return true;
        }

        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {
            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw)
                return false;

            if(bm.bmh.GetEnemyHandSize() <= 0)
                return false;

            return true;
        }

        // Verificação do Enemy
        if (BoardManager.curState == GameState.EnemyResponsePhase)
        {
            if (bm.bmh.GetPlayerHandSize() <= 0)
                return false;
            return true;
        }

        if (BoardManager.curState == GameState.EnemyEffectPhase){
          
            if (BoardManager.curWinCondition == WinCondition.Loss || BoardManager.curWinCondition == WinCondition.Draw){
                return false;
            }

            if(bm.bmh.GetPlayerHandSize() <= 0)
                return false;

            return true;
        }

        return false;
    }

    private bool IsPlayerERPhase()
    {
        if (BoardManager.curState == GameState.PlayerEffectPhase || BoardManager.curState == GameState.PlayerResponsePhase)
            return true;
        return false;
    }

    private bool IsEnemyERPhase()
    {
        if (BoardManager.curState == GameState.EnemyEffectPhase || BoardManager.curState == GameState.EnemyResponsePhase)
            return true;
        return false;
    }
}
