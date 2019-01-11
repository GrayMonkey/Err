#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using Menu = MenuHandler.MenuOverlay;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;
    public GameObject playerPanel;
    public Player activePlayer;
    //public bool randomTurns = false;
    public List<Player> playersActive;
    public List<Player> playerRoster;
    public int playerMoves = 0;
    public bool playerDataExists = false;

    GameManager gameManager;
    GameObject playerObject;
    GameOptions gameOptions;
    LocManager locManager;
    MenuHandler uiMenu;
    int randomPlayersCount = 0;

    // Create a singleton of PlayerController
    private void Awake()
    {
        playerController = this;
        playerRoster = new List<Player>();
        playersActive = new List<Player>();
    }
 
    private void Start()
    {
        Debug.Log (Application.persistentDataPath + "/PlayerData.dat");
        gameManager = GameManager.gameManager;
        gameOptions = GameOptions.gameOptions;
        locManager = LocManager.locManager;
        uiMenu = MenuHandler.uiMenus;
    }

    private void OnEnable()
    {
        playerDataExists = LoadPlayerData();
    }

    public Player AddNewPlayer()
    {
        Player newPlayer = new Player();

        int i = 1;
        string newPlayerName = "Player " + i.ToString(); // TODO Change for localisation
        while (!UniqueNameCheck(newPlayerName, newPlayer))
        {
            i++;
            newPlayerName = "Player " + i.ToString(); //TODO change for localisation
        }

        newPlayer.playerName = newPlayerName;
        newPlayer.language = locManager.gameLanguage;
        newPlayer.cardSets.Add(gameManager.defaultCardSet);

        //playersActive.Add(newPlayer);
        activePlayer = newPlayer;
        return activePlayer;
    }

    // Check that the new name is unique
    public bool UniqueNameCheck(string checkName, Player refPlayer)
    {
        if (checkName == "") { return false; }

        foreach (Player player in playerController.playersActive)
        {
            if (checkName == player.playerName && checkName
                != refPlayer.playerName) { return false; }
        }

        foreach (Player player in playerController.playerRoster)
        {
            if (checkName == player.playerName && checkName
                != refPlayer.playerName) { return false; }
        }
        return true;
    }

    // Set the next player as the active player dependent on if turn order is random or not
    // TODO redo this function so that if that is not random that the random number is 0 - Done???
    public void NextPlayer()
    {
        if (playersActive.Count > 0)
        {
            int randomPlayersLeft = playersActive.Count - randomPlayersCount;

            // If turn order is random set i to a random number
            int i = 0;
            if (GameManager.gameOptions.randomTurns)
            {
                i = UnityEngine.Random.Range(0, randomPlayersLeft);
            }
            activePlayer = playersActive[i];

            randomPlayersCount++;
            if (randomPlayersCount > playersActive.Count - 1)
            {
                randomPlayersCount = 0;
            }

            // move the current active player to the end of the player list
            playersActive.Add(activePlayer);
            playersActive.RemoveAt(i);
        }

        // show the next playter menu
        uiMenu.ShowMenu(Menu.NewQuestion);
    }

    public void GetNewQuestion()
    {
        // set the default CardSet as chosen
        CardSet cardSet = gameManager.defaultCardSet;
        cardSet.setQuestion();

        // set the gamestatemanager
        gameManager.UpdateGameState(GameManager.GameState.Question);
    }

    public void StartGame()
    {
        gameManager.gameInProgress = true;
        // Update stats for all the players
        foreach (Player player in playersActive)
        {
            player.answersThisGame = 0;
            player.questionsThisGame = 0;
            player.pointsThisGame = 0;
        }

        NextPlayer();
    }

    // Update the activePlayer stats
    // TODO Should this be in game manager?
    public void UpdatePlayerStats()
    {
        foreach(Player player in playersActive)
        {
            player.answersTotal += player.answersThisGame;
            player.questionsTotal += player.questionsThisGame;
            player.pointsTotal += player.pointsThisGame;
                    }
    }

    public void SavePlayerData()
    {
        foreach (Player player in playersActive)
        {
            player.gamesTotal++;
            Player newPlayer = playerRoster.Find((Player obj) => obj.playerName == player.playerName);
            if (newPlayer == null) { playerRoster.Add(player); }
        }

        playerRoster.Sort(delegate (Player x, Player y)
        {
            if (x.playerName == null && y.playerName == null) return 0;     // The null values should never
            else if (x.playerName == null) return -1;                       // be evaluated as the playerName
            else if (y.playerName == null) return 1;                        // will always have a value
            else return x.playerName.CompareTo(y.playerName);
        });

        // Create/write playerRoster to the save file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.OpenWrite(Application.persistentDataPath + "/PlayerData.dat");
        bf.Serialize(saveFile, playerRoster);
        saveFile.Close();
        playerDataExists = LoadPlayerData();
        PlayerRosterSelect.playerRosterSelect.PopulateList();
    }

    public bool LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {
            //Debug.Log(Application.persistentDataPath + "/PlayerData.dat");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream loadFile = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
            playerRoster = (List<Player>)bf.Deserialize(loadFile);
            return true;
        }
        return false;
    }

    public void RemovePlayerData(Player deletePlayer)
    {
        if (playerDataExists)
        {
            int index = playerRoster.FindIndex((Player obj) => obj.playerName.Equals(deletePlayer.playerName));
            if (index > 0)
            {
                playerRoster.RemoveAt(index);
            }
        }
    }
}