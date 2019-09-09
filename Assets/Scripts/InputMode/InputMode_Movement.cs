using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMode_Movement : I_InputMode
{
    private E_Direction m_MovingDirection;
    private E_Direction m_DirectionBuffer;

    private Vector2 m_SwipeStartPosition;
    private bool m_HasStartedSwipe;
    
    public void StartInputMode(UnitPlayer _Player)
    {
        m_MovingDirection = E_Direction.None;
        m_DirectionBuffer = E_Direction.None;
        m_SwipeStartPosition = new Vector2();
        m_HasStartedSwipe = false;
    }

    public void UpdateInputMode(UnitPlayer _Player, ControllerPlayer _Controller)
    {
        if (Input.touchCount >= 1)
        {
            Touch touch = Input.touches[0];
            if (!m_HasStartedSwipe)
            {
                m_HasStartedSwipe = true;
                m_SwipeStartPosition = touch.position;
            }
            UserInterface.DisplayMovementLine(m_SwipeStartPosition, touch.position);
            if (touch.phase == TouchPhase.Canceled)
            {
                m_HasStartedSwipe = false;
                UserInterface.HideMovementLine();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Vector2 swipeDirection = touch.position - m_SwipeStartPosition;
                if (swipeDirection.sqrMagnitude > 100)
                {
                    if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                    {
                        if (swipeDirection.x > 0)
                        {
                            m_DirectionBuffer = E_Direction.East;
                        }
                        else
                        {
                            m_DirectionBuffer = E_Direction.West;
                        }
                    }
                    else
                    {
                        if (swipeDirection.y > 0)
                        {
                            m_DirectionBuffer = E_Direction.North;
                        }
                        else
                        {
                            m_DirectionBuffer = E_Direction.South;
                        }
                    }
                }
                m_HasStartedSwipe = false;
                UserInterface.HideMovementLine();
            }
        }

        if (_Player.CanMove())
        {
            if (m_DirectionBuffer != E_Direction.None && _Player.CanMoveTo(m_DirectionBuffer))
            {
                m_MovingDirection = m_DirectionBuffer;
                m_DirectionBuffer = E_Direction.None;
            }
            if (m_MovingDirection != E_Direction.None)
            {
                I_Unit unitOnTile = _Player.GetTile().GetNeighbour(m_MovingDirection).GetUnit();
                if (_Player.CanMoveTo(m_MovingDirection))
                {
                    _Player.Move(m_MovingDirection);
                    TurnOrder.StartAllUnitsTurn();
                }
                else if (unitOnTile is UnitEnemy)
                {
                    _Player.SetAttackingEnemy(unitOnTile as UnitEnemy);
                }
                else
                {
                    m_MovingDirection = E_Direction.None;
                }
            }
        }

        _Controller.SetAnimatorBool("IsMoving", _Player.IsMoving());
    }

    public void EndInputMode(UnitPlayer _Player)
    {
        m_HasStartedSwipe = false;
        UserInterface.HideMovementLine();
    }
}
