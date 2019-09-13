using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInStandBy : MonoBehaviour
{
    [HideInInspector]
    public Sprite sprite;
    [HideInInspector]
    public Sprite cardBack;
    public CardType card;
    private Image img;

    private GameObject cardPrefab;
    private float zValue = 12f;

    [HideInInspector]
    public bool moveCard = false;
    private Vector3 offset;

    void Update()
    {
        if (moveCard)
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f)) - offset;
        else
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 8f);

        offset = Vector3.Lerp(offset, Vector3.zero, Time.deltaTime * 5f);
    }

    public void SetSpriteAndPrefab(Sprite s, GameObject p)
    {
        sprite = s;
        cardPrefab = p;
        offset = Vector3.zero;
        img = GetComponent<Image>();
        cardBack = img.sprite;
        //SpriteState ss = new SpriteState();
        //ss.pressedSprite = sprite;
        //GetComponent<Button>().spriteState = ss;
    }

    public void SetType(CardType card)
    {
       this.card = card;
    }

    public void Bind()
    {
        moveCard = !moveCard;
        offset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f)) - transform.position;
    }

    public void Bind(bool value)
    {
        moveCard = value;
        offset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f)) - transform.position;
    }

    public void Summon()
    {
        BoardManager.isInTransition = true;
        //Debug.Log(Input.mousePosition);
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zValue));
        //Debug.Log(v);
        Instantiate(cardPrefab, new Vector3(v.x,4f,v.z), Quaternion.identity);
        Destroy(transform.parent.gameObject);
        //Debug.Break();
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "StandBy")
        {
            if (BoardManager.isInTransition || BoardManager.curState == GameState.EndGame)
            {
                moveCard = false;
                return;
            }

            if (BoardManager.curState == GameState.PlayerPlayPhase)
                Summon();
            else
                moveCard = false;
        }
    }
}
