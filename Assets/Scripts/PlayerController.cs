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
        string newPlayerName = locManager.GetLocText("str_Player") + " " + i.ToString();
        while (!UniqueNameCheck(newPlayerName, newPlayer))
        {
            i++;
            newPlayerName = locManager.GetLocText("str_Player") + " " + i.ToString();
        }

        newPlayer.playerName = newPlayerName;
        newPlayer.language = locManager.GameLang;
        newPlayer.cardSets = gameManager.defaultCardSets;

        // Define the player ID using the first two characters or the first character
        // of each word if a space is in the name
        string _id;
        int _index;
        if (newPlayerName.Contains(" "))
        {
            _index = newPlayerName.IndexOf(" ") + 1;
            _id = newPlayerName.Substring(0, 1) +
                newPlayerName.Substring(_index, 1);
        }
        else
        {
            _id = newPlayerName.Substring(0, 2);
        }

        newPlayer.playerID = _id;

        //playersActive.Add(newPlayer);
        activePlayer = newPlayer;
        return activePlayer;
    }

    // Check that the new name is unique
    public bool UniqueNameCheck(string checkName, Player refPlayer)
    {
        if (checkName == "") { return false; }
        if (checkName == refPlayer.playerName) { return true; }

        Player _player = playerController.playersActive.Find((Player obj) => obj.playerName == checkName);
        if (_player != null) { return false; }

        _player = playerController.playerRoster.Find((Player obj) => obj.playerName == checkName);
        if (_player != null) { return false; }

        //foreach (Player player in playerController.playersActive)
        //{
        //    if (checkName == player.playerName && checkName
        //        != refPlayer.playerName) { return false; }
        //}

        //foreach (Player player in playerController.playerRoster)
        //{
        //    if (checkName == player.playerName && checkName
        //        != refPlayer.playerName) { return false; }
        //}
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
        CardSet questionSet;

        int i = UnityEngine.Random.Range(0, activePlayer.cardSets.Count()-1);
        questionSet = activePlayer.cardSets[i];
        questionSet.setQuestion();

        // set the gamestatemanager
        //gameManager.UpdateGameState(GameManager.GameState.Question);
        //gameManager.SetGameState(gameManager.gameState.question);
    }

    public void StartGame()
    {
        gameManager.gameInProgress = true;
        PlayerObject pObject = PlayerSelector.playerSelector.selectedPlayer;
        if (pObject)
            pObject.ShowMenu(false);

        // Update stats for all the players
        foreach (Player player in playersActive)
        {
            player.answersThisGame = 0;
            player.questionsThisGame = 0;
            player.pointsThisGame = 0;

            //Set the default card set for the player if cardSets is 0
            if (player.cardSets.Count == 0)
                player.cardSets = gameManager.defaultCardSets;
        }

        NextPlayer();
    }

    // Update the activePlayer stats
    // TODO Should this be in game manager?
    public void UpdatePlayerStats()
    {
        foreach (Player player in playersActive)
        {
            player.answersTotal += player.answersThisGame;
            player.questionsTotal += player.questionsThisGame;
            player.pointsTotal += player.pointsThisGame;
        }
    }

    public void SavePlayerData()
    {
        List<PlayerData> playersData = new List<PlayerData>();

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

        foreach (Player player in playerRoster)
        {
            PlayerData data = new PlayerData
            {
                playerName = player.playerName,
                language = player.language,
                gamesTotal = player.gamesTotal,
                gamesWon = player.gamesWon,
                questionsTotal = player.questionsTotal,
                answersTotal = player.answersTotal,
                pointsTotal = player.pointsTotal
            };

            foreach (CardSet cardSet in player.cardSets)
            {
                data.cardSets.Add(cardSet.gameObject.name);
            }

            playersData.Add(data);
        }

        // Create/write playerRoster to the save file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.OpenWrite(Application.persistentDataPath + "/PlayerData.dat");
        bf.Serialize(saveFile, playersData);
        saveFile.Close();
        playerDataExists = LoadPlayerData();
    }

    public bool LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream loadFile = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
            List<PlayerData> playersData = (List<PlayerData>)bf.Deserialize(loadFile);

            playerRoster.Clear();

            foreach (PlayerData data in playersData)
            {
                // Read in each player
                Player player = new Player(); ;
                player.playerName = data.playerName;
                player.language = data.language;
                player.gamesTotal = data.gamesTotal;
                player.gamesWon = data.gamesWon;
                player.questionsTotal = data.questionsTotal;
                player.answersTotal = data.answersTotal;
                player.pointsTotal = data.pointsTotal;

                foreach (string cardSetName in data.cardSets)
                {
                    GameObject obj = GameObject.Find(cardSetName);
                    if (obj != null)
                    {
                        CardSet cardSet = obj.GetComponent<CardSet>();
                        if (cardSet != null)
                        {
                            player.cardSets.Add(cardSet);
                        }
                    }
                }

                // Add player to the playerRoster
                playerRoster.Add(player);

                playerRoster.Sort(delegate (Player x, Player y)
                {
                    if (x.playerName == null && y.playerName == null) return 0;     // The null values should never
                    else if (x.playerName == null) return -1;                       // be evaluated as the playerName
                    else if (y.playerName == null) return 1;                        // will always have a value
                    else return x.playerName.CompareTo(y.playerName);
                });
            }
            return true;
        }
        return false;
    }

    public void RemovePlayerData(Player deletePlayer)
    {
        if (playerRoster.Count > 0)
        {
            int index = playerRoster.FindIndex((Player obj) => obj.playerName.Equals(deletePlayer.playerName));
            if (index > 0)
            {
                playerRoster.RemoveAt(index);
            }
        }
    }
}

[System.Serializable]
class PlayerData
{
    public string playerName;
    public string playerID;
    public SystemLanguage language;
    public int gamesTotal;
    public int gamesWon;
    public int questionsTotal;
    public int answersTotal;
    public int pointsTotal;
    public List<string> cardSets = new List<string>();
}