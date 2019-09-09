using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_InputMode
{
    void StartInputMode(UnitPlayer _Player);
    void UpdateInputMode(UnitPlayer _Player, ControllerPlayer _Controller);
    void EndInputMode(UnitPlayer _Player);
}
