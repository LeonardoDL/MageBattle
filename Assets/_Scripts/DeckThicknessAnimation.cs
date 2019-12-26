using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeckOption
{
    MainDeck,
    Discard,
    VictoryDeck,
}

public class DeckThicknessAnimation : MonoBehaviour
{
    public DeckOption option;

    private Deck mainDeck;

    private int quant = 20;
    private int old_quant;
    private float floatQuant;
    private Transform[] cards;

    private int total;
    private bool wait = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(.2f);

        Transform child = transform.Find("Cards");
        cards = new Transform[child.childCount];
        for (int i = 0; i < child.childCount; i++)
            cards[i] = child.GetChild(i);

        switch (option)
        {
            case DeckOption.MainDeck:
                quant = cards.Length;
                floatQuant = (float)cards.Length;
                break;
            case DeckOption.Discard:
            case DeckOption.VictoryDeck:
                quant = 0;
                floatQuant = 0f;
                break;
        }

        old_quant = -1;
        mainDeck = GameObject.FindWithTag("Deck").GetComponent<Deck>();
        total = mainDeck.cards.Count;

        wait = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (wait) return;

        quant = Mathf.Clamp(quant, 0, cards.Length - 1);
        if (quant == old_quant)
            return;
        old_quant = quant;

        for (int i = 0; i < quant; i++)
            cards[i].gameObject.SetActive(true);
        for (int i = quant; i < cards.Length; i++)
            cards[i].gameObject.SetActive(false);

        if (option == DeckOption.Discard && quant > 0)
        {
            //Esta fucking linha pega o card que ta no topo do descarte e corrige o sprite dele
            cards[quant - 1].Find("SpriteFront").GetComponent<SpriteRenderer>().sprite = 
                mainDeck.GetComponent<CardBuilder>().GetSprite(GetComponent<Discard>().cards[0]);
        }
    }

    public enum ComputeMode
    {
        Drawn,
        Added
    }

    public void ComputeCard(ComputeMode cm)
    {
        if (cm == ComputeMode.Drawn)
            floatQuant -= (float)cards.Length / (float)total;
        else
            floatQuant += (float)cards.Length / (float)total;

        quant = Mathf.RoundToInt(floatQuant);
    }
}
