using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Extends the button class and overrides some of the pointer events
// to create a contextual help when the button is held for touchTime

public class ContextButton : Button
{
    public GameObject helpButton;
    public string keyTitle;
    public string keyDetails;
    public Image altImage;
    private float touchTime = 1f;
    //private bool eligibleForClick = true;

    //[Header("Buttons")]
    //bool btnOK;
    //bool btnCancel;

    public override void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Button clicked!");
        Invoke("ShowHelp", touchTime);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Button released");
        CancelInvoke("ShowHelp");
    }

    void ShowHelp()
    {
        //Debug.Log("Showing help");
        HelpPanel helpPanel = helpButton.GetComponent<HelpPanel>();
       
        helpPanel.keyTitle.text = LocManager.instance.GetLocText(keyTitle);
        helpPanel.keyDetails.text = LocManager.instance.GetLocText(keyDetails);

        // If altIamge is used then override helpSprite
        if (altImage)
        {
            helpPanel.helpImage.sprite = altImage.sprite;
        }
        else
        {
            helpPanel.helpImage.sprite = this.image.sprite;
        }

        helpPanel.gameObject.SetActive(true);
        //Debug.Log("Image: " + helpButtonPanel.helpImage.sprite.name);
    }
}