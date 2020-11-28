using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSetSelect : MonoBehaviour
{
    [SerializeField] Toggle[] langFlags = default;
    [SerializeField] GameObject btnShopCardsets = default;
    [SerializeField] GameObject btnGameCardsets = default;
    //[SerializeField] Sprite btnOK = default;
    [SerializeField] GameObject freePurchaseScreen;
    [SerializeField] GameObject csGame = default;
    [SerializeField] GameObject csShop = default;
    [SerializeField] Text labelGameShop = default;

    private GameManager gameManager;
    private Player refPlayer;
    private List<CardSet> activeCardSets = new List<CardSet>();
    private CardSet[] allCardSets;
    private CardSetManager csManager;
    private LocManager locManager;
    private bool showFreeCardsetMessage = true;
    private Langs activeLangs = new Langs();

    private void Awake()
    {
        gameManager = GameManager.gameManager;
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
    
    private void OnEnable()
    {
        allCardSets = csManager.allCardSets;
        SetLangFlags();
        SortCardSets();
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

    public void SortCardSets()
    {
        List<CardSet> filteredCardSets = new List<CardSet>();
        bool freeCardSet = true;

        // Turn off CardSets that aren't relevant for the selected languages
        foreach (CardSet cardset in allCardSets)
        {
            cardset.gameObject.SetActive(false);

            if (cardset.langs.english && activeLangs.english ||
                cardset.langs.french && activeLangs.french ||
                cardset.langs.german && activeLangs.german ||
                cardset.langs.italian && activeLangs.italian ||
                cardset.langs.spanish && activeLangs.spanish)
            {
                cardset.gameObject.SetActive(true);
                filteredCardSets.Add(cardset);

                // Sort the cardset in to the correct store
                cardset.transform.SetParent(csShop.transform);

                if (cardset.purchased)
                {
                    cardset.transform.SetParent(csGame.transform);
                    freeCardSet = false;
                }
            }
        }

        // Only allow Free Purchase cardsets if no cardsets have been purchased
        if (freeCardSet)
        {
            foreach (CardSet freecardset in filteredCardSets)
            {
                if (!freecardset.freePurchase)
                    freecardset.gameObject.SetActive(false);
            }

            if (showFreeCardsetMessage)
            {
                freePurchaseScreen.SetActive(true);
                showFreeCardsetMessage = false;
            }
        }

    }
    
    public void ShowShop(bool showShop)
    {
        btnGameCardsets.SetActive(false);
        btnShopCardsets.SetActive(true);
        csGame.SetActive(true);
        csShop.SetActive(false);

        string _labelGameShop;
        string _player;

        _labelGameShop = locManager.GetLocText("str_CardSetsGame");

        if (refPlayer == null)
        {
            _player = locManager.GetLocText("str_AllPlayers");
        }
        else
        {
            _player = refPlayer.playerName;
        }

        _labelGameShop += " " + _player;

        labelGameShop.text = _labelGameShop;

        if (showShop)
        {
            btnGameCardsets.SetActive(true);
            btnShopCardsets.SetActive(false);
            csGame.SetActive(false);
            csShop.SetActive(true);

            labelGameShop.text = locManager.GetLocText("str_CardSetsShop");
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
        SortCardSets();
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

    // The Confirm button has two different roles depending on
    // if the player has the GameCardSets or ShopCardSets active
    public void ConfirmButton()
    {
        // If the GameCardsets is active then add the selected
        // Cardsets to selectedCardSets
        if (csGame.activeInHierarchy)
        {
            Toggle[] cardSets = csGame.GetComponentsInChildren<Toggle>();
            activeCardSets.Clear();
            foreach (Toggle cardSet in cardSets)
            {
                if (cardSet.isOn && cardSet.gameObject.activeInHierarchy)
                    activeCardSets.Add(cardSet.GetComponent<CardSet>());
            }

            // Set the default card set or player card set and return to 
            // the Home screen or player select...
            if (refPlayer == null)
            {
                gameManager.defaultCardSets = activeCardSets;
                gameManager.SetGameState(gameManager.gameState.home);
            }
            else
            {
                refPlayer.cardSets = activeCardSets;
                gameManager.SetGameState(gameManager.gameState.playerSelect);
            }
        }

        // Otherwise purchase the selected Cardsets and add them
        // to the GameCardSets
        else
        {
            Toggle[] cardSets = csShop.GetComponentsInChildren<Toggle>();
            foreach (Toggle cardSet in cardSets)
            {
                if(cardSet.isOn && cardSet.gameObject.activeInHierarchy)
                {
                    // TODO Add purchases in to the platform store
                    Debug.Log("Need to activate purchasing of cards!!!");
                    cardSet.GetComponent<CardSet>().purchased = true;

                    // If there are no default cardsets then added the recently
                    // purchase cardset as the default cardset
                    
                }
            }
            SortCardSets();
        }
    }

    public void CloseFreeCardSetScreen()
    {
        freePurchaseScreen.SetActive(false);
        ShowShop(true);
    }
}