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

    CardSetManager cardSetManager;
    PlayerController playerController;
    
    Color proceed = new Color(0.7f, 1.0f, 0.4f);
    Color select = new Color(1.0f, 0.7f, 0.7f);
    Image playerBtnImage;
    Image cardSetBtnImage;
    Text playerBtnCount;
    Text cardSetBtnCount;

    private void Awake()
    {
        cardSetManager = CardSetManager.instance;
        playerController = PlayerController.instance;
        playerBtnImage = playerSelect.GetComponent<Image>();
        cardSetBtnImage = cardSetSelect.GetComponent<Image>();
        playerBtnCount = playerSelect.GetComponentInChildren<Text>();
        cardSetBtnCount = cardSetSelect.GetComponentInChildren<Text>();
    }

    void OnEnable()
    {
        startGame.interactable = CheckGameStart();
    }

    private bool CheckGameStart()
    {
        bool players = false;
        bool cardsets = false;
        bool start = false;

        playerSelect.interactable = false;

        cardSetBtnImage.color = select;
        playerBtnImage.color = select;

        if (cardSetManager.activeCardSets.Count > 0)
        {
            cardSetBtnImage.color = proceed;
            cardsets = true;
            playerSelect.interactable = true;
        }

        instructions.GetComponent<Translate>().UpdateString();

        if (playerController.playersActive.Count > 1 && cardsets)
        {
            if (playerController.playersActive.Count > 1)
                playerBtnImage.color = proceed;
            players = true;
        }

        cardSetBtnCount.text = cardSetManager.activeCardSets.Count.ToString();
        playerBtnCount.text = playerController.playersActive.Count.ToString();

        if (players && cardsets)
            start = true;

        return start;
    }

    public void StartGame()
    {
        this.gameObject.SetActive(false);
        playerController.StartGame();
    }
}
