using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class mnu_FailAnswer : MonoBehaviour
{
    [SerializeField] Text playerName;

    PlayerController playerController;
    MenuHandler uiMenus;
    Player activePlayer;

    private void Awake()
    {
        playerController = PlayerController.playerController;
        uiMenus = MenuHandler.uiMenus;
    }

    private void OnEnable()
    {
        activePlayer = playerController.activePlayer;
        playerName.text = activePlayer.playerName;
    }

    public void CloseMenu()
    {
        uiMenus.CloseMenu(Menus.FailAnswer);
    }

    public void NextPlayer()
    {
        CloseMenu();
        activePlayer.questionsThisGame++;
        playerController.NextPlayer();
    }
}
