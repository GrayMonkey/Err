using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CardSetSelect : MonoBehaviour
{
    [SerializeField] Toggle[] langFlags = default;
    // [SerializeField] OnOffSlider onOffSlider;
    [SerializeField] GameObject[] csFlags;
    [SerializeField] Text txtCSTitle;
    [SerializeField] Text txtCSDesc;
    [SerializeField] Image imgSelected;
    [SerializeField] GameObject btnCSCost;
    [SerializeField] HorizontalScrollSnap hss;
    //[SerializeField] Toggle pageToggle;
    [SerializeField] Transform cardSetsHolder;
    [SerializeField] Text txtPurchaseButton;

    private Player refPlayer;
    private List<CardSet> csActiveList = new List<CardSet>();
    private QuestionManager questionManager;
    private LocManager locManager;
    //private IAPManager iapManager;
    private CardSet csActive;
    //private bool showFreeCardsetMessage = true;
    private Langs activeLangs = new Langs();
    private Toggle[] togPages;

    private void Awake()
    {
        questionManager = QuestionManager.instance;
        locManager = LocManager.instance;
        //iapManager = IAPManager.instance;

        // Add the system language to the active languages
        // If the system language isn't supported then default to English
        switch (locManager.GameLang)
        {
            case SystemLanguage.French:
                activeLangs.french = true;
                break;
            case SystemLanguage.German:
                activeLangs.german = true;
                break;
            case SystemLanguage.Italian:
                activeLangs.italian = true;
                break;
            case SystemLanguage.Spanish:
                activeLangs.spanish = true;
                break;
            default:
                activeLangs.english = true;
                break;
        }
    }

    private void Start()
    {
        togPages = hss.Pagination.GetComponentsInChildren<Toggle>();
        SetLangFlags();
        DisplayCardSets();
    }

    public void DisplayCardSets()
    {
        // remove all the active cardsets, if any
        RectTransform hssContent = hss.GetComponent<ScrollRect>().content;
        if (hssContent.childCount > 0)
        {
            hss.RemoveAllChildren(out GameObject[] childrenRemoved);
            csActiveList.Clear();

            foreach (GameObject go in childrenRemoved)
            {
                go.transform.SetParent(cardSetsHolder);
                go.transform.localPosition = new Vector3(0, 0, 0);
            }
        }

        // Check to see if the cardsets meeting the current display criteria
        foreach (CardSet cardSet in questionManager.csAll)
        {
            bool display = false;

            // Check for languages if the languages are selected
            if(cardSet.langs.english && activeLangs.english ||
                cardSet.langs.french && activeLangs.french ||
                cardSet.langs.german && activeLangs.german ||
                cardSet.langs.italian && activeLangs.italian ||
                cardSet.langs.spanish && activeLangs.spanish)
                display = true;

            // display the gameObject accordingly
            if (display)
            {
                hss.AddChild(cardSet.gameObject);
                csActiveList.Add(cardSet);
            }
        }

        // Set up the pagination toggles to be displayed
        for (int i = 0; i < togPages.Length; i++)
            if(i < hssContent.GetComponentsInChildren<CardSet>().Length)
                togPages[i].gameObject.SetActive(true);
            else
                togPages[i].gameObject.SetActive(false);

        // Remove the pagination if only one item is present
        if (hssContent.childCount == 1)
            togPages[0].gameObject.SetActive(false);

        hss.UpdateLayout();
        // Resize objects in hss as these are scaling on iPhone each time
        // the language selection is hit
        foreach (GameObject gameObject in hss.ChildObjects)
        {
            gameObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        }

        SetCardSetInfo(true);
    }

    public void SetCardSetInfo(bool setInfo)
    {

        if (!setInfo)
        {
            csFlags[0].SetActive(false);
            csFlags[1].SetActive(false);
            csFlags[2].SetActive(false);
            csFlags[3].SetActive(false);
            csFlags[4].SetActive(false);
            txtCSTitle.text = "";
            txtCSDesc.text = "";
            btnCSCost.SetActive(false);
            return;
        }
        else
        {
            CardSet[] content = hss.GetComponent<ScrollRect>().content.GetComponentsInChildren<CardSet>();
            csActive = content[hss.CurrentPage];
            Toggle csToggle = csActive.GetComponent<Toggle>();

            // Set the langs supported by the card set
            csFlags[0].SetActive(csActive.langs.english);
            csFlags[1].SetActive(csActive.langs.french);
            csFlags[2].SetActive(csActive.langs.german);
            csFlags[3].SetActive(csActive.langs.italian);
            csFlags[4].SetActive(csActive.langs.spanish);

            // Set the remaining CardSet info
            txtCSTitle.text = locManager.GetLocText(csActive.cardSetTitleKey);
            txtCSDesc.text = locManager.GetLocText(csActive.cardSetDescKey);
            btnCSCost.gameObject.SetActive(true);

            // Switch the purchase button on if csActive is not purchased
            //btnCSCost.GetComponentInChildren<Text>().text = "GetFromStore"; // TODO: Update from the store
            //btnCSCost.SetActive(!csActive.purchased);
            //csActive.CheckPurchaseFromStore();
            txtPurchaseButton.text = locManager.GetLocText("str_Unselect");
            PurchaseSelectButton(true);
        }
    }

    private void SetLangFlags()
    {
        foreach (Toggle flag in langFlags)
        {
            switch (flag.name)
            {
                case "French":
                    flag.isOn = activeLangs.french;
                    break;
                case "German":
                    flag.isOn = activeLangs.german;
                    break;
                case "Italian":
                    flag.isOn = activeLangs.italian;
                    break;
                case "Spanish":
                    flag.isOn = activeLangs.spanish;
                    break;
                default:
                    flag.isOn = activeLangs.english;
                    break;
            }
        }
    }

    public void LangToggle()
    {
        // What was the country flag that was just toggled
        Toggle lastFlag = EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();

        // Set languages according to the flag toggle and also check that at least one language is selected
        bool noLang = true;
        Langs newLangs = new Langs();

        foreach (Toggle flag in langFlags)
        {
            if (flag.isOn)
                noLang = false;

            switch (flag.name)
            {
                case "English":
                    newLangs.english = flag.isOn;
                    break;
                case "French":
                    newLangs.french = flag.isOn;
                    break;
                case "German":
                    newLangs.german = flag.isOn;
                    break;
                case "Italian":
                    newLangs.italian = flag.isOn;
                    break;
                case "Spanish":
                    newLangs.spanish = flag.isOn;
                    break;
                default:
                    break;
            }
        }

        if (noLang)
        {
            SetLangFlags();
            return;
        }

        activeLangs = newLangs;
        DisplayCardSets();
    }

    public void SetCardSetSelected(bool selected)
    {
        imgSelected.gameObject.SetActive(selected);
    }

    public void PurchaseCardSet()
    {
        string productID = csActive.cardSetProductID;
        //iapManager.BuyProduct(productID);
    }

    public void PurchaseSelectButton (bool setup)
    {
        List<CardSet> playableCardSets = questionManager.playableCardSets;
        if (csActive.purchased)
        {
            if (playableCardSets.Contains(csActive))
            {
                txtPurchaseButton.text = locManager.GetLocText("str_Unselect");
                if (!setup)
                {
                    txtPurchaseButton.text = locManager.GetLocText("str_Select");
                    playableCardSets.Remove(csActive);
                }
            }
            else
            {
                txtPurchaseButton.text = locManager.GetLocText("str_Select");
                if(!setup)
                {
                    txtPurchaseButton.text = locManager.GetLocText("str_Unselect");
                    playableCardSets.Add(csActive);
                }
            }
        }
        else
        {
            // TODO Add in actual purchasing
            // Debug, currently CardSets are added automatically when purchased
            if (setup)
            {
                txtPurchaseButton.text = "£2.49";
                return;
            }

            txtPurchaseButton.text = locManager.GetLocText("str_Unselect");
            csActive.purchased = true;
            playableCardSets.Add(csActive);
        }

        csActive.selectedIcon.SetActive(playableCardSets.Contains(csActive));
        questionManager.playableCardSets = playableCardSets;



        /*
                if (txtPurchaseButton.text == locManager.GetLocText("str_Unselect"))
                {
                    if (csInPlay.Contains(csActive))
                        csInPlay.Remove(csActive);
                    csActive.selectedIcon.SetActive(false);
                    txtPurchaseButton.text = locManager.GetLocText("str_Select");
                }
                else
                {
                    if(!csInPlay.Contains(csActive))
                        csInPlay.Add(csActive);
                    csActive.selectedIcon.SetActive(true);
                    txtPurchaseButton.text = locManager.GetLocText("str_Unselect");
                }*/
    }

    /*    public void CardSetPurchaseOrSelect()
        {
            // ToDo: Link this to actual purchasing
            CardSet _csPurchase = hss.CurrentPageObject().GetComponent<CardSet>();

            if (_csPurchase == null)
            {
                Debug.Log("Purchase failed");
                return;
            }

            if (_csPurchase.purchased)
            {
                Toggle _toggle = _csPurchase.GetComponent<Toggle>();
                _toggle.isOn = !_toggle.isOn;

                if (_toggle.isOn)
                    btnPurchase.text = locManager.GetLocText("str_Unselect");
                else
                    btnPurchase.text = locManager.GetLocText("str_Select");
            }
            else
            {
                string _purchase = _csPurchase.cardSetTitleKey;
                string _purchaseID = _csPurchase.cardSetProductID;
                _purchaseID = " (" + _purchaseID + ")";

                Debug.Log("Purchasing: " + _purchase + _purchaseID);
                _csPurchase.purchased = true;
            }

        }*/

}