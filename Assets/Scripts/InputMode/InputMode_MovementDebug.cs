using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMode_MovementDebug : I_InputMode
{
    private E_Direction m_Direction;

    public void StartInputMode(UnitPlayer _Player)
    {
        m_Direction = E_Direction.None;
    }

    public void UpdateInputMode(UnitPlayer _Player, ControllerPlayer _Controller)
    {
        m_Direction = E_Direction.None;
        if (Input.GetKey(KeyCode.Z))
        {
            m_Direction = E_Direction.North;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_Direction = E_Direction.South;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_Direction = E_Direction.East;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            m_Direction = E_Direction.West;
        }

        if (_Player.CanMove())
        {
            if (m_Direction != E_Direction.None)
            {
                I_Unit unitOnTile = _Player.GetTile().GetNeighbour(m_Direction).GetUnit();
                if (_Player.CanMoveTo(m_Direction))
                {
                    _Player.Move(m_Direction);
                    TurnOrder.StartAllUnitsTurn();
                }
                else if (unitOnTile is UnitEnemy)
                {
                    _Player.SetAttackingEnemy(unitOnTile as UnitEnemy);
                }
                else
                {
                    _Player.TurnToward(m_Direction);
                }
            }
        }
        _Controller.SetAnimatorBool("IsMoving", _Player.IsMoving());
    }

    public void EndInputMode(UnitPlayer _Player)
    {
    }
}
