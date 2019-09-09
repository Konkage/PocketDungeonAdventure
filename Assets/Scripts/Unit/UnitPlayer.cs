using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitPlayer : Unit
{
    private DateTime m_LastRegenTime;

    [SerializeField]
    private UnitLifebar m_Lifebar;
    [SerializeField]
    private int m_HealthRegen;
    [SerializeField]
    private int m_TimeBetweenHealthRegen;
    private float m_HealthRegenTimer;

    private UnitEnemy m_Enemy = null;
    private List<Spell> m_SpellList = new List<Spell>{new Spell_Fireball()};
    private List<I_OnPlayerAttackedEventReciever> m_OnPlayerAttackedEventRecievers = new List<I_OnPlayerAttackedEventReciever>();

    public void StartRandomly(I_Map _Map)
    {
        if (_Map != null)
        {
            TeleportTo(_Map.GetRandomWalkableTile());
            m_LastRegenTime = DateTime.Now;
        }
        m_Lifebar.DisplayUnitLife(this);
    }

    public void LoadLastPosition(I_Map _Map, PlayerData _PlayerData)
    {
        if (_PlayerData != null)
        {
            MaxHealth = _PlayerData.MaxHealth;
            Health = _PlayerData.CurrentHealth;

            if (_Map != null)
            {
                TeleportTo(_Map.GetTileAt(_PlayerData.PosX, _PlayerData.PosZ));
            }

            long lastSavedRegenTime = Convert.ToInt64(_PlayerData.LastRegenTime);
            m_LastRegenTime = DateTime.FromBinary(lastSavedRegenTime);
            RegenHealthSinceLastRegen();
        }
        m_Lifebar.DisplayUnitLife(this);
    }

    protected override void Update()
    {
        base.Update();
        m_HealthRegenTimer -= Time.deltaTime;
        if (m_HealthRegenTimer <= 0)
        {
            m_LastRegenTime = DateTime.Now;
            m_HealthRegenTimer = m_TimeBetweenHealthRegen;
            Heal(m_HealthRegen);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            MaxHealth++;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            MaxHealth--;
        }
    }

    public void SetAttackingEnemy(UnitEnemy _Enemy)
    {
        m_Enemy = _Enemy;
        for (int i = 0; i < m_OnPlayerAttackedEventRecievers.Count; i++)
        {
            m_OnPlayerAttackedEventRecievers[i].OnPlayerAttacked(m_Enemy);
        }
    }

    public bool IsAttackedByEnemy()
    {
        return m_Enemy != null;
    }

    public void AttackEnemy()
    {
        if (m_Enemy != null)
        {
            m_Enemy.Kill();
            SetAttackingEnemy(null);
        }
    }

    public override bool CanBeOnTile(I_Tile _Tile)
    {
        return (_Tile.IsWalkable() && _Tile.GetUnit() == null);
    }

    public override void Kill()
    {
    }

    public override void OnTileEnter(I_Tile _Tile)
    {
        _Tile.SetUnit(this);
    }

    public override void OnTileExit(I_Tile _Tile)
    {
        _Tile.SetUnit(null);
    }

    public void SubscribeToBeingAttackedEvent(I_OnPlayerAttackedEventReciever _EventReciever)
    {
        m_OnPlayerAttackedEventRecievers.Add(_EventReciever);
    }
    
    public Spell GetCorrespondingSpell(List<int> _Formula)
    {
        Spell spell = null;
        for (int i = 0; i < m_SpellList.Count && spell == null; i++)
        {
            if (m_SpellList[i].IsFormulaValid(_Formula))
            {
                spell = m_SpellList[i];
            }
        }
        return spell;
    }

    private void RegenHealthSinceLastRegen()
    {
        DateTime now = DateTime.Now;
        TimeSpan timeBetweenLastRegenAndNow = now.Subtract(m_LastRegenTime);
        int healthPointToRegen = Mathf.RoundToInt((float)timeBetweenLastRegenAndNow.TotalSeconds / m_TimeBetweenHealthRegen);
        Heal(healthPointToRegen * m_HealthRegen);
        m_HealthRegenTimer = m_TimeBetweenHealthRegen;
        m_LastRegenTime = now;
    }

    private void OnApplicationFocus(bool _Focus)
    {
        if (_Focus)
        {
            Time.timeScale = 1;
            RegenHealthSinceLastRegen();
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void SavePlayer()
    {
        Vector3 tilePosition = GetTile().GetPosition();
        PlayerData playerData = new PlayerData
        {
            CurrentHealth = Health,
            MaxHealth = MaxHealth,
            GemCount = 0,
            Level = 1,
            LastRegenTime = m_LastRegenTime.ToBinary().ToString(),
            PosX = Mathf.RoundToInt(tilePosition.x),
            PosZ = Mathf.RoundToInt(tilePosition.z)
        };
        SaveManager.SavePlayer(playerData);
    }

    public override void OnDamageTaken()
    {
        m_LastRegenTime = DateTime.Now;
    }
}
