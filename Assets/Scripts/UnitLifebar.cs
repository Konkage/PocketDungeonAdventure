using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitLifebar : MonoBehaviour, I_UnitHealthObserver
{
    [SerializeField]
    private Slider m_OpaqueLifebar;
    [SerializeField]
    private Slider m_TransparentLifebar;

    [SerializeField]
    private Text m_LifeText;

    [SerializeField]
    private float m_LifebarSpeed;

    private float m_CurrentLifeDisplayed;
    private int m_LastUnitLife;
    private int m_TargetUnitLife;

    private I_Unit m_Unit;

    public void DisplayUnitLife(I_Unit _Unit)
    {
        if (m_Unit != null)
        {
            m_Unit.UnregisterHealthObserver(this);
        }
        m_Unit = _Unit;
        m_Unit.RegisterHealthObserver(this);

        m_OpaqueLifebar.maxValue = m_Unit.MaxHealth;
        m_TransparentLifebar.maxValue = m_Unit.MaxHealth;

        m_CurrentLifeDisplayed = m_Unit.Health;
        m_LastUnitLife = m_Unit.Health;
        m_TargetUnitLife = m_Unit.Health;
        m_LifeText.text = _Unit.Health + " / " + _Unit.MaxHealth;
        UpdateLifeBar();
    }

    public void OnUnitHealthChange(I_Unit _Unit)
    {
        m_OpaqueLifebar.maxValue = _Unit.MaxHealth;
        m_TransparentLifebar.maxValue = _Unit.MaxHealth;
        m_TargetUnitLife = _Unit.Health;
        m_LifeText.text = _Unit.Health + " / " + _Unit.MaxHealth;
    }

    private void Update()
    {
        if (m_Unit != null)
        {
            if (m_TargetUnitLife != m_CurrentLifeDisplayed)
            {
                if (m_TargetUnitLife > m_CurrentLifeDisplayed)
                {
                    m_CurrentLifeDisplayed += m_LifebarSpeed * Time.deltaTime;
                    if (m_TargetUnitLife <= m_CurrentLifeDisplayed)
                    {
                        m_LastUnitLife = m_TargetUnitLife;
                        m_CurrentLifeDisplayed = m_TargetUnitLife;
                    }
                }
                else if (m_TargetUnitLife < m_CurrentLifeDisplayed)
                {
                    m_CurrentLifeDisplayed -= m_LifebarSpeed * Time.deltaTime;
                    if (m_TargetUnitLife >= m_CurrentLifeDisplayed)
                    {
                        m_LastUnitLife = m_TargetUnitLife;
                        m_CurrentLifeDisplayed = m_TargetUnitLife;
                    }
                }
                else
                {
                    m_LastUnitLife = m_TargetUnitLife;
                    m_CurrentLifeDisplayed = m_TargetUnitLife;
                }
            }
            else
            {
                m_CurrentLifeDisplayed = m_TargetUnitLife;
            }
            UpdateLifeBar();
        }
    }

    private void UpdateLifeBar()
    {
        if (m_TargetUnitLife == m_CurrentLifeDisplayed)
        {
            m_OpaqueLifebar.value = m_TargetUnitLife;
            m_TransparentLifebar.value = m_TargetUnitLife;
        }
        else if (m_TargetUnitLife > m_CurrentLifeDisplayed)
        {
            m_OpaqueLifebar.value = m_CurrentLifeDisplayed;
            m_TransparentLifebar.value = m_TargetUnitLife;
        }
        else
        {
            m_OpaqueLifebar.value = m_TargetUnitLife;
            m_TransparentLifebar.value = m_CurrentLifeDisplayed;
        }
    }
}
