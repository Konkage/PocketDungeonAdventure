using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder
{
    private static TurnOrder m_Instance;
    private List<I_Controller> m_AllUnits;

    private TurnOrder()
    {
        m_AllUnits = new List<I_Controller>();
    }

    private static TurnOrder GetInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = new TurnOrder();
        }
        return m_Instance;
    }

    public static void RegisterUnit(I_Controller _Unit)
    {
        GetInstance().InternalRegisterUnit(_Unit);
    }

    private void InternalRegisterUnit(I_Controller _Unit)
    {
        m_AllUnits.Add(_Unit);
    }

    public static void RemoveUnit(I_Controller _Unit)
    {
        GetInstance().InternalRemoveUnit(_Unit);
    }

    private void InternalRemoveUnit(I_Controller _Unit)
    {
        m_AllUnits.Remove(_Unit);
    }

    public static void StartAllUnitsTurn()
    {
        GetInstance().InternalStartAllUnitsTurn();
    }

    private void InternalStartAllUnitsTurn()
    {
        for (int i = m_AllUnits.Count - 1; i >= 0; i--)
        {
            if (m_AllUnits[i] != null)
            {
                m_AllUnits[i].StartTurn();
            }
            else
            {
                m_AllUnits.RemoveAt(i);
            }
        }
    }
}
