#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Menu = MenuHandler.MenuOverlay;

public class PlayerObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler 
{
    // Main PlayerObject variables
    public Player refPlayer;

    [SerializeField] private Text playerName;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button trashPlayer;
    [SerializeField] private Button[] subMenuButtons;
    [SerializeField] private Sprite btnReturn;
    [SerializeField] private Sprite btnTrash;
    [SerializeField] private Sprite btnCancel;

    GameObject dummyPlayer; // to mimic the playerDragged object
    GameObject helpButton;
    ModalDialog dialogDeletePlayer;
    PlayerRosterSelect playerRosterSelect;
    PlayerSelector playerSelector;
    PlayerController playerController;
    PlayerObject hasSubMenuFocus;
    MenuHandler uiMenus;
    RectTransform rectTransform;
    Vector3 newPos;
    Vector2 normSize = new Vector2(350.0f, 2.0f);
    Vector2 dragSize = new Vector2(350.0f, 20.0f);
    float lastTap = 0f;
    float delayTap = 0.25f;
    bool uniqueName;
    Color darkGreen = new Color(0.0f, 0.9f, 0.0f);
    Color hilightGreen = new Color(0.7f, 1.0f,0.4f);
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
        helpButton = uiMenus.helpButton;
        playerSelector = PlayerSelector.playerSelector;
        playerController = PlayerController.playerController;
        playerRosterSelect = PlayerRosterSelect.playerRosterSelect;
    }

    private void Start()
    {
        refPlayer = playerController.activePlayer;
        playerName.text = refPlayer.playerName;
        dummyPlayer = playerSelector.dummyPlayer;
        dialogDeletePlayer = ModalDialog.Instance();

        //Set up the ContextButton variables as this is a Prefab
        foreach (ContextButton button in subMenuButtons)
        {
            button.helpButton = helpButton;
        }
   }

    private void Update()
    {
        if (playerSelector.selectedPlayer != this)
        {
            ShowMenu(false);
            AnimatorClipInfo[] animatorClips = this.animator.GetCurrentAnimatorClipInfo(0);
            if (animatorClips.Length == 0)
            {
                animator.enabled = false;
            }
        }
    }

    #region Player handling

    public void AddToPlayerList()
    {
        if (this.gameObject.activeSelf)
        {
            playerController.playersActive.Add(refPlayer);
        }
    }

    public void UpdatePlayerName(bool endEdit)
    {
        // Only need to do this if the input field is active fixes
        // bug whereby this function is called when endEdit is true
        if (!nameInputField.IsActive()) { return; }

        string newName = nameInputField.text;
        bool goodName = playerController.UniqueNameCheck(newName, refPlayer);
        buttonImage.color = hilightRed;

        // check if the new name is blank and if so reset it to the original name
        // or if the new name is not unique
        if (goodName)
        {
            buttonImage.color = hilightGreen;
        }

        if (endEdit)
        {
            nameInputField.gameObject.SetActive(false);
            playerName.gameObject.SetActive(true);
            buttonImage.color = Color.white;
            nameInputField.text = "";

            if (goodName)
            {
                refPlayer.playerName = newName;
                playerName.text = newName;
            }
        }
    }

    public void UpdateToRosterPlayer(Player rosterPlayer)
    {
        int index = playerController.playersActive.IndexOf(refPlayer);
        playerController.playersActive.Insert(index, rosterPlayer);
        playerController.playersActive.Remove(refPlayer);

        refPlayer = rosterPlayer;
        playerName.text = refPlayer.playerName;
        playerController.activePlayer = refPlayer;
    }
    #endregion

    #region Clicking and dragging functions

    public void OnBeginDrag(PointerEventData eventData)
    {
        playerController.activePlayer = refPlayer;
        playerSelector.playerDragged = gameObject;
        GetComponent<BoxCollider2D>().size = dragSize;

        // Set up the dummy player as this will draw over the
        // playerDragged object and not be hidden by other objects
        // in the PlayersPanel group
        string playerName = refPlayer.playerName;
        dummyPlayer.SetActive(true);
        Text dummyName = dummyPlayer.GetComponentInChildren<Text>();
        dummyName.text = playerName;
     }

    public void OnDrag(PointerEventData eventData)
    {
        newPos = new Vector3(transform.position.x, eventData.position.y);
        transform.position = newPos;
        dummyPlayer.transform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<BoxCollider2D>().size = normSize;
        dummyPlayer.SetActive(false);

        // Redraw the layout group immediately otherwise playerDragged
        // is dropped where the player releases and looks unbalanced
        RectTransform rect = GetComponentInParent<RectTransform>();
        LayoutRebuilder.MarkLayoutForRebuild(rect);
        playerSelector.UpdatePlayerList();
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
            if(playerSelector.selectedPlayer != this)
            {
                return;
            }
            playerName.gameObject.SetActive(false);
            nameInputField.gameObject.SetActive(true);
            nameInputField.Select();
            nameInputField.text = playerName.text;
            lastTap = 0f;
        }
        else
        {
            lastTap = Time.time;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (playerSelector.playerDragged != gameObject)
        {
            // Change the SiblingIndex in the heirarchy to make space
            // immediately visible for playerDragged
            int colSibling = transform.GetSiblingIndex();
            playerSelector.playerDragged.transform.SetSiblingIndex(colSibling);
        }
    }
    #endregion

    #region Menu functions

    // Puts the focus on this button and then opens the menu
    public void GetFocus()
    {
        if (playerSelector.selectedPlayer == this)
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
    }

    // Open the collapsed menu
    public void ShowMenu(bool open)
    {
        animator.SetBool("openMenu", open);
    }

    // Set the active submenu
    public void SubMenuActivate (int subMenuID)
    {
        playerController.activePlayer = refPlayer;

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

    void CloseDialog()
    {
        dialogDeletePlayer.CloseDialog();
    }

    // Permanently deletes the player
    void DeletePlayerData ()
    {
        if(playerController.playerRoster.Contains(refPlayer))
            playerController.playerRoster.Remove(refPlayer);

        RemovePlayer();
    }

    // Removes the player from the current game
    public void RemovePlayer()
    {
        if (playerController.playersActive.Contains(refPlayer))
            playerController.playersActive.Remove(refPlayer);
        Destroy(gameObject);
    }

    IEnumerator SetScrollVPos (float vPos)
    {
        yield return null;
        subMenuRect.verticalNormalizedPosition = vPos;

    }
    #endregion
}