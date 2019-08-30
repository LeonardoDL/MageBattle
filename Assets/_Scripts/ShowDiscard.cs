using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowDiscard : MonoBehaviour, IPointerClickHandler
{

    public GameObject scrollView;
    public PopulateDiscardUI UI;
    public AnimationManager am;
    public GraphicRaycaster stPlayer;
    public GraphicRaycaster stEnemy;

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
        HideShow();
    }

    public void HideShow()
    {
        if (isShowing)
        {
            scrollView.SetActive(false);
            isShowing = false;
            am.Fade(false);
            stPlayer.enabled = true;
            stEnemy.enabled = true;
            BoardManager.GetBoardManager().HideElementsFromAnimation(false);
        }
        else
        {
            UI.Populate();
            scrollView.SetActive(true);
            isShowing = true;
            am.Fade(true);
            stPlayer.enabled = false;
            stEnemy.enabled = false;
            BoardManager.GetBoardManager().HideElementsFromAnimation(true);
        }
    }
}
