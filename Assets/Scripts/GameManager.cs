using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu = MenuHandler.MenuOverlay;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public static GameOptions gameOptions;
    public enum GameState {Home, CardSet, Players, Question, EndGame, HowToPlay};
    public Question activeQuestion;
    public List<CardSet> defaultCardSets;
    //public CardSet[] cardSets;
    public GameObject[] gameStateObject;
    public bool gameInProgress = false;

    PlayerController playerController;
    GameState currGameState;
    GameState newGameState;
    GameObject activeGameStateObject;
    GameObject pausedGameStateObject;
    MenuHandler uiMenus;

    private void Awake()
    {
        gameManager = this;
        gameOptions = gameManager.GetComponent<GameOptions>();
        playerController = PlayerController.playerController;
    }

    private void OnApplicationQuit()
    {
        
    }

    // Use this for initialization
    // TODO Read in save game data
    void Start()
    {
        activeGameStateObject = gameStateObject[0];
        playerController = PlayerController.playerController;
        currGameState = GameState.Home;
        gameOptions = GameOptions.gameOptions;
        uiMenus = MenuHandler.uiMenus;

        for (int i = 0; i < gameStateObject.Length; i++)
        {
            gameStateObject[i].SetActive(false);
        }

        gameStateObject[0].SetActive(true);

        // if this is the first time the game has been launched
        // then show the Welcome message
        if(gameOptions.firstLaunch)
        {

        }
    }

    // Bug Fix: If the card type is changed during a question and 
    // answers correctly, the card would not update correctly to the
    // next question, but would use the old question. This forces an
    // update mid game. See mnu_Options.CloseMenu()
    // Called when card type is changed via options during a game
    public void ChangeCardType()
    {
        UpdateGameState(GameState.Question);
    }

    // Update is called once per frame
    void Update()
    {
        if (currGameState != newGameState)
        {
            UpdateGameState(newGameState);
        }
    }

    // Set up a new game state
    public void UpdateGameState(GameState setGameState)
    {
        activeGameStateObject.SetActive(false);

        // Switch to the gamestate using the enum GameState
        switch (setGameState)
        {
            //Main game menu
            case GameState.Home:                    //"Home":
                activeGameStateObject = gameStateObject[(int)GameState.Home];
                break;

            // Set the basic cardsets for all players
            case GameState.CardSet:
                activeGameStateObject = gameStateObject[(int)GameState.CardSet];
                break;

            // Display the current players sleected for the game, if any
            case GameState.Players:                 //"Players":
                activeGameStateObject = gameStateObject[(int)GameState.Players];
                break;


            //Activate a new question
            case GameState.Question:                //"Question":
                if (gameOptions.modCards)
                {
                    activeGameStateObject = gameStateObject[2];     // Modern cards
                }
                else
                {
                    activeGameStateObject = gameStateObject[3];     // Traditional cards
                }
                break;

            // End the game
            case GameState.EndGame:                 //"EndGame":
                // TODO save the player data
                // TODO show game stats
                playerController.SavePlayerData();
                newGameState = GameState.Home; // temp hack
                activeGameStateObject = gameStateObject[(int)GameState.Home];
                break;

            // Instructions on how to play
            case GameState.HowToPlay:               //"HowToPlay":
                break;
            
            default:
                break;
        }

        activeGameStateObject.SetActive(true);
        currGameState = newGameState;
    }

    // Called by Start button
    public void NewGame()
    {
        //newGameState = GameState.Players;
        newGameState = GameState.CardSet;
    }

    public void ShowInstructions()
    {
        uiMenus.ShowMenu(Menu.Instructions);
    }

    public void ShowOptions()
    {
        uiMenus.ShowMenu(Menu.Options);
    }
}