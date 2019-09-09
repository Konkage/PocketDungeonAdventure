using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeDisplayer : MonoBehaviour
{
    [SerializeField]
    private Image m_Display;

    [SerializeField]
    private Sprite m_Ring;
    [SerializeField]
    private Sprite m_RingLeft;
    [SerializeField]
    private Sprite m_RingRight;

    private ControllerPlayer m_Player;
    private bool m_HasStartedShaking;
    private float m_LastValue;
    private bool m_WasGoingForward;
    private float m_TimeGoingTheSameDirection;
    private float m_TimerBeforeCanChangeDirection;
    private int m_NumberTimesShaked;
    
    public void DisplayShakeNone()
    {
        m_Display.sprite = m_Ring;
    }

    public void DisplayShakeLeft()
    {
        m_Display.sprite = m_RingLeft;
    }

    public void DisplayShakeRight()
    {
        m_Display.sprite = m_RingRight;
    }
}
