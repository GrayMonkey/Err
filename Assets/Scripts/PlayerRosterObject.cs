using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRosterObject : MonoBehaviour
{
    [SerializeField] Text playerName;

    PlayerController playerController;
    Player player;

	// Use this for initialization
	private void Start () 
    {
        playerController = PlayerController.playerController;
	}

    public void AttachPlayer (Player _player)
    {
        player = _player;
        playerName.text = player.playerName;
    }

     public void  AddPlayerToGame()
    {
        playerController.playersActive.Add(player);
    }
}
