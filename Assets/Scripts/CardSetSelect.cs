﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetSelect : MonoBehaviour
{
    [SerializeField] List<GameObject> allCardSets = default;
    [SerializeField] GameObject[] langFlags = default;
    [SerializeField] GameObject btnShopCardsets = default;
    [SerializeField] GameObject btnGameCardsets = default;
    [SerializeField] Sprite btnOK = default;
    [SerializeField] GameObject csGame = default;
    [SerializeField] GameObject csShop = default;
    [SerializeField] Text labelGameShop = default;

    private GameManager gameManager;
    private Player refPlayer;
    private List<CardSet> selectedCardSets = new List<CardSet>();
    private List<string> activeLanguages = new List<string>();
    private CardSetManager csManager;
    private LocManager locManager;
    private ModalDialog dialogNoLangs;

    private void OnEnable()
    {
        gameManager = GameManager.gameManager;
        csManager = CardSetManager.csManager;
        locManager = LocManager.locManager;
        dialogNoLangs = ModalDialog.Instance();

        InitialiseLangFlags();
        SortCardSets();
    }

    private void InitialiseLangFlags()
    {
        string lang = locManager.GameLang.ToString();
        activeLanguages.Clear();
        activeLanguages.Add(lang);

        foreach (GameObject flag in langFlags)
        {
            flag.GetComponent<Toggle>().isOn = false;

            if (activeLanguages.Contains(flag.name))
                flag.GetComponent<Toggle>().isOn = true;
        }
    }

    public void SortCardSets()
    {
        foreach (GameObject cardSet in allCardSets)
        {
            cardSet.transform.SetParent(csShop.transform);

            if (cardSet.GetComponent<CardSet>().purchased)
                cardSet.transform.SetParent(csGame.transform);
        }
        ShowShop(false);
    }

    public void ShowShop(bool showShop)
    {
        btnGameCardsets.SetActive(false);
        btnShopCardsets.SetActive(true);
        csGame.SetActive(true);
        csShop.SetActive(false);

        labelGameShop.text = locManager.GetLocText("str_CardSetsGame");

        if (showShop)
        {
            btnGameCardsets.SetActive(true);
            btnShopCardsets.SetActive(false);
            csGame.SetActive(false);
            csShop.SetActive(true);

            labelGameShop.text = locManager.GetLocText("str_CardSetsShop");
        }
    }

    public void FilterLanguages()
    {
        //Check for any languages that are toggled on
        List<string> newLangs = new List<string>();
        foreach (GameObject flag in langFlags)
        {
            if (flag.GetComponent<Toggle>().isOn)
                newLangs.Add(flag.name);
        }

        //If newLangs is empty send player message and quit
        if (newLangs.Count == 0)
        {
            ShowDialog();
            return;
        }

        //If newLangs is not empty then filter cardsets
        foreach (GameObject cardSet in allCardSets)
        {
            cardSet.SetActive(false);

            foreach (string lang in newLangs)
            {
                switch (lang)
                {
                    case "English":
                        if (cardSet.GetComponent<CardSet>().english)
                            cardSet.SetActive(true);
                        break;

                    case "French":
                        if (cardSet.GetComponent<CardSet>().french)
                            cardSet.SetActive(true);
                        break;

                    case "German":
                        if (cardSet.GetComponent<CardSet>().german)
                            cardSet.SetActive(true);
                        break;

                    case "Italian":
                        if (cardSet.GetComponent<CardSet>().italian)
                            cardSet.SetActive(true);
                        break;

                    case "Spanish":
                        if (cardSet.GetComponent<CardSet>().spanish)
                            cardSet.SetActive(true);
                        break;

                    case "default":
                        break;

                }
            }
        }


        // Update activeLanguages
        activeLanguages.Clear();
        activeLanguages = newLangs;
    }

    public void SetCardSets(Player player)
    {
        List<CardSet> newCardSets = new List<CardSet>();

        foreach (GameObject cardSet in allCardSets)
        {
            if (cardSet.GetComponent<Toggle>().isOn)
                newCardSets.Add(cardSet.GetComponent<CardSet>());
        }

        // Set the CardSet if refPlayer is set (and then reset refPlayer
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

    private void ShowDialog()
    {
        ModalDialogDetails details = new ModalDialogDetails();
        ButtonDetails button1 = new ButtonDetails
        {
            icon = btnOK,
            action = CloseDialog
        };

        details.button1Details = null;
        details.title = locManager.GetLocText("str_DialogTitleWarning");
        details.body = locManager.GetLocText("str_DialogNoLanguage");

        dialogNoLangs.Show(details);
    }

    private void CloseDialog()
    {
        // Reset the flags and use the existing activeLanguages
        foreach (string langName in activeLanguages)
        {
            foreach (GameObject langFlag in langFlags)
            {
                if (langFlag.name == langName)
                {
                    langFlag.GetComponent<Toggle>().isOn = true;
                }
            }
        }

        // Close the dialog box
        dialogNoLangs.CloseDialog();
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
            selectedCardSets.Clear();
            foreach (Toggle cardSet in cardSets)
            {
                if (cardSet.isOn && cardSet.gameObject.activeInHierarchy)
                    selectedCardSets.Add(cardSet.GetComponent<CardSet>());
            }

            if (refPlayer == null)
            {
                gameManager.defaultCardSets = selectedCardSets;
            }
            else
            {
                refPlayer.cardSets = selectedCardSets;
            }

            // Return to the GameManager and select players
            // gameManager.UpdateGameState(GameManager.GameState.Players);
            gameManager.SetGameState(gameManager.gameState.playerSelect);
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

                }
            }
            SortCardSets();
        }
    }
}