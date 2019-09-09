using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileEventDoToNextFloor : TileEvent
{
    private ControllerPlayer m_Player;

    public override void ActivateEvent(I_Unit _UnitThatWalkedOnTile)
    {
        if (_UnitThatWalkedOnTile is UnitPlayer)
        {
            UnitPlayer player = _UnitThatWalkedOnTile as UnitPlayer;
            m_Player = player.GetComponent<ControllerPlayer>();
            m_Player.SetInputMode(new InputMode_InMenu());
            List<KeyValuePair<string, OnUserInterfaceButtonPressed>> menuElements = new List<KeyValuePair<string, OnUserInterfaceButtonPressed>> { new KeyValuePair<string, OnUserInterfaceButtonPressed>("Proceed", GoToNextFloor), new KeyValuePair<string, OnUserInterfaceButtonPressed>("Exit", ExitMenu) };
            UserInterface.DisplayMenu(menuElements);
        }
    }

    public void GoToNextFloor()
    {
        UserInterface.DisplayLoadingScreen(true);
        UserInterface.HideMenu();
        if (Application.isMobilePlatform)
        {
            m_Player.SetInputMode(new InputMode_Movement());
        }
        else
        {
            m_Player.SetInputMode(new InputMode_MovementDebug());
        }
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ExitMenu()
    {
        if (Application.isMobilePlatform)
        {
            m_Player.SetInputMode(new InputMode_Movement());
        }
        else
        {
            m_Player.SetInputMode(new InputMode_MovementDebug());
        }
        UserInterface.HideMenu();
    }
}
