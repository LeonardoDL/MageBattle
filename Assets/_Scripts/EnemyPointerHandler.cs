using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyPointerHandler : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    private static bool activate = false;

    private Vector3 targetSize = Vector3.one;
    private Vector3 targetPos = Vector3.zero;
    private Color targetCol = Color.white;
    private float bigger = 1f;
    private Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            targetSize = new Vector3(1.1f, 1.1f, 1.1f);
            targetPos = new Vector3(0f, -4f, 0f);
            targetCol = new Color(180f / 255f, 240f / 255f, 1f);
        }
        else
        {
            targetSize = Vector3.one;
            targetPos = Vector3.zero;
            targetCol = Color.white;
        }

        transform.localScale = targetSize * bigger;
        transform.localPosition = targetPos;
        img.color = targetCol;
    }

    public static void activatePointer(bool on)
    {
        activate = on;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {  
        if (activate){
            //GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
            bigger = 1.4f;
        }
        else
            bigger = 1f;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (activate){
        //    //GetComponent<Image>().color = new Color(1f, 1f, 1f);
        //    bigger = 1f;
        //}
        //else
        bigger = 1f;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (!activate)
            return;

        Effect effect = BoardManager.GetBoardManager().GetEffect();
        effect.FishingRodSelection(GetComponent<SpriteHandler>().card);
    }
}
