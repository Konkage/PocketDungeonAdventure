using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMode_MagicalCircle : I_InputMode
{
    public void StartInputMode(UnitPlayer _Player)
    {
        UserInterface.ActivateMagicalFormula(true);
    }

    public void UpdateInputMode(UnitPlayer _Player, ControllerPlayer _Controller)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UserInterface.StartMagicalFormulaDrawing(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            List<int> magicalFormula = UserInterface.EndMagicalFormulaDrawing(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            Spell spellToCast = _Player.GetCorrespondingSpell(magicalFormula);
            if (spellToCast != null)
            {
                _Controller.SetInputMode(new InputMode_MovementDebug());
                spellToCast.Cast(_Player);
            }
        }
        UserInterface.UpdateMagicalFormulaDrawing(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    public void EndInputMode(UnitPlayer _Player)
    {
        UserInterface.EndMagicalFormulaDrawing(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        UserInterface.ActivateMagicalFormula(false);
    }
}
