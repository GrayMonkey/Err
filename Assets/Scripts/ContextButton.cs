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
    private float touchTime = 1f;
    private bool eligibleForClick = true;

    //[Header("Buttons")]
    //bool btnOK;
    //bool btnCancel;

    public override void OnPointerDown(PointerEventData eventData)
    {
//        Debug.Log("Button clicked!");
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
        HelpButtonPanel helpButtonPanel = helpButton.GetComponent<HelpButtonPanel>();
        Sprite helpSprite = helpButtonPanel.helpImage.sprite;

        helpButtonPanel.helpImage.sprite = this.image.sprite;
        helpButtonPanel.keyTitle.text = LocManager.locManager.GetLocText(keyTitle);
        helpButtonPanel.keyDetails.text = LocManager.locManager.GetLocText(keyDetails);
        helpButtonPanel.gameObject.SetActive(true);

        //Debug.Log("Image: " + helpButtonPanel.helpImage.sprite.name);
    }
}