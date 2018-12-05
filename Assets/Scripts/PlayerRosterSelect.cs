using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRosterSelect : MonoBehaviour
{
    public static PlayerRosterSelect playerRosterSelect;

    [SerializeField] GameObject playerRosterObject;
    [SerializeField] Transform PlayerRosterContent;
    PlayerController playerController;
    List<Player> playersActive;
    List<Player> playerRoster;

    private void Awake()
    {
        playerRosterSelect = this;
    }

    private void Start()
    {
        playerController = PlayerController.playerController;
        playerRoster = playerController.playerRoster;
        PopulateList();
    }

    private void PopulateList()
    {
        playersActive = playerController.playersActive;

        foreach (Player player in playerRoster)
        {
            if (!playersActive.Contains(player))
            {
                GameObject newPlayer = Instantiate(playerRosterObject, transform);
                newPlayer.transform.SetParent(PlayerRosterContent);
                newPlayer.GetComponent<PlayerRosterObject>().AttachPlayer(player);
            }
        }
    }
}