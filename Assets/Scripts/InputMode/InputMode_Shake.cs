using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMode_Shake : I_InputMode
{
    private List<I_Activable> m_Activables = new List<I_Activable>();

    private bool m_HasStartedShaking;
    private float m_LastValue;
    private bool m_WasGoingForward;
    private float m_TimeGoingTheSameDirection;
    private float m_TimerBeforeCanChangeDirection;
    private int m_NumberTimesShaked;


    public void StartInputMode(UnitPlayer _Player)
    {
        m_HasStartedShaking = true;
        m_LastValue = Input.acceleration.z;
        m_TimeGoingTheSameDirection = 0;
        m_TimerBeforeCanChangeDirection = 0;
        m_WasGoingForward = true;
        UserInterface.ActivateBellRing(true);
        UserInterface.SetBellRing();
    }

    public void UpdateInputMode(UnitPlayer _Player, ControllerPlayer _Controller)
    {
        if (m_HasStartedShaking)
        {
            if (m_TimerBeforeCanChangeDirection > 0)
            {
                m_TimerBeforeCanChangeDirection -= Time.unscaledDeltaTime;
            }
            else
            {
                UserInterface.SetBellRing();
            }
            float newValue = Input.acceleration.z;
            if (m_WasGoingForward && newValue > m_LastValue + Time.unscaledDeltaTime * 3 || !m_WasGoingForward && newValue < m_LastValue - Time.unscaledDeltaTime * 3)
            {
                m_TimeGoingTheSameDirection += Time.unscaledDeltaTime;
            }
            else if (m_WasGoingForward && newValue < m_LastValue || !m_WasGoingForward && newValue > m_LastValue)
            {
                if (m_TimerBeforeCanChangeDirection <= 0)
                {
                    m_TimerBeforeCanChangeDirection = 0.25f;
                    if (m_TimeGoingTheSameDirection > 0.08f)
                    {
                        if (newValue < m_LastValue)
                        {
                            UserInterface.SetBellRingLeft();
                        }
                        else
                        {
                            UserInterface.SetBellRingRight();
                        }
                        m_NumberTimesShaked++;
                        if (m_NumberTimesShaked > 10)
                        {
                            m_HasStartedShaking = false;
                            for (int i = 0; i < m_Activables.Count; i++)
                            {
                                m_Activables[i].OnActivation();
                            }
                            _Controller.SetInputMode(new InputMode_Movement());
                        }
                    }
                }
                m_TimeGoingTheSameDirection = 0;
                m_WasGoingForward = !m_WasGoingForward;
            }
            m_LastValue = newValue;
        }
    }

    public void EndInputMode(UnitPlayer _Player)
    {
        UserInterface.ActivateBellRing(false);
        m_HasStartedShaking = false;
    }

    public void RegisterActivable(I_Activable _Activable)
    {
        m_Activables.Add(_Activable);
    }
}
