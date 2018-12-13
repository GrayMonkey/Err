using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerObjectEvents : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler 
{
    //public static GameObject playerDragged;

    [SerializeField] InputField nameInputField;
    [SerializeField] Text playerName;
    [SerializeField] Button loadPlayer;
    [SerializeField] Button removePlayer;

    GameObject dummyPlayer; // to mimic the playerDragged object
    PlayerSelector playerSelector;
    PlayerController playerController;
    PlayerObject playerButton;
    PlayerRosterSelect playerRosterSelect;
    RectTransform rectTransform;
    Text inputText;
    Text inputPlaceholder;
    Vector3 newPos;
    Vector2 normSize = new Vector2(350.0f, 2.0f);
    Vector2 dragSize = new Vector2(350.0f, 20.0f);
    float lastTap = 0f;
    float delayTap = 0.25f;
    bool singleTap = false;

    private void Start()
    {
        playerSelector = PlayerSelector.playerSelector;
        playerController = PlayerController.playerController;
        playerRosterSelect = PlayerRosterSelect.playerRosterSelect;
        dummyPlayer = GameObject.FindWithTag("DummyPlayer");
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (lastTap > 0.0f)
        {
            if (lastTap + delayTap < Time.time)
            {
                Debug.Log("Execute single click");
                lastTap = 0.0f;
                loadPlayer.gameObject.SetActive(!loadPlayer.gameObject.activeInHierarchy);
                removePlayer.gameObject.SetActive(!removePlayer.gameObject.activeInHierarchy);
            }
        }
    }

    private void OnEnable()
    {
        //playerRosterSelect.Reset();

        playerButton = GetComponent<PlayerObject>();
        inputText = nameInputField.textComponent.GetComponent<Text>();
        inputPlaceholder = nameInputField.placeholder.GetComponent<Text>();
        ShowRemovePlayer();
    }

    private void ShowRemovePlayer()
    {
        loadPlayer.gameObject.SetActive(true);
        removePlayer.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //playerRosterSelect.Reset();

        playerSelector.playerDragged = gameObject;
        GetComponent<BoxCollider2D>().size = dragSize;

        // Set up the dummy player as this will draw over the
        // playerDragged object and not be hidden by other objects
        // in the PlayersPanel group
        string playerName = playerButton.refPlayer.playerName;
        dummyPlayer.SetActive(true);
        Text dummyName = dummyPlayer.GetComponentInChildren<Text>();
        dummyName.text = playerName;
        //nameInputField.placeholder.GetComponent<Text>().text = playerName;
        ShowRemovePlayer();
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
        //Debug.Log("Double Tap Intv: " + (lastTap + delayTap).ToString());
        //Debug.Log("Time.time: " + Time.time.ToString());

        if (lastTap + delayTap > Time.time)
        {
            // TODO Edit player details
            playerName.gameObject.SetActive(false);
            nameInputField.gameObject.SetActive(true);
            nameInputField.image.gameObject.SetActive(true);
            nameInputField.Select();
            inputText.text = playerName.text;
            inputPlaceholder.text = playerName.text;
            lastTap = 0f;
            //singleTap = false;
            ShowRemovePlayer();
            //Debug.Log("Execute double click!");
            //Debug.Log(playerName.text + ": Double tap");
            lastTap = 0.0f;
        } else {
            //Debug.Log(playerName.text + ": Single tap");
            lastTap = Time.time;
        }

        // reset the button in case it has the playerRoster
        if (playerRosterSelect.inUse)
        {
            ResetButton();
            return;
        }




        //singleTap = true;

        if (eventData.clickCount == 1)
        {
            lastTap = Time.time;
        }

        if (eventData.clickCount == 2)
        {
            // TODO Edit player details
            playerName.gameObject.SetActive(false);
            nameInputField.gameObject.SetActive(true);
            nameInputField.image.gameObject.SetActive(true);
            nameInputField.Select();
            inputText.text = playerName.text;
            inputPlaceholder.text = playerName.text;
            lastTap = 0f;
            //singleTap = false;
            ShowRemovePlayer();
            //Debug.Log("Execute double click!");
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (playerSelector.playerDragged != gameObject)
        {
            // Change the SiblingIndex in the heirarchy to make space
            // immediately visible for playerDragged
            //            int dragSibling = playerDragged.transform.GetSiblingIndex();
            int colSibling = transform.GetSiblingIndex();
            playerSelector.playerDragged.transform.SetSiblingIndex(colSibling);
        }
    }

    public void ResetButton()
    {
        rectTransform.sizeDelta = new Vector2(350.0f, 60.0f);
        playerRosterSelect.Hide();
    }
}
