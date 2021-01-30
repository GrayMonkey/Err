using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class mnu_TileRef : MonoBehaviour
{
    //[SerializeField] GameObject bodyText;
    [SerializeField] CanvasGroup tileIndex;
    [SerializeField] CanvasGroup tileDetails;
    [SerializeField] Text tileDetailsTitle;
    [SerializeField] Text tileDetailsBody;
    [SerializeField] Image tileDetailsIcon;
    [SerializeField] ScrollRect tileDetailsScrollRect;

    private Image tileIcon;
    private Text tileTitle;
    private string tileBody;

    /*    public void ToggleDescription()
        {
            bodyText.SetActive (!bodyText.activeInHierarchy);
        }
    */
    private void Start()
    {
        tileIndex.gameObject.SetActive(true);
        tileIndex.alpha = 1.0f;
        tileDetails.gameObject.SetActive(false);
        tileDetails.alpha = 0.0f;
    }

    public void SetTileDetails()
    {
        //Set the tile details up from the button clicked reference
        GameObject tileButton = EventSystem.current.currentSelectedGameObject;
        string tileTitle = tileButton.GetComponentInChildren<Text>().text;
        string tileBody = "UI_TileRefDetails"+tileButton.name;
        Sprite tileIcon = tileButton.GetComponentInChildren<Image>().sprite;

        tileDetailsTitle.text = tileTitle;
        tileDetailsBody.text = LocManager.instance.GetLocText(tileBody);
        tileDetailsIcon.sprite = tileIcon;

        ToggleDescription(false);
    }
    
    public void ToggleDescription(bool fadeToTileRef)
    {
        StartCoroutine(FadeMenus(fadeToTileRef, Time.time, 0.5f));
    }

    IEnumerator FadeMenus(bool fadeToTileRef, float startTime,  float duration)
    {
        //Set the canvasgroups to fade between
        CanvasGroup toCanvasGroup = tileDetails;
        CanvasGroup fromCanvasGroup = tileIndex;

        if (fadeToTileRef)
        {
            fromCanvasGroup = tileDetails;
            toCanvasGroup = tileIndex;
        }

        //Activate the target gameobject to fade to
        toCanvasGroup.gameObject.SetActive(true);

        //If fading to the tile details reset the vertical scroll to the top
        if (toCanvasGroup == tileDetails)
            tileDetailsScrollRect.verticalNormalizedPosition = 1.0f;

        //Begin the fade
        while (toCanvasGroup.alpha < 1.0f)
        {
            toCanvasGroup.alpha = Mathf.SmoothStep(0.0f, 1.0f, (Time.time - startTime) / duration);
            fromCanvasGroup.alpha = Mathf.SmoothStep(1.0f, 0.0f, (Time.time - startTime) / duration);
            yield return null;
        }

        //Turn of the canvasgroup just faded from
        fromCanvasGroup.gameObject.SetActive(false);
        yield return null;
    }
}