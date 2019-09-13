using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool canvasComp = false;
    private CardInHand cardH;
    private CardInStandBy cardS;

    private bool drag;

    void Start()
    {
        cardH = GetComponent<CardInHand>();
        if (cardH == null)
            cardS = GetComponent<CardInStandBy>();
        else
            cardS = null;
    }

    void Update()
    {
        if ((cardH != null && cardH.moveCard) || (cardS != null && cardS.moveCard))
            PutCanvasOnParent();
        else
            RemoveCanvasFromParent();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardH != null)
            GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        if (cardS != null)
            GetComponent<Image>().sprite = cardS.sprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardH != null)
            GetComponent<Image>().color = new Color(1f, 1f, 1f);
        if (cardS != null)
            GetComponent<Image>().sprite = cardS.cardBack;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
        drag = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Down");

        drag = false;

        if (cardH != null)
            cardH.Bind();
        if (cardS != null)
            cardS.Bind();
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //Debug.Log("Up");
        if (drag)
        {
            if (cardH != null)
                cardH.moveCard = false;
            if (cardS != null)
                cardS.moveCard = false;
        }
    }

    public void PutCanvasOnParent()
    {
        if (canvasComp)
            return;

        Canvas c = transform.parent.gameObject.AddComponent<Canvas>();
        c.overrideSorting = true;
        c.sortingOrder = 2;
        transform.parent.gameObject.AddComponent<GraphicRaycaster>();
        canvasComp = true;
    }

    public void RemoveCanvasFromParent()
    {
        if (!canvasComp)
            return;
        
        Destroy(transform.parent.gameObject.GetComponent<GraphicRaycaster>());
        Canvas c = transform.parent.gameObject.GetComponent<Canvas>();
        c.sortingOrder = 1;
        c.overrideSorting = false;
        Destroy(c);
        canvasComp = false;
    }
}
