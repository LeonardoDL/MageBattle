using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInBoard : MonoBehaviour
{
    private GameObject slot;
    public CardType type;
    public Execute execute;

    private bool hidden = false;

    private AnimationManager am;

    public void Activate(SlotsOnBoard place)
    {
        switch (place)
        {
            case SlotsOnBoard.ElementPlayer:
                slot = GameObject.FindWithTag("Slot/ElementPlayer");
                break;
            case SlotsOnBoard.ElementEnemy:
                slot = GameObject.FindWithTag("Slot/ElementEnemy");
                Destroy(gameObject.GetComponent<DragObj>());
                break;
            case SlotsOnBoard.EffectPlayer:
                slot = GameObject.FindWithTag("Slot/EffectPlayer");
                break;
            case SlotsOnBoard.EffectEnemy:
                slot = GameObject.FindWithTag("Slot/EffectEnemy");
                Destroy(gameObject.GetComponent<DragObj>());
                break;
            case SlotsOnBoard.Discard:
                slot = GameObject.FindWithTag("Discard");
                Destroy(gameObject.GetComponent<DragObj>());
                break;
            case SlotsOnBoard.VictoryDeckPlayer:
                slot = GameObject.FindWithTag("VictoryDeck/Player");
                Destroy(gameObject.GetComponent<DragObj>());
                break;
            case SlotsOnBoard.VictoryDeckEnemy:
                slot = GameObject.FindWithTag("VictoryDeck/Enemy");
                Destroy(gameObject.GetComponent<DragObj>());
                break;
            case SlotsOnBoard.ElementPlayerPortal:
                slot = GameObject.FindWithTag("Slot/PortalPlayer");
                break;
            case SlotsOnBoard.ElementEnemyPortal:
                slot = GameObject.FindWithTag("Slot/PortalEnemy");
                break;
        }
        
        slot.GetComponent<PlaceCard>().PlaceOnSlot(gameObject);
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (BoardManager.curState == GameState.ClearPhase || BoardManager.curState == GameState.PlayerPlayPhase
            || BoardManager.curState == GameState.EnemyPlayPhase || BoardManager.curState == GameState.EndGame
            || BoardManager.isInTransition)
            return;

        if (am == null)
            am = GameObject.Find("Manager").GetComponent<AnimationManager>();

        if (hidden)
            return;
        am.FocusAnimation();
    }

    public void HiddenFromAnimation(bool hide)
    {
        hidden = hide;
    }
}
