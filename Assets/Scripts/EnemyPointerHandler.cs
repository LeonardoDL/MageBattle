using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyPointerHandler : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    private static bool activate = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void activatePointer(bool on){
        activate = on;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {  
        if (activate){
            //GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (activate){
            //GetComponent<Image>().color = new Color(1f, 1f, 1f);
            transform.localScale = Vector3.one;
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
       Effect effect = BoardManager.GetBoardManager().GetEffect();
       transform.localScale = Vector3.one;
       effect.Selection(true);
    }
}
