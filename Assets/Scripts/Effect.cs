﻿using System.Collections;
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
        if (BoardManager.curState == GameState.PlayerPlayPhase || BoardManager.curState == GameState.PlayerEffectPhase)
            bm.deck.DrawHandPlayer(2);

        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase)
            bm.DrawHandEnemy(2);
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
            BoardManager.curWinCondition = WinCondition.Victory;
        else
        if (BoardManager.curWinCondition == WinCondition.Victory)
            BoardManager.curWinCondition = WinCondition.Loss;

        return true;
    }

    public bool SuperGenius()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (BoardManager.curState == GameState.PlayerPlayPhase || BoardManager.curState == GameState.PlayerEffectPhase)
            bm.discard.DrawHandPlayer(3);

        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase){
            if( bm.discard.Size() <= 0 )
                return true;
            bm.DrawHandEnemyFromDiscard(3);
        }

        return true;
    }

    public bool Disintegration()
    {
        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase){
            DisintegrationSelection(true);
            return true;
        }

        selection = CardType.Disintegration;
        BoardManager bm = BoardManager.GetBoardManager();
        bm.setButtonEnemyPlayer(true);
        return true;
    }

    public void DisintegrationSelection(bool isPlayer)
    {
        BoardManager bm = BoardManager.GetBoardManager();

        bm.setButtonEnemyPlayer(false);

        if(isPlayer){
            bm.DiscardPlayerHand();
        } else {
            bm.DiscardEnemyHand();
        }
   
    }

    public bool BlackHole()
    {
        BoardManager bm = BoardManager.GetBoardManager();
        List<CardType> standBy;

        
        standBy = bm.GetEnemyStandBy();
               
        standBy.AddRange( bm.GetPlayerStandBy() );

        foreach(CardType card in standBy)
            bm.deck.AddCard(card);
        
        bm.deck.Shuffle();

        return true;
    }

    public bool Eclipse()
    {
        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase){
            EclipseSelection(true);
            return true;
        }
        selection = CardType.Eclipse;
        BoardManager bm = BoardManager.GetBoardManager();
        bm.setButtonEnemyPlayer(true);
        return true;
    }

    public void EclipseSelection(bool isPlayer)
    {
        BoardManager bm = BoardManager.GetBoardManager();
        List<CardType> standBy;

        bm.setButtonEnemyPlayer(false);

        if(isPlayer)
            standBy = bm.GetPlayerStandBy();           
        else 
            standBy = bm.GetEnemyStandBy();

        if (BoardManager.curState == GameState.PlayerPlayPhase || BoardManager.curState == GameState.PlayerEffectPhase){
            foreach(CardType card in standBy)
                bm.victoryDeckPlayer.AddCard(card);
        } else if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase){
            foreach(CardType card in standBy)
                bm.victoryDeckEnemy.AddCard(card);
        }
        

    }

    public bool FishingRod()
    {
        BoardManager bm = BoardManager.GetBoardManager();
        CardType card;
        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase){
            if(bm.GetPlayerHandSize() <= 0)
                return true;
            
            card = bm.GetPlayerCardRandom();
            bm.AddEnemyHand(card);
            return true;
        }
        EnemyPointerHandler.activatePointer(true);
        selection = CardType.FishingRod;
        bm.HidePassButton(true);
        
        return true;
    }

    public void FishingRodSelection()
    {
        EnemyPointerHandler.activatePointer(false);
        BoardManager bm = BoardManager.GetBoardManager();
        bm.HidePassButton(false);
        CardType card;
        card = bm.GetEnemyCardRandom();
        bm.AddPlayerHand(card);      
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
                FishingRodSelection();
                break;
        }
        selection = CardType.None;
    }

    public bool IntelligenceI()
    {
        if (BoardManager.curWinCondition == WinCondition.Victory)
            return false;
        return true;
    }

    public bool PortalI()
    {
        if (BoardManager.curState != GameState.PlayerEffectPhase ||
            BoardManager.curWinCondition == WinCondition.Victory ||
            BoardManager.curWinCondition == WinCondition.Draw)
            return false;
        return true;
    }

    public bool SuperGeniusI()
    {
        if (BoardManager.curWinCondition == WinCondition.Victory)
            return false;
        BoardManager bm = BoardManager.GetBoardManager();

        if(bm.discard.Size() <= 0 )
            return false;

        return true;
    }

    public bool DisintegrationI()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (BoardManager.curState != GameState.PlayerEffectPhase){
            return false;
        }

        return true;
    }

    public bool BlackHoleI()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (BoardManager.curState != GameState.PlayerEffectPhase){
            return false;
        }

        return true;
    }

    public bool EclipseI()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (BoardManager.curState != GameState.PlayerEffectPhase){
            return false;
        }

        return true;
    }

    public bool FishingRodI()
    {
        BoardManager bm = BoardManager.GetBoardManager();

        if (BoardManager.curState != GameState.PlayerEffectPhase){
            return false;
        }
        if(bm.GetEnemyHandSize() <= 0){
            return false;
        }

        return true;
    }
}
