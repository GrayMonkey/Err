﻿#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Menu = MenuHandler.MenuOverlay;
using UnityEngine.Events;
using System;
using JetBrains.Annotations;

public class PlayerObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    // TODO: Still some tidying up of this script, need to remove unneeded variables.

    // Main PlayerObject variables
    public Player refPlayer;
    public bool subMenuOpen = false;

    [SerializeField] private Text playerName;
    [SerializeField] private Image playerNameBG;
    [SerializeField] private Text playerID;
    [SerializeField] private Text dummyPlayerName;
    [SerializeField] private Text dummyPlayerID;
    [SerializeField] private Image playerIDBG;
    [SerializeField] private Image playerIDOL;
    [SerializeField] private GameObject dummyPlayer;
    [SerializeField] private GameObject addRosterPlayerButton;
    [SerializeField] private GameObject moreButton;
    [SerializeField] private GameObject activePlayerHolder;
    [SerializeField] private GameObject rosterPlayerHolder;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button trashPlayer;
    [SerializeField] private Button[] subMenuButtons;
    [SerializeField] private Sprite btnReturn;
    [SerializeField] private Sprite btnTrash;
    [SerializeField] private Sprite btnCancel;

    //GameObject dummyPlayer; // to mimic the playerDragged object and also last in heirarchy so draws over other PlayerObjects
    //GameObject helpButton;
    RectTransform activePlayerRectTransform;
    ModalDialog dialogDeletePlayer;
    //PlayerRosterSelect playerRosterSelect;
    PlayerSelector playerSelector;
    PlayerController playerController;
    MenuHandler uiMenus;
    float lastTap = 0f;
    float delayTap = 0.25f;
    float startTime;
    float clickOffset;
    bool active = false;
    bool uniqueName;
    Color darkGreen = new Color(0.0f, 0.9f, 0.0f);
    Color hilightGreen = new Color(0.7f, 1.0f, 0.4f);
    Color lightRed = new Color(1.0f, 0.35f, 0.35f);
    Color hilightRed = new Color(1.0f, 0.6f, 0.6f);
    Color normal = new Color(1.0f, 1.0f, 1.0f);
    string locText;

    // SubMenus variables
    public Text subMenuInfoText;

    [SerializeField] Animator animator;
    [SerializeField] ScrollRect subMenuRect;
    [SerializeField] GameObject[] mnu_SubMenus;

    enum SubMenu { PlayerRoster, CardSets, RemovePlayer };

    private void Awake()
    {
        uiMenus = MenuHandler.uiMenus;
        //helpButton = uiMenus.helpButton;
        playerSelector = PlayerSelector.playerSelector;
        playerController = PlayerController.playerController;
        //playerRosterSelect = PlayerRosterSelect.playerRosterSelect;
        activePlayerRectTransform = activePlayerHolder.GetComponent<RectTransform>();
    }

    private void Start()
    {
        //refPlayer = playerController.activePlayer;
        //playerName.text = refPlayer.playerName;
        //playerID.text = refPlayer.playerID;
        //dummyPlayer = playerSelector.dummyPlayer;
        //gameObject.name = refPlayer.playerName;
        //SetContextButton();
    }

    private void SetContextButton()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        bool rosterPlayer = parent.name.Equals("Roster Player Holder");

        addRosterPlayerButton.SetActive(rosterPlayer);
        moreButton.SetActive(!rosterPlayer);
        active = !rosterPlayer;
    }

    public void SetPlayer(Player player)
    {
        playerName.text = player.playerName;
        playerID.text = player.playerID;
        gameObject.name = player.playerName;
        SetContextButton();
    }

    #region Player handling

    //public void AddToPlayerList()
    //{
    //    if (this.gameObject.activeSelf)
    //    {
    //        playerController.playersActive.Add(refPlayer);
    //    }
    //}

    public void UpdatePlayerName(bool endEdit)
    {
        // Only need to do this if the input field is active fixes
        // bug whereby this function is called when endEdit is true
        if (!nameInputField.IsActive() || active)
            return;

        string newName = nameInputField.text;
        bool goodName = playerController.UniqueNameCheck(newName, refPlayer);
        buttonImage.color = hilightRed;

        // check if the new name is blank and if so reset it to the original name
        // or if the new name is not unique
        if (goodName)
        {
            buttonImage.color = hilightGreen;

            // Set the new ID tag
            playerID.fontStyle = FontStyle.BoldAndItalic;
            int length = Math.Min(newName.Length, 2);           // In case the name only has one character
            playerID.text = newName.Substring(0, length);

            if (newName.Contains(" "))
            {
                int index = newName.IndexOf(" ") + 1;
                playerID.text = newName.Substring(0, 1) + newName.Substring(index, 1);
            }
        }

        if (endEdit)
        {
            nameInputField.gameObject.SetActive(false);
            playerName.gameObject.SetActive(true);
            playerID.fontStyle = FontStyle.Bold;
            buttonImage.color = Color.white;
            nameInputField.text = "";

            if (goodName)
            {
                refPlayer.playerName = newName;
                refPlayer.playerID = playerID.text;
                playerName.text = newName;
            }
        }
    }

    public void AddRosterPlayerToGame()
    {
        gameObject.transform.SetParent(activePlayerHolder.transform);
        //gameObject.transform.parent = activePlayerHolder.transform;
        //playerSelector.UpdatePlayerList();
        SetContextButton();
    }

    public void UpdateToRosterPlayer(Player rosterPlayer)
    {
        throw new NotImplementedException();
        //int index = playerController.playersActive.IndexOf(refPlayer);
        //playerController.playersActive.Insert(index, rosterPlayer);
        //playerController.playersActive.Remove(refPlayer);

        //refPlayer = rosterPlayer;
        //playerName.text = refPlayer.playerName;
        //playerController.activePlayer = refPlayer;
    }
    #endregion

    #region Clicking and dragging functions

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (active == false)
            return;

        playerController.activePlayer = refPlayer;
        playerSelector.playerDragged = gameObject;
        GetComponent<BoxCollider2D>().isTrigger = true;

        // Turn on the dummyplayer and set it to the position of the playerobject
        dummyPlayerName.text = playerName.text;
        dummyPlayerID.text = playerID.text;
        dummyPlayer.SetActive(true);
        dummyPlayer.transform.localPosition = gameObject.transform.localPosition;

        // Set the delta between the the gameObejct position and the eventData position
        Vector2 offset = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(activePlayerRectTransform, eventData.position, null, out offset);
        clickOffset = transform.localPosition.y - offset.y;
        // Debug.Log("Dragging: " + refPlayer.playerName);
        // Debug.Log("Offset: " + clickOffset.ToString());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (active == false)
            return;
        RectTransform thisRectTransform = this.GetComponent<RectTransform>();
        Vector2 posNew = gameObject.transform.localPosition;
        Vector2 posInputLocal = new Vector2();
        Vector2 posInput = eventData.position;
        Camera camInput = null; // eventData.pressEventCamera; // seems to work better with a null camera

        // Set the lower limits of the player holder, upper limit will not change
        // bottomLimit is plus the size of the current gameObject height
        float topLimit = activePlayerRectTransform.sizeDelta.y / 2;
        float bottomLimit = -topLimit + thisRectTransform.sizeDelta.y;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(activePlayerRectTransform, posInput, camInput, out posInputLocal);
        posNew.y = Mathf.Clamp(posInputLocal.y + clickOffset, bottomLimit, topLimit);
        thisRectTransform.localPosition = posNew;
        dummyPlayer.transform.position = thisRectTransform.position;
        //Debug.Log("Object.y: " + posNew.y + " | Upper/Lower: " + topLimit.ToString() + "," + bottomLimit.ToString());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        dummyPlayer.SetActive(false);
        playerSelector.UpdatePlayerList();
        LayoutRebuilder.MarkLayoutForRebuild(GetComponentInParent<RectTransform>());
        //        Debug.Log("Dropped: " + refPlayer.playerName);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        playerController.activePlayer = refPlayer;

        // Check if another button has the sub menu open and close it by
        // forcing this player to take the focus
        playerSelector.selectedPlayer = this;

        // Double tap detected. Can't use tapcount as not supported by Android
        // Change players name
        if (lastTap + delayTap > Time.time)
        {
            if (playerSelector.selectedPlayer != this)
            {
                return;
            }
            Debug.Log("Editing player...");
            playerName.gameObject.SetActive(false);
            nameInputField.text = playerName.text;
            nameInputField.gameObject.SetActive(true);
            nameInputField.Select();
            lastTap = 0f;
        }
        else
        {
            lastTap = Time.time;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collider: " + collision.gameObject.name + " | Trigger: " + gameObject.name);

        // Change the SiblingIndex in the heirarchy to make space
        // immediately visible for playerDragged
        if (gameObject == playerSelector.playerDragged)
        {
            int colSibling = collision.transform.GetSiblingIndex();
            playerSelector.playerDragged.transform.SetSiblingIndex(colSibling);
        }
    }

    #endregion

    #region Menu functions

    // Puts the focus on this button and then opens the menu
    public void GetFocus()
    {
        /*        if (playerSelector.selectedPlayer == this)
                {
                    ShowMenu(false);
                    playerSelector.selectedPlayer = null;
                } else {
                    this.animator.enabled = true;
                    playerSelector.selectedPlayer = this;
                    ShowMenu(true);
                    if (playerController.playerRoster.Count > 0)
                    {
                        SubMenuActivate((int)SubMenu.PlayerRoster);
                    }
                    else
                    {
                        SubMenuActivate((int)SubMenu.CardSets);
                    }
                }
        */
    }

    // Open the collapsed menu
    public void ShowMenu(bool open)
    {
        animator.SetBool("openMenu", open);
    }

    // Set the active submenu
    public void SubMenuActivate(int subMenuID)
    {
        /*        playerController.activePlayer = refPlayer;

                foreach (ContextButton button in subMenuButtons)
                {
                    button.GetComponent<Image>().color = normal;
                }

                foreach(GameObject _mnu in mnu_SubMenus)
                {
                    _mnu.SetActive(false);
                }

                switch (subMenuID)
                {
                    case (int) SubMenu.CardSets:
                        mnu_SubMenus[subMenuID].SetActive(true);
                        subMenuRect.content = mnu_SubMenus[subMenuID].GetComponent<RectTransform>();
                        subMenuButtons[subMenuID].GetComponent<Image>().color = hilightGreen;
                        string locText = LocManager.locManager.GetLocText("str_SelectCardsets");
                        subMenuInfoText.text = locText + " (" + refPlayer.cardSets.Count.ToString() + ")";
                        break;            

                    case (int) SubMenu.PlayerRoster:
                        mnu_SubMenus[subMenuID].SetActive(true);
                        rectTransform = subMenuRect.content;
                        rectTransform = mnu_SubMenus[subMenuID].GetComponent<RectTransform>();
                        subMenuRect.content = rectTransform;
                        subMenuButtons[subMenuID].GetComponent<Image>().color = hilightGreen;
                        locText = LocManager.locManager.GetLocText("str_SelectPlayer");

                        if (playerController.playerRoster.Count == 0)
                            locText = LocManager.locManager.GetLocText("str_NoPlayerData");

                        subMenuInfoText.text = locText;
                        break;

                    case (int)SubMenu.RemovePlayer:
                        mnu_SubMenus[subMenuID].SetActive(true);
                        subMenuRect.content = mnu_SubMenus[subMenuID].GetComponent<RectTransform>();
                        subMenuButtons[subMenuID].GetComponent<Image>().color = hilightGreen;

                        trashPlayer.interactable = false;
                        if (playerController.playerRoster.Contains(refPlayer))
                            trashPlayer.interactable = true;

                        locText = LocManager.locManager.GetLocText("str_RemovePlayer");
                        subMenuInfoText.text = locText;
                        break;

                    default:
                        locText = LocManager.locManager.GetLocText("str_SelectOption");
                        subMenuInfoText.text = locText;
                        break;
                }
        */
    }

    public void SetLanguage(int sysLang)
    {
        refPlayer.language = LocManager.locManager.GameLang;
        locText = LocManager.locManager.GetLocText("str_PlayerLanguage");
        subMenuInfoText.text = locText + " (" + refPlayer.language.ToString() + ")";
    }

    public void DeletePlayer()
    {
        ModalDialogDetails details = new ModalDialogDetails();
        details.button1Details = new ButtonDetails();
        details.button2Details = new ButtonDetails();
        details.buttonCanceldetails = new ButtonDetails();

        locText = LocManager.locManager.GetLocText("str_DialogTitleWarning");
        details.title = locText;

        locText = LocManager.locManager.GetLocText("str_DialogPlayerDelete");

        try
        {
            locText.Replace("%%PlayerName", refPlayer.playerName);
        }
        catch
        {
            Debug.Log("str_WarningBody is missing %% PlayerName in localised text!");
        }
        details.body = locText;

        details.button1Details.action = CloseDialog;
        details.button1Details.icon = btnReturn;

        details.button2Details.action = DeletePlayerData;
        details.button2Details.icon = btnTrash;

        details.buttonCanceldetails.icon = btnCancel;

        dialogDeletePlayer.Show(details);
    }

    private void CloseDialog()
    {
        dialogDeletePlayer.CloseDialog();
    }

    // Permanently deletes the player
    private void DeletePlayerData()
    {
        if (playerController.playerRoster.Contains(refPlayer))
            playerController.playerRoster.Remove(refPlayer);

        RemovePlayer();
    }

    // Removes the player from the current game
    public void RemovePlayer()
    {
        if (playerController.playersActive.Contains(refPlayer))
            playerController.playersActive.Remove(refPlayer);
        Destroy(gameObject);
        playerSelector.ResizePlayerHolders();
    }

    IEnumerator SetScrollVPos(float vPos)
    {
        yield return null;
        subMenuRect.verticalNormalizedPosition = vPos;

    }
    #endregion
}