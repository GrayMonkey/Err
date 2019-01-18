using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// This class is based around the online training for modal dialogs

public class ModalDialog : MonoBehaviour
{
    public Text title;
    public Text body;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button buttonCancel;

    [SerializeField] private GameObject dialogObject;
    [SerializeField] private GameObject dialogBG;

    private static ModalDialog modalDialog;

    public static ModalDialog Instance()
    {
        if (!modalDialog)
        {
            modalDialog = FindObjectOfType<ModalDialog>();
            if (!modalDialog)
                Debug.LogError("No ModalDialog found in scene.");
        }

        return modalDialog;
    }

    public void Show(ModalDialogDetails details)
    {
        dialogBG.gameObject.SetActive(true);
        dialogObject.SetActive(true);

        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);

        title.text = details.title;
        body.text = details.body;

        if (details.button1Details == null)
        {
            Debug.LogError("ModalDialog needs at least one button!");
            return;
        }
        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(details.button1Details.action);
        button1.onClick.AddListener(CloseDialog);
        button1.image.sprite = details.button1Details.icon;
        button1.gameObject.SetActive(true);

        if (details.button2Details != null)
        {
            button2.onClick.RemoveAllListeners();
            button2.onClick.AddListener(details.button2Details.action);
            button2.onClick.AddListener(CloseDialog);
            button2.image.sprite = details.button2Details.icon;
            button2.gameObject.SetActive(true);
        }

        if (details.button3Details != null)
        {
            button3.onClick.RemoveAllListeners();
            button3.onClick.AddListener(CloseDialog);
            button3.image.sprite = details.button3Details.icon;
            button3.gameObject.SetActive(true);
        }

        if (details.buttonCanceldetails != null)
        {
            buttonCancel.onClick.RemoveAllListeners();
            buttonCancel.onClick.AddListener(CloseDialog);
            buttonCancel.image.sprite = details.buttonCanceldetails.icon;
            buttonCancel.gameObject.SetActive(true);
        }
    }

    public void CloseDialog()
    {
        dialogObject.SetActive(false);
        dialogBG.gameObject.SetActive(false);
    }
}

// Allows button details to be individually set up
public class ButtonDetails
{
    public Sprite icon;
    public UnityAction action; 
}

// Allows dialog details to be individually set up
public class ModalDialogDetails
{
    public string title;
    public string body;
    public Sprite backgroundImage;
    public ButtonDetails button1Details;
    public ButtonDetails button2Details;
    public ButtonDetails button3Details;
    public ButtonDetails buttonCanceldetails;
}