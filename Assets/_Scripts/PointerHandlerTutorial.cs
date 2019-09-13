using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PointerHandlerTutorial : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject tooltip;
    public Vector3 percentOffset;

    public string description;
    private CardType card = CardType.None;
    private GameObject instantTooltip;

    public void DefineDescription()
    {
        Power p = GetComponent<Power>();
        Effect e = null;
        if (p != null)
            card = p.cardType;
        else
            e = GetComponent<Effect>();

        if (e != null)
            card = e.cardType;
        
        switch (card)
        {
            case CardType.None:
                //description = "Oh Wait, a Bug";
                break;

            case CardType.Intelligence:
                description = "Draw 2 cards";
                break;

            case CardType.Portal:
                description = "Switch the 2 elements on the arena";
                break;

            case CardType.SuperGenius:
                description = "Draw 3 cards from the Discard (no elements in there!)";
                break;

            case CardType.Disintegration:
                description = "Discard a player's hand";
                break;

            case CardType.BlackHole:
                description = "Shuffle all elements (that are face down) in the board back into the deck";
                break;

            case CardType.Eclipse:
                description = "Shuffle a player's elements (that are face down) into your Victory Deck";
                break;

            case CardType.FishingRod:
                description = "Pick a card from your opponent's hand";
                break;

            default:

                //Debug.Log("" + card + "  " + p.air);
                CardType element = BoardManager.GetBoardManager().playerCard;
                if (element == CardType.None)
                    description = "";
                else
                {
                    if (GetComponent<CardInHand>().Correspondence(element, p))
                        description = "You can play this Power!";
                    else
                        description = "You can't play this because the elements don't match!";
                }
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DefineDescription();
        if (description == "")
            return;

        if (transform.parent.parent != null)
            instantTooltip = Instantiate(tooltip, Input.mousePosition + GetTheLongLine(), Quaternion.identity, transform.parent.parent.parent);
        else
            instantTooltip = Instantiate(tooltip, Input.mousePosition + GetTheLongLine(), Quaternion.identity, transform.parent);

        instantTooltip.GetComponentInChildren<TextMeshProUGUI>().text = description;
        Destroy(instantTooltip, 6f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (instantTooltip != null)
            Destroy(instantTooltip);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (instantTooltip != null)
            Destroy(instantTooltip);
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerCurrentRaycast.gameObject != gameObject)
            return;

        DefineDescription();
        if (description == "")
            return;

        if (transform.parent.parent != null)
            instantTooltip = Instantiate(tooltip, Input.mousePosition + GetTheLongLine(), Quaternion.identity, transform.parent.parent.parent);
        else
            instantTooltip = Instantiate(tooltip, Input.mousePosition + GetTheLongLine(), Quaternion.identity, transform.parent);

        instantTooltip.GetComponentInChildren<TextMeshProUGUI>().text = description;
        //Destroy(instantTooltip, 6f);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (instantTooltip != null)
                instantTooltip.transform.position = Input.mousePosition + GetTheLongLine();
        }
        else
        {
            if (instantTooltip != null)
                Destroy(instantTooltip);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Destroy(instantTooltip);
    }

    private Vector3 GetTheLongLine()
    {
        return new Vector3(percentOffset.x * 0.01f * Screen.width, percentOffset.y * 0.01f * Screen.height, 0f);
    }
}
