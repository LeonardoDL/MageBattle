using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObj : MonoBehaviour
{
    public float distance = 12;
    private bool moveCard = true;
    public GameObject reference;

    private BoardManager bm;

    void Start()
    {
        bm = BoardManager.GetBoardManager();
    }

    //void OnMouseDrag()
    //{
        //moveCard = true;
        //if (!enabled)
        //    return;

        //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        //Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        //transform.position = objectPos;
    //}

    void Update()
    {
        if (moveCard)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

            transform.position = objectPos;
        }
        if (Input.GetMouseButtonUp(0) && moveCard)
        {
            moveCard = false;
            if (reference.tag == "Card/Element")
                reference.GetComponent<CardInBoard>().Activate(SlotsOnBoard.ElementPlayer);
            if (reference.tag == "Card/Effect" || reference.tag == "Card/Power")
                reference.GetComponent<CardInBoard>().Activate(SlotsOnBoard.Stack);
            Destroy(gameObject);
        }
        if (Input.GetMouseButtonUp(1) && moveCard)
        {
            moveCard = false;
            bm.deck.CreateCardForPlayer(reference.GetComponent<CardInBoard>().type);
            BoardManager.isInTransition = false;
            Destroy(transform.parent.gameObject);

            if (BoardManager.curState == GameState.PlayerResponsePhase)
                bm.bmh.StopTime();
        }
    }

    //public void OnPointerDown(PointerEventData eventData)
    //void OnMouseDown()
    //{
    //    moveCard = true;
    //}

    //public void OnPointerUp(PointerEventData pointerEventData)
    //void OnMouseUp()
    //{
        //Debug.Log("7");
        //moveCard = false;
        //if (reference.tag == "Card/Element")
        //    reference.GetComponent<CardInBoard>().Activate(SlotsOnBoard.ElementPlayer);
        //if (reference.tag == "Card/Effect" || reference.tag == "Card/Power")
        //    reference.GetComponent<CardInBoard>().Activate(SlotsOnBoard.EffectPlayer);
        //Destroy(gameObject);
    //}
}
