using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProjectile : Unit
{
    public override bool CanBeOnTile(I_Tile _Tile)
    {
        return (_Tile.IsWalkable());
    }

    public override void OnTileEnter(I_Tile _Tile)
    {
        I_Unit unit = _Tile.GetUnit();
        if (unit != null && unit is UnitEnemy)
        {
            unit.Kill();
            Kill();
        }
    }

    public override void OnTileExit(I_Tile _Tile) {}

    public override void Kill()
    {
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();
        I_Unit unit = GetTile().GetUnit();
        if (unit != null && unit is UnitEnemy)
        {
            unit.Kill();
            Kill();
        }
    }

    public override void OnDamageTaken()
    {
    }
}
