using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDiscard : MonoBehaviour, IPointerClickHandler
{

    public GameObject scrollView;
    public PopulateDiscardUI UI;
    bool isShowing = false;
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
        
        if (isShowing){
            scrollView.SetActive(false);
            isShowing = false;
        } else {
            UI.Populate();
            scrollView.SetActive(true);
            isShowing = true;
        }
    }
}
