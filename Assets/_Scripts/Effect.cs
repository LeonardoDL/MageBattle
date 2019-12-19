using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool Execute();
public delegate bool IsPlayable();

public class Effect : MonoBehaviour
{
    public Execute execute;
    public IsPlayable isPlayable;
    public CardType cardType;

    private static CardType selection = CardType.None;

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
                isPlayable = DisintegrationI;
                break;
            case CardType.BlackHole:
                execute = BlackHole;
                isPlayable = BlackHoleI;
                break;
            case CardType.Eclipse:
                execute = Eclipse;
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

        if (BoardManager.curState == GameState.PlayerPlayPhase || BoardManager.curState == GameState.PlayerEffectPhase)
        {
            bm.texts[0].text = "Player used Intelligence";
            bm.deck.DrawHandPlayer(2);
        }

        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase)
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

        if (BoardManager.curState == GameState.PlayerPlayPhase || BoardManager.curState == GameState.PlayerEffectPhase)
        {
            bm.texts[0].text = "Player used Super Genius";
            bm.discard.DrawHandPlayer(3);
        }
        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase)
        {
            bm.texts[0].text = "Enemy used Super Genius";
            bm.bmh.DrawHandEnemyFromDiscard(3);
        }

        return true;
    }

    public bool Disintegration()
    {
        if (BoardManager.curState == GameState.EnemyEffectPhase){
            DisintegrationSelection(true);
            return true;
        }

        selection = CardType.Disintegration;
        BoardManager bm = BoardManager.GetBoardManager();
        //bm.setButtonEnemyPlayerPartial((bm.GetPlayerHandSize() > 0), (bm.GetEnemyHandSize() > 0), true);
        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {
            bm.setButtonEnemyPlayerPartial((bm.bmh.GetPlayerHandSize() > 0), (bm.bmh.GetEnemyHandSize() > 0), true);
            bm.texts[0].text = "Player used Disintegration";
        }
        if (BoardManager.curState == GameState.EnemyEffectPhase)
            bm.texts[0].text = "Enemy used Disintegration";

        return true;
    }

    public void DisintegrationSelection(bool isPlayer)
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (BoardManager.curState == GameState.PlayerEffectPhase)
            bm.setButtonEnemyPlayer(false);

        if (isPlayer)
        {
            bm.bmh.DiscardPlayerHand();
        } else {
            bm.bmh.DiscardEnemyHand();
        }
   
    }

    public bool BlackHole()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (BoardManager.curState == GameState.PlayerEffectPhase)
            bm.texts[0].text = "Player used Black Hole";
        if (BoardManager.curState == GameState.EnemyEffectPhase)
            bm.texts[0].text = "Enemy used Black Hole";

        List<CardType> standBy;
        
        standBy = bm.bmh.GetEnemyStandBy();
               
        standBy.AddRange( bm.bmh.GetPlayerStandBy() );

        foreach(CardType card in standBy)
            bm.deck.AddCard(card);
        
        bm.deck.Shuffle();

        return true;
    }

    public bool Eclipse()
    {
        BoardManager bm = BoardManager.GetBoardManager();
        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase){
            if (bm.bmh.GetEnemyStandByCount() > bm.bmh.GetPlayerStandByCount())
                EclipseSelection(false);
            else
                EclipseSelection(true);

            bm.texts[0].text = "Enemy used Eclipse";
            return true;
        }
        selection = CardType.Eclipse;
        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {
            bm.setButtonEnemyPlayerPartial((bm.bmh.GetPlayerStandByCount() > 0), (bm.bmh.GetEnemyStandByCount() > 0), true);
            bm.texts[0].text = "Player used Eclipse";
        }

        return true;
    }

    public void EclipseSelection(bool isPlayer)
    {
        BoardManager bm = BoardManager.GetBoardManager();
        List<CardType> standBy;

        if (BoardManager.curState == GameState.PlayerEffectPhase)
            bm.setButtonEnemyPlayer(false);

        if (isPlayer)
            standBy = bm.bmh.GetPlayerStandBy();           
        else 
            standBy = bm.bmh.GetEnemyStandBy();

        if (BoardManager.curState == GameState.PlayerEffectPhase){
            foreach(CardType card in standBy)
                bm.victoryDeckPlayer.AddCard(card);
        }

        if (BoardManager.curState == GameState.EnemyEffectPhase){
            foreach(CardType card in standBy)
                bm.victoryDeckEnemy.AddCard(card);
        }
    }

    public bool FishingRod()
    {
        BoardManager bm = BoardManager.GetBoardManager();
        CardType card;
        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase){
            bm.texts[0].text = "Enemy used Fishing Rod";
            card = bm.bmh.GetPlayerCardRandom();
            bm.bmh.AddEnemyHand(card);

            return true;
        }
        EnemyPointerHandler.activatePointer(true);
        selection = CardType.FishingRod;
        bm.HidePassButton(true);
        
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
    }

    public void Selection(bool isPlayer)
    {
        switch (selection)
        {       
            case CardType.Disintegration:
                DisintegrationSelection(isPlayer);
                break;
            case CardType.Eclipse:
                EclipseSelection(isPlayer);
                break;
            case CardType.FishingRod:
                //FishingRodSelection();
                break;
        }
        selection = CardType.None;
    }

    public bool IntelligenceI()
    {
        BoardManager bm = BoardManager.GetBoardManager();
         // Verificação do Player
        if (BoardManager.curState == GameState.PlayerPlayPhase){
                return true;
        } 
        
        if(BoardManager.curState == GameState.PlayerEffectPhase){
            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw)
                return false;
       
            return true;
        }
        // Verificação do Enemy  
        
        if (BoardManager.curState == GameState.EnemyPlayPhase){
            return true;
        }

        if(BoardManager.curState == GameState.EnemyEffectPhase){
            if (BoardManager.curWinCondition == WinCondition.Loss || BoardManager.curWinCondition == WinCondition.Draw)
                return false; 
            return true;
        }

        return false;
    }

    public bool PortalI()
    {

        // Verificação do Player
        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {
            if (BoardManager.curWinCondition == WinCondition.Victory ||
                BoardManager.curWinCondition == WinCondition.Draw)
                return false;

            return true;
        // Verificação do Enemy  
        } else if (BoardManager.curState == GameState.EnemyEffectPhase)
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
        
        if(BoardManager.curState == GameState.PlayerEffectPhase){
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

        if(BoardManager.curState == GameState.EnemyEffectPhase){
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
        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {
            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw)
                return false;

            if (bm.bmh.GetEnemyHandSize() <= 0 && bm.bmh.GetPlayerHandSize() <= 1)
                return false;

            return true;
        }

        // Verificação do Enemy 
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
        if (BoardManager.curState == GameState.PlayerEffectPhase){

            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw){
                return false;
            }

            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;

            return true;
        // Verificação do Enemy  
        } else if (BoardManager.curState == GameState.EnemyEffectPhase){
          
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
        if (BoardManager.curState == GameState.PlayerEffectPhase){

            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw){
                return false;
            }

            if (bm.bmh.GetPlayerStandByCount() <= 0 && bm.bmh.GetEnemyStandByCount() <= 0)
                return false;

            return true;
        }

        // Verificação do Enemy  
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
        if (BoardManager.curState == GameState.PlayerEffectPhase)
        {
            if (BoardManager.curWinCondition == WinCondition.Victory || BoardManager.curWinCondition == WinCondition.Draw)
                return false;

            if(bm.bmh.GetEnemyHandSize() <= 0)
                return false;

            return true;
        }

        // Verificação do Enemy  
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
}
