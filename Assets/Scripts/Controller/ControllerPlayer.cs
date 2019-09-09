using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayer : MonoBehaviour, I_OnPlayerAttackedEventReciever
{
    private Animator m_Animator;

    private E_Direction m_MovingDirection = E_Direction.None;
    private E_Direction m_DirectionBuffer = E_Direction.None;
    private UnitPlayer m_Player;

    private bool m_HasStartedSwipe;
    private Vector2 m_SwipeStartPosition;

    private I_InputMode m_InputMode;

    private void Start()
    {
        m_Player = GetComponent<UnitPlayer>();
        m_Animator = GetComponent<Animator>();
        m_Player.SubscribeToBeingAttackedEvent(this);

        if (Application.isMobilePlatform)
        {
            m_InputMode = new InputMode_Movement();
        }
        else
        {
            m_InputMode = new InputMode_MovementDebug();
        }
    }

    public void SetAnimatorBool(string _Name, bool _Value)
    {
        m_Animator.SetBool(_Name, _Value);
    }

    private void Update()
    {
        m_InputMode.UpdateInputMode(m_Player, this);
    }

    public void OpenMagicalCircleTablet()
    {
        if (Application.isMobilePlatform)
        {
            if (m_InputMode is InputMode_Movement)
            {
                SetInputMode(new InputMode_MagicalCircle());
            }
            else
            {
                SetInputMode(new InputMode_Movement());
            }
        }
        else
        {
            if (m_InputMode is InputMode_MovementDebug)
            {
                SetInputMode(new InputMode_MagicalCircleDebug());
            }
            else
            {
                SetInputMode(new InputMode_MovementDebug());
            }
        }
    }

    public void SetInputMode(I_InputMode _InputMode)
    {
        if (m_InputMode != null)
        {
            m_InputMode.EndInputMode(m_Player);
        }
        m_InputMode = _InputMode;
        if (m_InputMode != null)
        {
            m_InputMode.StartInputMode(m_Player);
        }
    }

    public void OnPlayerAttacked(UnitEnemy _Enemy)
    {
        if (_Enemy != null)
        {
            m_Player.TakeDamages(1);
            m_Player.AttackEnemy();
        }
        /*
        if (Application.isMobilePlatform)
        {
            if (_Enemy != null)
            {
                SetInputMode(new InputMode_MeleeBattle());
            }
            else
            {
                SetInputMode(new InputMode_Movement());
            }
        }
        else
        {
            if (_Enemy != null)
            {
                SetInputMode(new InputMode_MeleeBattleDebug());
            }
            else
            {
                SetInputMode(new InputMode_MovementDebug());
            }
        }*/
    }
}
