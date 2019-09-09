using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, I_Unit
{
    [SerializeField]
    private bool m_MustSpawnRandomly;

    [SerializeField]
    protected Map m_Map;

    protected virtual void Start()
    {
        Health = m_MaxHealth;
    }

    protected virtual void Update()
    {
        if (m_IsMoving)
        {
            float movementAvailable = m_Speed * Time.deltaTime;
            Vector3 target = m_Tile.GetPosition();
            Vector3 direction = target - transform.position;
            if (direction != Vector3.zero)
            {
                transform.forward = direction.normalized;
            }
            float distanceLeft = direction.magnitude;
            Vector3 finalPosition = new Vector3();
            if (distanceLeft < movementAvailable)
            {
                m_IsMoving = false;
                finalPosition = target;
            }
            else
            {
                finalPosition = transform.position + direction.normalized * movementAvailable;
            }
            transform.position = finalPosition;
        }
    }

    [SerializeField]
    private float m_Speed = 2.0f;

    protected I_Tile m_Tile;
    private bool m_IsMoving = false;

    [SerializeField]
    private int m_MaxHealth;
    private int m_CurrentHealth;

    private List<I_UnitHealthObserver> m_LifeObservers = new List<I_UnitHealthObserver>();

    public int Health
    {
        get
        {
            return m_CurrentHealth;
        }
        protected set
        {
            m_CurrentHealth = value;
            if (m_CurrentHealth > m_MaxHealth)
            {
                m_CurrentHealth = m_MaxHealth;
            }
            else if (m_CurrentHealth < 0)
            {
                m_CurrentHealth = 0;
            }
            for (int i = 0; i < m_LifeObservers.Count; i++)
            {
                m_LifeObservers[i].OnUnitHealthChange(this);
            }
        }
    }

    public int MaxHealth
    {
        get
        {
            return m_MaxHealth;
        }
        protected set
        {
            m_MaxHealth = value;
            if (m_CurrentHealth > m_MaxHealth)
            {
                m_CurrentHealth = m_MaxHealth;
            }
            for (int i = 0; i < m_LifeObservers.Count; i++)
            {
                m_LifeObservers[i].OnUnitHealthChange(this);
            }
        }
    }

    public bool CanMove()
    {
        return !m_IsMoving;
    }

    public bool CanMoveTo(E_Direction _Direction)
    {
        I_Tile tile = m_Tile.GetNeighbour(_Direction);
        return (tile != null && CanBeOnTile(tile));
    }

    public E_TileAction Move(E_Direction _Direction)
    {
        E_TileAction action = E_TileAction.None;
        I_Tile tile = m_Tile.GetNeighbour(_Direction);
        if (tile != null)
        {
            m_IsMoving = true;
            OnTileExit(m_Tile);
            m_Tile = tile;
            OnTileEnter(m_Tile);
            m_Tile.OnUnitEnter(this);
        }
        return action;
    }

    public void TurnToward(E_Direction _Direction)
    {
        if (_Direction == E_Direction.North)
        {
            transform.forward = Vector3.forward;
        }
        else if (_Direction == E_Direction.South)
        {
            transform.forward = -Vector3.forward;
        }
        else if (_Direction == E_Direction.East)
        {
            transform.forward = Vector3.right;
        }
        else if (_Direction == E_Direction.West)
        {
            transform.forward = -Vector3.right;
        }
    }

    public I_Tile GetTile()
    {
        return m_Tile;
    }

    public void TeleportTo(I_Tile _Tile)
    {
        if (m_Tile != null)
        {
            m_Tile.SetUnit(null);
            OnTileExit(m_Tile);
        }
        m_Tile = _Tile;

        transform.position = m_Tile.GetPosition();
        m_Tile.SetUnit(this);
        OnTileEnter(m_Tile);
    }

    public bool IsMoving()
    {
        return m_IsMoving;
    }

    public abstract void OnTileEnter(I_Tile _Tile);
    public abstract void OnTileExit(I_Tile _Tile);

    public abstract bool CanBeOnTile(I_Tile _Tile);

    public abstract void OnDamageTaken();

    public abstract void Kill();

    public void TakeDamages(int _Damages)
    {
        Health -= _Damages;
        OnDamageTaken();
    }

    public void Heal(int _Heal)
    {
        Health += _Heal;
    }

    public void RegisterHealthObserver(I_UnitHealthObserver _Observer)
    {
        m_LifeObservers.Add(_Observer);
    }

    public void UnregisterHealthObserver(I_UnitHealthObserver _Observer)
    {
        m_LifeObservers.Remove(_Observer);
    }
}
