using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteHandler : MonoBehaviour
{
	public Sprite cardFront;
	public CardType card;
	private Sprite cardBack;
	private Image image;
	
	public void Swap()
	{
		if (image == null)
			image = GetComponent<Image>();
		if (cardBack == null)
			cardBack = image.sprite;
		
		if (image.sprite == cardBack)
			image.sprite = cardFront;
		else
		if (image.sprite == cardFront)
			image.sprite = cardBack;
	}
	
	public void SetFront() //Not Recomended
	{
		if (image == null)
			image = GetComponent<Image>();
		
		image.sprite = cardFront;
	}
	
	public void SetBack() //Not Recomended
	{
		if (image == null)
			image = GetComponent<Image>();
		
		if (cardBack == null)
			cardBack = image.sprite;
		else
			image.sprite = cardBack;
	}
}
