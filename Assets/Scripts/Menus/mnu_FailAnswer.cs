using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class mnu_FailAnswer : MonoBehaviour
{
    [SerializeField] Text playerName;
    [SerializeField] GameObject qCard;

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

    public void Return()
    {
        qCard.SetActive(true);
        uiMenus.CloseMenu(Menus.FailAnswer);
    }

    public void NextPlayer()
    {
        //Return();
        uiMenus.CloseMenu(Menus.FailAnswer);
        activePlayer.questionsThisGame++;
        playerController.NextPlayer();
    }
}
