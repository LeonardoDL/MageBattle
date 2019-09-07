using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInBoard : MonoBehaviour
{
    private GameObject slot;
    public CardType type;
    public Execute execute;

    private bool hidden = false;
    private ParticleSystem ps;
    private SpriteRenderer sprite;

    private AnimationManager am;

    void Start()
    {
        enabled = false;
    }

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
        {
            ps.Stop();
            return;
        }
        Debug.Log("here");
        am.FocusAnimation();
        ps.Stop();
    }

    public void HiddenFromAnimation(bool hide)
    {
        hidden = hide;
    }

    void OnMouseEnter()
    {
        if (!enabled)
            return;
        if (hidden)
            return;

        if (ps == null)
            ps = GetComponentInChildren<ParticleSystem>();
        if (sprite == null)
            sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (type == CardType.Intelligence || type == CardType.SuperGenius)
            if (BoardManager.curState == GameState.PlayerPlayPhase || BoardManager.curState == GameState.EnemyPlayPhase)
                return;

        if (ps != null)//As vezes não tem ps no objeto
            ps.Play();

        if (sprite != null)
            sprite.color = new Color(1f, 1f, 1f, .88f);
    }

    void OnMouseExit()
    {
        if (!enabled)
            return;
        if (hidden)
            return;
        if (sprite != null)
            sprite.color = new Color(1f, 1f, 1f, 1f);
    }
}
