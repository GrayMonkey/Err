using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu = MenuHandler.MenuOverlay;

public class GameManager : MonoBehaviour
{
    // TODO Tidy this file up and remove all old
    // commented code!!!!

    [System.Serializable]
    public struct GameState
    {
        public GameObject landingScreen;
        public GameObject home;
        public GameObject cardSetSelect;
        public GameObject playerSelect;
        public GameObject question;
        public GameObject endGame;
    }

    public static GameManager gameManager;
    public static GameOptions gameOptions;
    public GameState gameState;
    public Question activeQuestion;
    public CardSet activeCardSet;
    public List<CardSet> defaultCardSets;
    //public CardSet[] cardSets;
    public bool gameInProgress = false;

    // The following GameState and gameStateObject should be in the
    // same order
    // public enum GameState { Home, CardSet, Players, Question, EndGame, LandingScreen };
    // public GameObject[] gameStateObject;

    [SerializeField] GameObject cardMod;
    [SerializeField] GameObject cardTrad;

    PlayerController playerController;
    //GameState currGameState;
    //GameState newGameState;
    GameObject currGameState;
    GameObject prevGameState;
    GameObject activeGameStateObject;
    GameObject pausedGameStateObject;
    MenuHandler uiMenus;

    private void Awake()
    {
        gameManager = this;
        //gameOptions = GameOptions.gameOptions;
        //gameOptions = gameManager.GetComponent<GameOptions>();
        //playerController = PlayerController.playerController;
    }

    private void OnApplicationQuit()
    {
        
    }

    // Use this for initialization
    // TODO Read in save game data
    void Start()
    {
        //activeGameStateObject = gameStateObject[(int)GameState.LandingScreen];
        //currGameState = GameState.Home;
        playerController = PlayerController.playerController;
        gameOptions = GameOptions.gameOptions;
        uiMenus = MenuHandler.uiMenus;
        //SetCardType();

        // Turn off all game states
        gameState.landingScreen.SetActive(false);
        gameState.home.SetActive(false);
        gameState.endGame.SetActive(false);
        gameState.playerSelect.SetActive(false);
        gameState.question.SetActive(false);
        gameState.cardSetSelect.SetActive(false);

        currGameState = gameState.landingScreen;
        SetGameState(currGameState);
    }

    // Bug Fix: If the card type is changed during a question and 
    // answers correctly, the card would not update correctly to the
    // next question, but would use the old question. This forces an
    // update mid game. See mnu_Options.CloseMenu()
    // Called when card type is changed via options during a game
    public void ForceCardTypeChange()
    {
        //UpdateGameState(GameState.Question);
        SetGameState(gameState.question);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (currGameState != newGameState)
    //    {
    //        UpdateGameState(newGameState);
    //    }
    //}

    public void SetGameState(GameObject newGameState)
    {
        if (currGameState != newGameState)
        {
            prevGameState = currGameState;
            prevGameState.SetActive(false);
        }
        else
        {
            prevGameState = null;
        }

        currGameState = newGameState;
        currGameState.SetActive(true);
    }

    //public void SetGameState(bool prevScreen)
    //{
    //    if(prevScreen)
    //    {
    //        currGameState = prevGameState;
    //        prevGameState = null;
    //        currGameState.SetActive(true);
    //    }
    //}

    public void PlayGame()
    {
        GameObject cardType = cardTrad;

        if (gameOptions.easyRead) cardType = cardMod;

        SetGameState(cardType);
    }

    //public void SwitchScreen(GameObject newScreen)
    //{
    //    oldScreen.SetActive(false);
    //    newScreen.SetActive(true);
    //}

    // Set up a new game state
    //public void UpdateGameState(GameState setGameState)
    //{
    //    activeGameStateObject.SetActive(false);

    //    // Switch to the gamestate using the enum GameState
    //    switch (setGameState)
    //    {
    //        //Main game menu
    //        case GameState.Home:                    //"Home":
    //            activeGameStateObject = gameStateObject[(int)GameState.Home];
    //            break;

    //        // Set the basic cardsets for all players
    //        case GameState.CardSet:
    //            activeGameStateObject = gameStateObject[(int)GameState.CardSet];
    //            break;

    //        // Display the current players sleected for the game, if any
    //        case GameState.Players:                 //"Players":
    //            activeGameStateObject = gameStateObject[(int)GameState.Players];
    //            break;


    //        //Activate a new question
    //        case GameState.Question:                //"Question":
    //            if (gameOptions.modCards)
    //            {
    //                activeGameStateObject = gameStateObject[2];     // Modern cards
    //            }
    //            else
    //            {
    //                activeGameStateObject = gameStateObject[3];     // Traditional cards
    //            }
    //            break;

    //        // End the game
    //        case GameState.EndGame:                 //"EndGame":
    //            // TODO save the player data
    //            // TODO show game stats
    //            playerController.SavePlayerData();
    //            newGameState = GameState.Home; // temp hack
    //            activeGameStateObject = gameStateObject[(int)GameState.Home];
    //            break;

    //        // Instructions on how to play
    //        // case GameState.HowToPlay:               //"HowToPlay":
    //        //    break;

    //        default:
    //            break;
    //    }

    //    activeGameStateObject.SetActive(true);
    //    currGameState = newGameState;
    //}

    // Called by Start button
    public void NewGame()
    {
        //newGameState = GameState.Players;
        //newGameState = GameState.CardSet;
        SetGameState(gameState.playerSelect);
    }

    public void ShowInstructions()
    {
        uiMenus.ShowMenu(Menu.Instructions,currGameState);
    }

    public void ShowOptions()
    {
        uiMenus.ShowMenu(Menu.Options,currGameState);
    }

 /*
    public void SetCardType()
    {
        if(gameOptions.easyReadClues)
        {
            gameState.question = cardMod;
        }
        else
        {
            gameState.question = cardTrad;
        }
    }
*/
}