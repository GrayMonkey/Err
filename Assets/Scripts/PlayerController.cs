#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using Menu = MenuHandler.MenuOverlay;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject playerPanel;
    public Player activePlayer;
    public List<Player> playersActive;
    public List<Player> playerRoster;
    public int playerMoves = 0;
    public bool playerDataExists = false;

    GameManager gameManager;
    CardSetManager cardSetManager;
    LocManager locManager;
    MenuHandler uiMenu;
    int randomPlayersCount = 0;

    // Create a singleton of PlayerController
    private void Awake()
    {
        instance = this;
        playerRoster = new List<Player>();
        playersActive = new List<Player>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        cardSetManager = CardSetManager.instance;
        locManager = LocManager.instance;
        uiMenu = MenuHandler.instance;
    }

    private void OnEnable()
    {
        playerDataExists = LoadPlayerData();

        // For dev purposes only... comment out for release
        // or testing...
        //if (!playerDataExists) CreateDummyRoster();
    }

    private void CreateDummyRoster()
    {
        for (int i = 1; i < 4; i++)
        {
            Player dummyPlayer = new Player();
            dummyPlayer.playerName = "Roster " + i.ToString();
            dummyPlayer.playerID = "R" + i.ToString();
            playerRoster.Add(dummyPlayer);
        }
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
        newPlayer.cardSets = cardSetManager.activeCardSets;

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

        playersActive.Add(newPlayer);
        activePlayer = newPlayer;
        return activePlayer;
    }

    // Check that the new name is unique
    public bool UniqueNameCheck(string checkName, Player refPlayer)
    {
        if (checkName == "") { return false; }
        if (checkName == refPlayer.playerName) { return true; }

        if (instance.playersActive.Count > 0)
        {
            Player _player = instance.playersActive.Find((Player obj) => obj.playerName == checkName);
            if (_player != null) { return false; }
        }

        if (instance.playerRoster.Count > 0)
        {
            Player _player = instance.playerRoster.Find((Player obj) => obj.playerName == checkName);
            if (_player != null) { return false; }
        }

        return true;
    }

    // Set the next player as the active player dependent on if turn order is random or not
    public void NextPlayer()
    {
        if (playersActive.Count > 0)
        {
            int randomPlayersLeft = playersActive.Count - randomPlayersCount;

            // If turn order is random set i to a random number
            int i = 0;
            if (GameOptions.instance.randomTurns)
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

        // show the next player menu
        uiMenu.ShowMenu(Menu.NewQuestion);
    }

    public void GetNewQuestion()
    {
        CardSet questionSet;

        int i = UnityEngine.Random.Range(0, activePlayer.cardSets.Count() - 1);
        questionSet = activePlayer.cardSets[i];
        questionSet.GetQuestion();
    }

    public void StartGame()
    {
        gameManager.gameInProgress = true;
        PlayerObject pObject = PlayerSelector.instance.activePlayerObject;
        if (pObject)
            pObject.SubMenu(false);

        // Update stats for all the players
        foreach (Player player in playersActive)
        {
            player.answersThisGame = 0;
            player.questionsThisGame = 0;
            player.pointsThisGame = 0;

            //Set the default card set for the player if cardSets is 0
            if (player.cardSets.Count == 0)
                player.cardSets = cardSetManager.activeCardSets;
        }

        NextPlayer();
    }

    // Update the activePlayer stats
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
        List<SaveData> playerData = new List<SaveData>();
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
            //SaveData data = new SaveData
            SaveData data = new SaveData
            {
                playerName = player.playerName,
                playerID = player.playerID,
                language = player.language,
                gamesTotal = player.gamesTotal,
                gamesWon = player.gamesWon,
                questionsTotal = player.questionsTotal,
                answersTotal = player.answersTotal,
                pointsTotal = player.pointsTotal,
            };


            foreach (CardSet cardSet in player.cardSets)
                data.cardSetList.Add(cardSet.name);

            playerData.Add(data);
        }

        // Create/write playerRoster to the save file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.OpenWrite(Application.persistentDataPath + "/SaveData.dat");
        bf.Serialize(saveFile, playerData);
        saveFile.Close();
    }

    public bool LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream loadFile = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);
            List<SaveData> playerData = (List<SaveData>)bf.Deserialize(loadFile);
            playerRoster.Clear();

            //foreach (SaveData data in playersData)
            foreach (SaveData data in playerData)
            {
                // Read in each player
                Player player = new Player()
                {
                    playerName = data.playerName,
                    playerID = data.playerID,
                    language = data.language,
                    gamesTotal = data.gamesTotal,
                    gamesWon = data.gamesWon,
                    questionsTotal = data.questionsTotal,
                    answersTotal = data.answersTotal,
                    pointsTotal = data.pointsTotal,
                    cardSetList = data.cardSetList
                };

                foreach (string cardSetName in data.cardSetList)
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
}

// TODO: Why is SaveData being used to save out players and not Player????
// Answer: Cannot Serialize List<CardSet>() for binary write! Possibly use scriptableobjects instead?
[System.Serializable]
class SaveData
{
    public string playerName;
    public string playerID;
    public SystemLanguage language;
    public int gamesTotal;
    public int gamesWon;
    public int questionsTotal;
    public int answersTotal;
    public int pointsTotal;
    public List<string> cardSetList = new List<string>();
}