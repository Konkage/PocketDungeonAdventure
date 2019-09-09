using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private Map m_Map;

    [SerializeField]
    private UnitPlayer m_Player;

    [SerializeField]
    private int m_EnemyCount;

    [SerializeField]
    private Vector2Int m_MapSize;

    private void Start()
    {
        if (LevelLoader.IsLevelMustBeLoadedFromSave())
        {
            LoadLevel();
            LoadPlayerData();
            LoadEnemyData();
        }
        else
        {
            GenerateLevel();
            PlacePlayerRandomly();
            PlaceEnemiesRandomly(m_EnemyCount);
        }
    }

    private void GenerateLevel()
    {
        m_Map.GenerateRandomMap(m_MapSize);
    }

    private void LoadLevel()
    {
        m_Map.LoadMap(SaveManager.LoadMap());
    }

    private void PlacePlayerRandomly()
    {
        m_Player.StartRandomly(m_Map);
    }

    private void LoadPlayerData()
    {
        m_Player.LoadLastPosition(m_Map, SaveManager.LoadPlayer());
    }

    private void PlaceEnemiesRandomly(int _Count)
    {
        EnemyManager.SpawnEnemyRandomly(m_Map, _Count);
    }

    private void LoadEnemyData()
    {
        EnemyManager.LoadEnemies(m_Map, SaveManager.LoadEnemies());
    }

    private void OnApplicationQuit()
    {
        m_Map.SaveMap();
        m_Player.SavePlayer();
        EnemyManager.SaveEnemies();
    }
}