using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMode_MeleeBattle : I_InputMode
{
    private bool m_HasStartedSlash;
    private Vector2 m_SlashStartPosition;

    public void StartInputMode(UnitPlayer _Player)
    {
        UserInterface.ActivateMeleeFight(true);
        UserInterface.SetMeleeFightWeakness(0.3f, 0.2f, 1.0f);
        m_HasStartedSlash = false;
        m_SlashStartPosition = new Vector2();
    }

    public void UpdateInputMode(UnitPlayer _Player, ControllerPlayer _Controller)
    {
        if (Input.touchCount >= 1)
        {
            Touch touch = Input.touches[0];
            if (!m_HasStartedSlash)
            {
                m_HasStartedSlash = true;
                m_SlashStartPosition = touch.position;
            }
            UserInterface.DisplaySlashLine(m_SlashStartPosition, touch.position);
            if (touch.phase == TouchPhase.Canceled)
            {
                m_HasStartedSlash = false;
                UserInterface.HideSlashLine();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                m_HasStartedSlash = false;
                if (UserInterface.IsMeleeSlashHitting(m_SlashStartPosition, touch.position))
                {
                    _Player.AttackEnemy();
                }
                UserInterface.HideSlashLine();
            }
        }

    }

    public void EndInputMode(UnitPlayer _Player)
    {
        UserInterface.ActivateMeleeFight(false);
    }
}
