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

        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase)
            bm.DrawHandEnemyFromDiscard(3);

        return true;
    }

    public bool Disintegration()
    {
        if (BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EnemyEffectPhase){
            DisintegrationSelection(true);
            return true;
        }

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
        //verificar se tem 0 cartas no descarte
        if (BoardManager.curWinCondition == WinCondition.Victory)
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
}
