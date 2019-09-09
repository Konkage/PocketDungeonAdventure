using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuElement : MonoBehaviour
{
    [SerializeField]
    private Text m_MenuElementText;

    private OnUserInterfaceButtonPressed m_Event;

    public void SetEvent(KeyValuePair<string, OnUserInterfaceButtonPressed> _Event)
    {
        m_MenuElementText.text = _Event.Key;
        m_Event = _Event.Value;
    }

    public void OnMenuElementClick()
    {
        m_Event();
    }
}
