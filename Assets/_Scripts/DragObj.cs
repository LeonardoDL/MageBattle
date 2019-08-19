using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObj : MonoBehaviour
{
    public float distance = 12;
    private bool moveCard = true;
    public GameObject reference;

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
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("7");
            moveCard = false;
            if (reference.tag == "Card/Element")
                reference.GetComponent<CardInBoard>().Activate(SlotsOnBoard.ElementPlayer);
            if (reference.tag == "Card/Effect" || reference.tag == "Card/Power")
                reference.GetComponent<CardInBoard>().Activate(SlotsOnBoard.EffectPlayer);
            Destroy(gameObject);
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
