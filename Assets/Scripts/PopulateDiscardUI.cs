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

	void Update()
	{

	}

	public void Populate()
	{
		if(cards.Count != 0){
			foreach(GameObject gObj in cards){
				Destroy(gObj);
			}
			cards = new List<GameObject>();
		}

		GameObject newObj; // Create GameObject instance
        BoardManager bm = BoardManager.GetBoardManager();
        foreach (CardType c in bm.discard.cards){
			newObj = (GameObject)Instantiate(Image, transform);
            newObj.GetComponent<Image>().sprite = bm.deck.GetCardBuilder().GetSpriteGameObject(c).Item1;
			cards.Add(newObj);
        }
	}
}
