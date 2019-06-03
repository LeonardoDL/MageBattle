using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInBoard : MonoBehaviour
{
    private GameObject slot;
    public CardType type;
    public Execute execute;

    private AnimationManager am;
    //public bool isPlayer = true;

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
        }

        //if (!enabled)
            //return;

        //float f1 = Random.Range(-15f, 15f);
        //float f2 = Random.Range(-15f, 15f);

        //gameObject.transform.rotation = Quaternion.Euler(0f, (f1 + f2)/2, 0f);

        
        slot.GetComponent<PlaceCard>().PlaceOnSlot(gameObject);
        //this.enabled = false;
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (!enabled)
    //        return;

    //    //if (collision.gameObject.tag == "Board" )
    //    slot.GetComponent<PlaceCard>().PlaceOnSlot(gameObject.name);
    //    this.enabled = false;
    //}

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (am == null)
            am = GameObject.Find("Manager").GetComponent<AnimationManager>();
        am.FocusAnimation();
    }
}
