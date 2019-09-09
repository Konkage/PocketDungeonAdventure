using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader
{
    private static LevelLoader m_Instance;

    private bool m_MustBeLoadedFromSave;

    private LevelLoader()
    {
        m_MustBeLoadedFromSave = true;
    }

    private static LevelLoader GetInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = new LevelLoader();
        }
        return m_Instance;
    }

    public static void SetLevelMustBeLoadedFromSave(bool _MustBeLoadedFromSave)
    {
        GetInstance().InternalSetLevelMustBeLoadedFromSave(_MustBeLoadedFromSave);
    }

    private void InternalSetLevelMustBeLoadedFromSave(bool _MustBeLoadedFromSave)
    {
        m_MustBeLoadedFromSave = _MustBeLoadedFromSave;
    }

    public static bool IsLevelMustBeLoadedFromSave()
    {
        return GetInstance().InternalIsLevelMustBeLoadedFromSave();
    }

    private bool InternalIsLevelMustBeLoadedFromSave()
    {
        return m_MustBeLoadedFromSave;
    }
}
