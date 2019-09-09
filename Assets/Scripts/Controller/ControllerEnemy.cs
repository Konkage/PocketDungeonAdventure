using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEnemy : MonoBehaviour, I_Controller
{
    private float m_TimerMove;
    private UnitEnemy m_Enemy;
    private E_Direction m_MovingDirection = E_Direction.North;

    protected void Start()
    {
        TurnOrder.RegisterUnit(this);
        m_Enemy = GetComponent<UnitEnemy>();
    }

    public void StartTurn()
    {
        if (m_MovingDirection != E_Direction.None && !m_Enemy.IsMoving())
        {
            I_Tile currentTile = m_Enemy.GetTile();
            if (currentTile != null)
            {
                bool isPlayerAtNorth = currentTile.GetNeighbour(E_Direction.North).GetUnit() is UnitPlayer;
                bool isPlayerAtSouth = currentTile.GetNeighbour(E_Direction.South).GetUnit() is UnitPlayer;
                bool isPlayerAtEast = currentTile.GetNeighbour(E_Direction.East).GetUnit() is UnitPlayer;
                bool isPlayerAtWest = currentTile.GetNeighbour(E_Direction.West).GetUnit() is UnitPlayer;
                if (isPlayerAtNorth || isPlayerAtSouth || isPlayerAtEast || isPlayerAtWest)
                {
                    if (isPlayerAtNorth)
                    {
                        m_Enemy.TurnToward(E_Direction.North);
                        UnitPlayer player = currentTile.GetNeighbour(E_Direction.North).GetUnit() as UnitPlayer;
                        if (!player.IsAttackedByEnemy())
                        {
                            player.SetAttackingEnemy(m_Enemy);
                        }
                    }
                    else if (isPlayerAtSouth)
                    {
                        m_Enemy.TurnToward(E_Direction.South);
                        UnitPlayer player = currentTile.GetNeighbour(E_Direction.South).GetUnit() as UnitPlayer;
                        if (!player.IsAttackedByEnemy())
                        {
                            player.SetAttackingEnemy(m_Enemy);
                        }
                    }
                    else if (isPlayerAtEast)
                    {
                        m_Enemy.TurnToward(E_Direction.East);
                        UnitPlayer player = currentTile.GetNeighbour(E_Direction.East).GetUnit() as UnitPlayer;
                        if (!player.IsAttackedByEnemy())
                        {
                            player.SetAttackingEnemy(m_Enemy);
                        }
                    }
                    else if (isPlayerAtWest)
                    {
                        m_Enemy.TurnToward(E_Direction.West);
                        UnitPlayer player = currentTile.GetNeighbour(E_Direction.West).GetUnit() as UnitPlayer;
                        if (!player.IsAttackedByEnemy())
                        {
                            player.SetAttackingEnemy(m_Enemy);
                        }
                    }
                }
                else
                {
                    m_TimerMove -= Time.deltaTime;
                    if (m_TimerMove < 0)
                    {
                        List<E_Direction> directionsPossible = new List<E_Direction>();
                        if (m_MovingDirection != E_Direction.South && m_Enemy.CanMoveTo(E_Direction.North))
                        {
                            directionsPossible.Add(E_Direction.North);
                        }
                        if (m_MovingDirection != E_Direction.North && m_Enemy.CanMoveTo(E_Direction.South))
                        {
                            directionsPossible.Add(E_Direction.South);
                        }
                        if (m_MovingDirection != E_Direction.West && m_Enemy.CanMoveTo(E_Direction.East))
                        {
                            directionsPossible.Add(E_Direction.East);
                        }
                        if (m_MovingDirection != E_Direction.East && m_Enemy.CanMoveTo(E_Direction.West))
                        {
                            directionsPossible.Add(E_Direction.West);
                        }
                        if (directionsPossible.Count > 0)
                        {
                            m_MovingDirection = directionsPossible[Random.Range(0, directionsPossible.Count)];
                            m_TimerMove = Random.Range(1.0f, 3.0f);
                        }
                    }
                    if (m_Enemy.CanMoveTo(m_MovingDirection))
                    {
                        m_Enemy.Move(m_MovingDirection);
                    }
                    else
                    {
                        if (!m_Enemy.GetTile().GetNeighbour(m_MovingDirection).IsWalkable() || m_Enemy.GetTile().GetNeighbour(m_MovingDirection).GetUnit() is UnitEnemy)
                        {
                            List<E_Direction> directionsPossible = new List<E_Direction>();
                            if (m_MovingDirection != E_Direction.North && m_Enemy.CanMoveTo(E_Direction.North))
                            {
                                directionsPossible.Add(E_Direction.North);
                            }
                            if (m_MovingDirection != E_Direction.South && m_Enemy.CanMoveTo(E_Direction.South))
                            {
                                directionsPossible.Add(E_Direction.South);
                            }
                            if (m_MovingDirection != E_Direction.East && m_Enemy.CanMoveTo(E_Direction.East))
                            {
                                directionsPossible.Add(E_Direction.East);
                            }
                            if (m_MovingDirection != E_Direction.West && m_Enemy.CanMoveTo(E_Direction.West))
                            {
                                directionsPossible.Add(E_Direction.West);
                            }
                            if (directionsPossible.Count > 0)
                            {
                                m_MovingDirection = directionsPossible[Random.Range(0, directionsPossible.Count)];
                            }
                        }
                    }
                }
            }
            else
            {
                TurnOrder.RemoveUnit(this);
            }
        }
    }

    private void OnDestroy()
    {
        TurnOrder.RemoveUnit(this);
    }
}
