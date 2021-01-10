using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CardSetSelect : MonoBehaviour
{
    [SerializeField] Toggle[] langFlags = default;
    [SerializeField] OnOffSlider onOffSlider;
    [SerializeField] Text txtCSTitle;
    [SerializeField] Text txtCSDesc;
    [SerializeField] Button btnCSCost;
    [SerializeField] HorizontalScrollSnap hss;
    [SerializeField] Toggle pageToggle;
    [SerializeField] Transform cardSetsHolder;
    [SerializeField] GameObject freePurchaseScreen;

    private Player refPlayer;
    private CardSet[] allCardSets;
    private List<CardSet> csActive = new List<CardSet>();
    private CardSetManager csManager;
    private LocManager locManager;
    private bool showFreeCardsetMessage = true;
    private Langs activeLangs = new Langs();
    private Toggle[] togPages;

    private void Awake()
    {
        csManager = CardSetManager.csManager;
        locManager = LocManager.locManager;

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

 /*   public void SwitchCardSets()
    {
        if (onOffSlider.value == 0)
        {
            labelGame.color = Color.black;
            labelGame.fontStyle = FontStyle.Normal;

            labelShop.color = Color.gray;
            labelShop.fontStyle = FontStyle.Italic;
        }
        else
        {
            labelGame.color = Color.gray;
            labelGame.fontStyle = FontStyle.Italic;

            labelShop.color = Color.black;
            labelShop.fontStyle = FontStyle.Normal;
        }

        DisplayCardSets();
    }
*/
    public void DisplayCardSets()
    {
        // remove all the active cardsets, if any
        RectTransform hssContent = hss.GetComponent<ScrollRect>().content;
        if (hssContent.childCount > 0)
        {
            hss.RemoveAllChildren(out GameObject[] childrenRemoved);
            csActive.Clear();

            foreach (GameObject go in childrenRemoved)
            {
                go.transform.SetParent(cardSetsHolder);
                go.transform.localPosition = new Vector3(0, 0, 0);
            }
        }

        // Check to see if the cardsets meeting the current display criteria
        foreach (CardSet cardSet in csManager.csAll)
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
                csActive.Add(cardSet);
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
        SetCardSetInfo(true);
    }

    public void SetCardSetInfo(bool setInfo)
    {
        if(!setInfo)
        {
            txtCSTitle.text = "";
            txtCSDesc.text = "";
            btnCSCost.gameObject.SetActive(false);
            return;
        }
        else
        {
            CardSet[] content = hss.GetComponent<ScrollRect>().content.GetComponentsInChildren<CardSet>();
            CardSet csActive = content[hss.CurrentPage];

            txtCSTitle.text = locManager.GetLocText(csActive.cardSetTitleKey);
            txtCSDesc.text = locManager.GetLocText(csActive.cardSetDescKey);
            btnCSCost.gameObject.SetActive(true);
            Text btnText = btnCSCost.GetComponentInChildren<Text>();

            if(csActive.purchased)
            {
                btnText.text = "Select";
            } else
            {
                btnText.text = "£9.99";
            }
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

    public void SetCardSets(Player player)
    {
        List<CardSet> newCardSets = new List<CardSet>();

        foreach (CardSet cardSet in allCardSets)
        {
            if (cardSet.GetComponent<Toggle>().isOn)
                newCardSets.Add(cardSet);
        }

        // Set the CardSet if thisPlayer is set and then reset thisPlayer
        // to avoid overwriting. If no refPLayer then set as default
        // cardset

        if (refPlayer != null)
        {
            player.cardSets = newCardSets;
        }
        else
        {
            GameManager.gameManager.defaultCardSets = newCardSets;
        }

        refPlayer = null;
    }
}