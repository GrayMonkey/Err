using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerContent : MonoBehaviour
{
    public static LoadPlayerContent loadPlayer;

    [SerializeField] GameObject loadPlayerButton;

    PlayerController playerController;
    List<Player> playersActive;
    List<Player> playerRoster;

    private void Start()
    {
        playerController = PlayerController.playerController;
        playerRoster = playerController.playerRoster;
    }

    private void PopulateList()
    {
        playersActive = playerController.playersActive;

        foreach (Player player in playerRoster)
        {
            if(!playersActive.Contains(player))
            {
                GameObject newPlayer = Instantiate(loadPlayerButton, transform);
                newPlayer.GetComponent<LoadPlayerButton>().AttachPlayer(player);
            }
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}