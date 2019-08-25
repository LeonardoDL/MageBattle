using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ClickableText : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI tmp;
    public UnityEvent[] events;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmp, Input.mousePosition, null);
        //Debug.Log("Clicked " + linkIndex);
        if (linkIndex != -1)
        {
            //TMP_LinkInfo linkInfo = tmp.textInfo.linkInfo[linkIndex];
            events[linkIndex].Invoke();
            transform.parent.gameObject.SetActive(false);
        }
    }
}
