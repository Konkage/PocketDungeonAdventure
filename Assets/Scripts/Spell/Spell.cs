using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    private string m_SpellName;
    private List<int> m_PointOrder;

    public Spell(string _SpellName, List<int> _PointOrder)
    {
        m_SpellName = _SpellName;
        m_PointOrder = _PointOrder;
    }

    public bool IsFormulaValid(List<int> _PointOrder)
    {
        bool isFormulaValid = false;
        if (_PointOrder != null && _PointOrder.Count == m_PointOrder.Count + 1)
        {
            List<int> allStartingIndex = new List<int>();
            for (int i = 0; i < _PointOrder.Count; i++)
            {
                if (_PointOrder[i] == m_PointOrder[0])
                {
                    allStartingIndex.Add(i);
                }
            }
            for (int i = 0; i < allStartingIndex.Count && !isFormulaValid; i++)
            {
                isFormulaValid = IsFormulaInList(_PointOrder, allStartingIndex[i]);
            }
        }
        return isFormulaValid;
    }

    private bool IsFormulaInList(List<int> _PointOrder, int startingIndex)
    {
        return IsFormulaInListGoingOnward(_PointOrder, startingIndex) || IsFormulaInListGoingBackward(_PointOrder, startingIndex);
    }

    private bool IsFormulaInListGoingOnward(List<int> _PointOrder, int startingIndex)
    {
        bool isFormulaInList = true;
        int customIndex = startingIndex;
        for (int i = 0; i < m_PointOrder.Count && isFormulaInList; i++)
        {
            isFormulaInList = _PointOrder[customIndex] == m_PointOrder[i];
            customIndex++;
            if (customIndex >= _PointOrder.Count)
            {
                customIndex = 1;
            }
        }
        return isFormulaInList;
    }

    private bool IsFormulaInListGoingBackward(List<int> _PointOrder, int startingIndex)
    {
        bool isFormulaInList = true;
        int customIndex = startingIndex;
        for (int i = 0; i < m_PointOrder.Count && isFormulaInList; i++)
        {
            isFormulaInList = _PointOrder[customIndex] == m_PointOrder[i];
            customIndex--;
            if (customIndex < 0)
            {
                customIndex = _PointOrder.Count - 2;
            }
        }
        return isFormulaInList;
    }

    public abstract void Cast(UnitPlayer _Player);
}
