using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateDiscardUI : MonoBehaviour
{
    public GameObject Image; 

	public int numberToCreate; // number of objects to create. Exposed in inspector

	public List<GameObject> cards;
	void Start()
	{
		//Populate();
		cards = new List<GameObject>();
	}

	public void Populate()
	{
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject != gameObject)
                Destroy(t.gameObject);
        }

        cards = new List<GameObject>();
        GameObject newObj; // Create GameObject instance
        BoardManager bm = BoardManager.GetBoardManager();
        foreach (CardType c in bm.discard.cards){
			newObj = (GameObject)Instantiate(Image, transform);
            //Debug.Log(" it's " + c);
            newObj.GetComponent<Image>().sprite = bm.deck.GetCardBuilder().GetSpriteGameObject(c).Item1;
			cards.Add(newObj);
        }
	}
}
