using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Extends the toggle class and overrides some of the pointer events
// to create a contextual help when the button is held for touchTime

public class ContextToggle : Toggle
{
    public GameObject helpToggle;
    public string keyTitle;
    public string keyDetails;
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
        HelpPanel helpPanel = helpToggle.GetComponent<HelpPanel>();
        Sprite helpSprite = helpPanel.helpImage.sprite;

        helpPanel.helpImage.sprite = this.image.sprite;
        helpPanel.keyTitle.text = LocManager.instance.GetLocText(keyTitle);
        helpPanel.keyDetails.text = LocManager.instance.GetLocText(keyDetails);
        helpPanel.gameObject.SetActive(true);

        //Debug.Log("Image: " + helpButtonPanel.helpImage.sprite.name);
    }
}