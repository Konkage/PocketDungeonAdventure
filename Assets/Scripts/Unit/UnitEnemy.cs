using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnemy : Unit
{
    public void LoadFromData(I_Map _Map, EnemyData _EnemyData)
    {
        if (_EnemyData != null)
        {
            MaxHealth = _EnemyData.MaxHealth;
            Health = _EnemyData.CurrentHealth;

            if (_Map != null)
            {
                TeleportTo(_Map.GetTileAt(_EnemyData.PosX, _EnemyData.PosZ));
            }
        }
    }

    public EnemyData GetEnemySave()
    {
        Vector3 tilePosition = GetTile().GetPosition();
        EnemyData enemyData = new EnemyData
        {
            CurrentHealth = Health,
            MaxHealth = MaxHealth,
            Level = 1,
            PosX = Mathf.RoundToInt(tilePosition.x),
            PosZ = Mathf.RoundToInt(tilePosition.z)
        };
        return enemyData;
    }

    public override bool CanBeOnTile(I_Tile _Tile)
    {
        return (_Tile.IsWalkable() && _Tile.GetUnit() == null);
    }

    public override void OnTileEnter(I_Tile _Tile)
    {
        _Tile.SetUnit(this);
    }

    public override void Kill()
    {
        EnemyManager.SpawnEnemyRandomly(GetTile().GetMap(), 1);
        OnTileExit(GetTile());
        Destroy(gameObject);
    }

    public override void OnTileExit(I_Tile _Tile)
    {
        _Tile.SetUnit(null);
    }

    public override void OnDamageTaken()
    {
    }
}
