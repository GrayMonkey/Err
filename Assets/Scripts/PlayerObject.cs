#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEditorInternal;

public class PlayerObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    // TODO: Still some tidying up of this script, need to remove unneeded variables.

    // Main PlayerObject variables
    public Player thisPlayer;
    public bool subMenuOpen = false;

    [SerializeField] private Text playerName;
    //[SerializeField] private Image playerNameBG;
    [SerializeField] private Text playerID;
    [SerializeField] private Text dummyPlayerName;
    [SerializeField] private Text dummyPlayerID;
    [SerializeField] private GameObject dummyPlayer;
    [SerializeField] private GameObject addRosterPlayerButton;
    [SerializeField] private GameObject moreButton;
    [SerializeField] private GameObject activePlayerHolder;
    [SerializeField] private GameObject rosterPlayerHolder;
    [SerializeField] private GameObject extraButtons;
    [SerializeField] private ModalDialog dialogDeletePlayer;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Animator animator;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Image idImage;
    [SerializeField] private Sprite btnRemove;
    [SerializeField] private Sprite btnTrash;
    [SerializeField] private Sprite btnCancel;
    [SerializeField] private Sprite btnConfirm;

    RectTransform activePlayerRectTransform;
    PlayerSelector playerSelector;
    PlayerController playerController;
    float lastTap = 0f;
    float delayTap = 0.25f;
    float startTime;
    float clickOffset;
    bool active = false;
    Color darkGreen = new Color(0.0f, 0.9f, 0.0f);
    Color hilightGreen = new Color(0.7f, 1.0f, 0.4f);
    Color lightRed = new Color(1.0f, 0.35f, 0.35f);
    Color hilightRed = new Color(1.0f, 0.6f, 0.6f);
    //Color normal = new Color(1.0f, 1.0f, 1.0f);
    string locText;

    private void Awake()
    {
        playerSelector = PlayerSelector.playerSelector;
        playerController = PlayerController.playerController;
        activePlayerRectTransform = activePlayerHolder.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // only using this for debug
        try
        {
            PlayerObject player = playerSelector.activePlayerObject;
        }
        catch (Exception)
        {
            Debug.Log("No active player");
            throw;
        }
        if(playerSelector.activePlayerObject == this)
        {
            RectTransform _rect = gameObject.GetComponent<RectTransform>();
            Debug.LogError(gameObject.name +": " +_rect.position.ToString());
        }
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
        thisPlayer = player;
        playerName.text = thisPlayer.playerName;
        playerID.text = thisPlayer.playerID;
        gameObject.name = thisPlayer.playerName;
        SetContextButton();
    }

    #region Player handling
    public void UpdatePlayerName(bool endEdit)
    {
        // Only need to do this if the input field is active fixes
        // bug whereby this function is called when endEdit is true
        string newName = nameInputField.text;
        bool goodName = playerController.UniqueNameCheck(newName, thisPlayer);

        playerID.text = GetPlayerID(newName);
        buttonImage.color = hilightRed;
        idImage.color = hilightRed;

        if (endEdit)
        {
            nameInputField.gameObject.SetActive(false);
            playerName.gameObject.SetActive(true);
            playerID.fontStyle = FontStyle.Bold;
            buttonImage.color = Color.white;

            if (goodName)
            {
                thisPlayer.playerName = newName;
                thisPlayer.playerID = GetPlayerID(newName);
                playerName.text = newName;
                gameObject.name = newName;
            }
            else
            {
                playerID.text = thisPlayer.playerID;
            }
            idImage.color = new Color(0.85f, 0.93f, 1.0f);
            return;
        }


        // check if the new name is blank and if so reset it to the original name
        // or if the new name is not unique
        if (goodName)
        {
            buttonImage.color = hilightGreen;
            idImage.color = hilightGreen;

            // Set the new ID tag
            playerID.fontStyle = FontStyle.BoldAndItalic;
            playerID.text = GetPlayerID(newName);
        }
    }

    private string GetPlayerID(string name)
    {
        string id;

        int length = Math.Min(name.Length, 2);           // In case the name only has one character
        id = name.Substring(0, length);

        if (name.Contains(" ") && name.Substring(name.Length - 1, 1) != " ")
        {
            int index = name.IndexOf(" ") + 1;
            id = name.Substring(0, 1) + name.Substring(index, 1);
        }

        return id;
    }

    public void AddRosterPlayerToGame()
    {
        gameObject.transform.SetParent(activePlayerHolder.transform);
        //playerSelector.CheckPlayerCounts();
        playerController.playersActive.Add(thisPlayer);
        if (playerSelector.activePlayerObject != null)
            playerSelector.activePlayerObject.SubMenu(false);
        playerSelector.activePlayerObject = this;
        SetContextButton();
    }
    #endregion

    #region Clicking and dragging functions

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!active)
            return;
        SubMenu(false);
        playerSelector.activePlayerObject = this;
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
        // Debug.Log("Dragging: " + thisPlayer.playerName);
        // Debug.Log("Offset: " + clickOffset.ToString());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!active)
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
        //Debug.Log(gameObject.name + " yPos: " + gameObject.transform.position.y.ToString() + " | DummyObject yPos: " + dummyPlayer.transform.position.y.ToString());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        dummyPlayer.SetActive(false);
        playerSelector.UpdatePlayerList();
        LayoutRebuilder.MarkLayoutForRebuild(GetComponentInParent<RectTransform>());
        //Debug.Log("Dropped: " + thisPlayer.playerName);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //playerController.activePlayer = thisPlayer;
        if (!active)
            return;
        playerSelector.activePlayerObject.SubMenu(false);
        playerSelector.activePlayerObject = this;

        // Double tap detected. Can't use tapcount as not supported by Android
        // Change players name
        if (lastTap + delayTap > Time.time)
        {
            if (playerSelector.activePlayerObject != this)
            {
                return;
            }
            Debug.Log("Editing player...");
            playerName.gameObject.SetActive(false);
            nameInputField.text = playerName.text;
            nameInputField.gameObject.SetActive(true);
            nameInputField.Select();
            lastTap = 0f;
            UpdatePlayerName(false);
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
            //Debug.Log("Swapping: " + gameObject.name + "==" + playerSelector.playerDragged.name);
        }
        else
        {
            //Debug.Log("Not swapping! " + gameObject.name + "<>" + playerSelector.playerDragged.name);
        }
    }
    #endregion

    #region Menu functions
    // Activate or deactive the sub menu
    public void SubMenu(bool open)
    {
        // Close the previous active playerObject
        if (playerSelector.activePlayerObject != this)
            playerSelector.activePlayerObject.SubMenu(false);
        playerSelector.activePlayerObject = this;
        animator.SetBool("subMenu", open);
        //Debug.Log(gameObject.name + ": OpenSubMenu(" + open.ToString() + ")");
    }

    public void SetAddPlayerFromRosterButton(bool _active)
    {
        // If maxPlayers has been reached make the button non interactive
        addRosterPlayerButton.GetComponent<Button>().interactable = _active;
    }

    public void DeletePlayer()
    {
        if (playerController.playerRoster.Contains(thisPlayer))
        {
            RemoveRosterPlayer();
        }
        else
        {
            RemoveCurrentPlayer(true);
        }
    }

    private void RemoveRosterPlayer()
    { 
        ModalDialogDetails details = new ModalDialogDetails();
        details.button1Details = new ButtonDetails();
        details.button2Details = new ButtonDetails();
        details.buttonCanceldetails = new ButtonDetails();

        locText = thisPlayer.playerName;
        details.title = locText;

        locText = LocManager.locManager.GetLocText("str_DialogPlayerDelete");
        details.body = locText;

        details.button1Details.icon = btnRemove;
        details.button1Details.action = RosterCurrentPlayer;

        details.button2Details.icon = btnTrash;
        details.button2Details.action = FinalWarning;

        details.buttonCanceldetails.icon = btnCancel;

        dialogDeletePlayer.Show(details);
    }

    // final warning if trashing the player
    private void FinalWarning()
    {
        ModalDialogDetails details = new ModalDialogDetails();
        details.button1Details = new ButtonDetails();
        details.button2Details = new ButtonDetails();
        details.buttonCanceldetails = new ButtonDetails();

        locText = thisPlayer.playerName;
        details.title = locText;

        locText = LocManager.locManager.GetLocText("str_DialogFinalWarning");
        details.body = locText;

        details.button1Details.action = CloseDialog;
        details.button1Details.icon = btnCancel;

        details.button2Details.action = DestroyCurrentPlayer;
        details.button2Details.icon = btnConfirm;

        details.buttonCanceldetails.icon = btnCancel;

        dialogDeletePlayer.Show(details);
    }

    private void CloseDialog()
    {
        dialogDeletePlayer.CloseDialog();
    }


    // Put the player back in the Roster if necessary
    private void RosterCurrentPlayer()
    {
        RemoveCurrentPlayer(false);
    }

    // Trash the player and their data
    private void DestroyCurrentPlayer()
    {
        RemoveCurrentPlayer(true);
    }

    private void RemoveCurrentPlayer(bool destroyPlayer)
    {
        bool rosteredPlayer = playerController.playerRoster.Contains(thisPlayer);

        // close the modal dialog
        CloseDialog();

        // if player exists in the roster then send them back to the roster if its not
        // destoryed, otherwise destory the player
        if (rosteredPlayer)
        {
            gameObject.transform.SetParent(rosterPlayerHolder.transform);
        }
        else
        {
            destroyPlayer = true;
        }

        // close the submenu
        SubMenu(false);
        extraButtons.SetActive(false);
        moreButton.SetActive(false);
        addRosterPlayerButton.SetActive(true);

        if (destroyPlayer)
        {
            playerController.playersActive.Remove(thisPlayer);
            Destroy(gameObject);
        }



        // if the player has been destroyed and there is at least one player
        // in the active player list then make the first player in the list
        // the activePlayerObject
        /*for (int i = 0; i < playerSelector.newPlayerCount; i++)
        {
            if (activePlayerHolder.transform.GetChild(i).gameObject.TryGetComponent<PlayerObject>(out PlayerObject _playerObject))
                playerSelector.activePlayerObject = _playerObject;
        }*/
    }
    #endregion
}