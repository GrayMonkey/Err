using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [SerializeField] Text instructions;
    [SerializeField] Button playerSelect;
    [SerializeField] Button cardSetSelect;
    [SerializeField] Button startGame;

    GameManager gameManager;
    PlayerController playerController;
    
    Color proceed = new Color(0.7f, 1.0f, 0.4f);
    Color select = new Color(1.0f, 0.7f, 0.7f);
    Image playerBtnImage;
    Image cardSetBtnImage;
    Text playerBtnCount;
    Text cardSetBtnCount;

    private void Awake()
    {
        gameManager = GameManager.instance;
        playerController = PlayerController.instance;
        playerBtnImage = playerSelect.GetComponent<Image>();
        cardSetBtnImage = cardSetSelect.GetComponent<Image>();
        playerBtnCount = playerSelect.GetComponentInChildren<Text>();
        cardSetBtnCount = cardSetSelect.GetComponentInChildren<Text>();
    }

    void OnEnable()
    {
        startGame.interactable = false;
        cardSetBtnImage.color = select;
        cardSetBtnCount.text = gameManager.defaultCardSets.Count.ToString();
        playerBtnImage.color = select;
        playerBtnCount.text = playerController.playersActive.Count.ToString();
        playerSelect.interactable = false;
    }

    private void Start()
    {
        bool players = false;
        bool cardsets = false;

        instructions.text = "UI_CardSetSelect";

        if (gameManager.defaultCardSets.Count > 0)
        {
            cardSetBtnImage.color = proceed;
            cardsets = true;
            instructions.text = "str_BtnHelpSelectPlayers";
            playerSelect.interactable = true;
        }
        instructions.GetComponent<Translate>().UpdateString();

        if (playerController.playersActive.Count > 1 && cardsets)
        {
            if (playerController.playersActive.Count > 1) 
                playerBtnImage.color = proceed;
            players = true;
        }

        if (players && cardsets)
        {
            instructions.text = "";
            startGame.interactable = true;
        }
    }

    public void StartGame()
    {
        this.gameObject.SetActive(false);
        playerController.StartGame();
    }
}
