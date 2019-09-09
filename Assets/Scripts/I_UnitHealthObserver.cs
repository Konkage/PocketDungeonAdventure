using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_UnitHealthObserver
{
    void OnUnitHealthChange(I_Unit _Unit);
}
