using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Fireball : Spell
{
    public Spell_Fireball() : base("Fireball", new List<int> { 0, 4, 8, 2, 6 }) {}

    public override void Cast(UnitPlayer _Player)
    {
        GameObject.Instantiate(Resources.Load<GameObject>("Entities/Fireball"), _Player.transform.position, _Player.transform.rotation);
    }
}
