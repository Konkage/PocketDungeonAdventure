using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMode_ShakeDebug : I_InputMode
{
    private List<I_Activable> m_Activables = new List<I_Activable>();
    private bool m_GoingRight;

    private int m_TimesShaked;

    public void StartInputMode(UnitPlayer _Player)
    {
        UserInterface.ActivateBellRing(true);
        UserInterface.SetBellRing();
        m_TimesShaked = 0;
    }

    public void UpdateInputMode(UnitPlayer _Player, ControllerPlayer _Controller)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (m_GoingRight)
            {
                UserInterface.SetBellRingLeft();
                m_TimesShaked++;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_GoingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!m_GoingRight)
            {
                UserInterface.SetBellRingRight();
                m_TimesShaked++;
            }
        }
        else if (Input.GetKey(KeyCode.E))
        {
            m_GoingRight = true;
        }
        else
        {
            UserInterface.SetBellRing();
        }

        if (m_TimesShaked > 10)
        {
            for (int i = 0; i < m_Activables.Count; i++)
            {
                m_Activables[i].OnActivation();
            }
            _Controller.SetInputMode(new InputMode_MovementDebug());
        }
    }

    public void EndInputMode(UnitPlayer _Player)
    {
        UserInterface.ActivateBellRing(false);
    }

    public void RegisterActivable(I_Activable _Activable)
    {
        m_Activables.Add(_Activable);
    }
}
