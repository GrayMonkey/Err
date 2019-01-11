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

    [SerializeField] Text playerName;
    [SerializeField] InputField nameInputField;
    [SerializeField] Image buttonImage;

    GameObject dummyPlayer; // to mimic the playerDragged object
    PlayerRosterSelect playerRosterSelect;
    PlayerSelector playerSelector;
    PlayerController playerController;
    PlayerObject hasSubMenuFocus;
    MenuHandler uiMenus;
    RectTransform rectTransform;
    Text inputText;
    Text inputPlaceholder;
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

    // SubMenus variables
    [SerializeField] Animator animator;
    [SerializeField] ScrollRect subMenuRect;
    [SerializeField] GameObject[] mnu_SubMenus;
    [SerializeField] Button[] btn_SubMenuButtons;
    [SerializeField] Text subMenuInfoText;

    enum SubMenu { PlayerRoster, CardSets, Language, RemovePlayer };

    private void Awake()
    {
        uiMenus = MenuHandler.uiMenus;
        playerSelector = PlayerSelector.playerSelector;
        playerController = PlayerController.playerController;
        playerRosterSelect = PlayerRosterSelect.playerRosterSelect;
    }

    private void Start()
    {
        uiMenus = MenuHandler.uiMenus;
        playerSelector = PlayerSelector.playerSelector;
        playerRosterSelect = PlayerRosterSelect.playerRosterSelect;refPlayer = playerController.activePlayer;
        playerName.text = refPlayer.playerName;
        dummyPlayer = GameObject.FindWithTag("DummyPlayer");
   }

    private void Update()
    {
        //if (animator.enabled && animator.GetBool("openMenu") == false)
        if (playerSelector.hasFocus != this)
        {
            OpenMenu(false);
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

    public void NameCheck(string text)
    {
        buttonImage.color = hilightRed;
        text = nameInputField.text;
        if (playerController.UniqueNameCheck(text, refPlayer))
        { 
            buttonImage.color = hilightGreen; 
        }
    }

    public void UpdatePlayerName()
    {
        string newName = nameInputField.text;

        // check if the new name is blank and if so reset it to the original name
        // or if the new name is not unique
        if (!playerController.UniqueNameCheck(newName, refPlayer)) {newName = playerName.text;}

        refPlayer.playerName = newName;
        playerName.text = newName;
        playerName.gameObject.SetActive(true);
        nameInputField.text = newName;
        nameInputField.gameObject.SetActive(false);
        buttonImage.color = Color.white;
    }

    public void UpdateToRosterPlayer(Player rosterPlayer)
    {
        int index = playerController.playersActive.IndexOf(refPlayer);
        playerController.playersActive.Insert(index, rosterPlayer);
        playerController.playersActive.Remove(refPlayer);

        refPlayer = rosterPlayer;
        playerName.text = refPlayer.playerName;
    }
    #endregion

    #region Clicking and dragging functions

    public void OnBeginDrag(PointerEventData eventData)
    {
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
        // Check if another button has the sub menu open and close it
        playerSelector.hasFocus = this;

        // Double tap detected. Can't use tapcount as not supported by Android
        // Change players name
        if (lastTap + delayTap > Time.time)
        {
            // TODO Edit player details
            Debug.Log("Obj Pressed: " + this.gameObject.name);
            playerName.gameObject.SetActive(false);
            nameInputField.gameObject.SetActive(true);
            nameInputField.image.gameObject.SetActive(true);
            nameInputField.Select();
            inputText.text = playerName.text;
            inputPlaceholder.text = playerName.text;
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
        if (playerSelector.hasFocus == this)
        {
            OpenMenu(false);
            playerSelector.hasFocus = null;
        } else {
            this.animator.enabled = true;
            playerSelector.hasFocus = this;
            OpenMenu(true);
            SubMenuActivate(999);
        }

        // Select the active button
        //SubMenuActivate((int)SubMenu.PlayerRoster);
    }

    // Open the collapsed menu
    public void OpenMenu(bool open)
    {
        animator.SetBool("openMenu", open);
    }

    // Set the active submenu
    public void SubMenuActivate (int subMenuID)
    {
        foreach (Button button in btn_SubMenuButtons)
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
                mnu_SubMenus[0].SetActive(true);
                subMenuRect.content = mnu_SubMenus[0].GetComponent<RectTransform>();
                btn_SubMenuButtons[0].GetComponent<Image>().color = hilightGreen;
                subMenuInfoText.text = "Select card set...";
                break;
                
            case (int) SubMenu.Language:
                mnu_SubMenus[1].SetActive(true);
                rectTransform = mnu_SubMenus[1].GetComponent<RectTransform>();
                subMenuRect.content = rectTransform;
                btn_SubMenuButtons[1].GetComponent<Image>().color = hilightGreen;

                int playerLang = (int)refPlayer.language;
                Toggle[] languages = GetComponentsInChildren<Toggle>();
                for (int i = 0; i < languages.Length; i++)
                {
                    languages[i].isOn = false;
                }
                languages[playerLang].isOn = true;
                subMenuInfoText.text = "Select language... (" + refPlayer.language.ToString() + ")"; 

                // set the scrollrect so that the selected language is shown
                float maxPos = languages.Length - 1;
                float vPos = playerLang / maxPos;
                vPos = 1.0f - vPos;
                subMenuRect.verticalNormalizedPosition = vPos;
 
                break;
                
            case (int) SubMenu.PlayerRoster:
                mnu_SubMenus[2].SetActive(true);
                rectTransform = subMenuRect.content;
                rectTransform = mnu_SubMenus[2].GetComponent<RectTransform>();
                subMenuRect.content = rectTransform;
                btn_SubMenuButtons[2].GetComponent<Image>().color = hilightGreen;
                subMenuInfoText.text = "Select player...";
                if (!playerController.playerDataExists)
                {
                    subMenuInfoText.text = "No player data is available!";
                }
                break;
                
            case (int) SubMenu.RemovePlayer:
                mnu_SubMenus[3].SetActive(true);
                subMenuRect.content = mnu_SubMenus[3].GetComponent<RectTransform>();
                btn_SubMenuButtons[3].GetComponent<Image>().color = hilightGreen;
                subMenuInfoText.text = "Remove player...?...";
                break;

            default:
                subMenuInfoText.text = "Select option...";
                break;
        }
    }

    public void SetLanguage(int sysLang)
    {
        refPlayer.language = (GameLanguage)sysLang;
        subMenuInfoText.text = "Select language... (" + refPlayer.language.ToString() + ")";
    }

    public void SetCardSets()
    {
        
    }

    public void RemovePlayer(string removal)
    {
        
    }

    IEnumerator SetScrollVPos (float vPos)
    {
        yield return null;
        subMenuRect.verticalNormalizedPosition = vPos;

    }
    #endregion
}