using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerProjectile : MonoBehaviour, I_Controller
{
    private UnitProjectile m_Projectile;
    private E_Direction m_MovingDirection = E_Direction.None;

    protected void Start()
    {
        TurnOrder.RegisterUnit(this);
        m_Projectile = GetComponent<UnitProjectile>();
        float rotation = transform.rotation.eulerAngles.y;
        while (rotation < 0)
        {
            rotation += 360;
        }
        while (rotation > 360)
        {
            rotation -= 360;
        }
        if ((rotation <= 45 && rotation >= 0) || (rotation <= 360 && rotation > 315))
        {
            m_MovingDirection = E_Direction.North;
        }
        else if ((rotation > 45 && rotation <= 135))
        {
            m_MovingDirection = E_Direction.East;
        }
        else if ((rotation > 135 && rotation <= 225))
        {
            m_MovingDirection = E_Direction.South;
        }
        else
        {
            m_MovingDirection = E_Direction.West;
        }
    }

    public void StartTurn()
    {
        if (m_MovingDirection != E_Direction.None)
        {
            if (m_Projectile.CanMoveTo(m_MovingDirection))
            {
                m_Projectile.Move(m_MovingDirection);
            }
            else
            {
                if (!m_Projectile.GetTile().GetNeighbour(m_MovingDirection).IsWalkable())
                {
                    m_Projectile.Kill();
                }
            }
        }
    }

    private void OnDestroy()
    {
        TurnOrder.RemoveUnit(this);
    }
}
