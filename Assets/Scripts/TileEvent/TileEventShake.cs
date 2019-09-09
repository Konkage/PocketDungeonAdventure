using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEventShake : TileEvent
{
    private ControllerPlayer m_Player;
    private I_Activable m_Activable;

    public override void ActivateEvent(I_Unit _UnitThatWalkedOnTile)
    {
        if (_UnitThatWalkedOnTile is UnitPlayer)
        {
            UnitPlayer player = _UnitThatWalkedOnTile as UnitPlayer;
            m_Player = player.GetComponent<ControllerPlayer>();
            if (Application.isMobilePlatform)
            {
                InputMode_Shake inputMode = new InputMode_Shake();
                inputMode.RegisterActivable(m_Activable);
                m_Player.SetInputMode(inputMode);
            }
            else
            {
                InputMode_ShakeDebug inputMode = new InputMode_ShakeDebug();
                inputMode.RegisterActivable(m_Activable);
                m_Player.SetInputMode(inputMode);
            }
        }
    }

    public void SetTileToActivate(I_Activable _Activable)
    {
        m_Activable = _Activable;
    }
}
