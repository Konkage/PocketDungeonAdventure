using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private static EnemyManager m_Instance;

    private List<UnitEnemy> m_Enemies;

    private EnemyManager()
    {
        m_Enemies = new List<UnitEnemy>();
    }

    private static EnemyManager GetInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = new EnemyManager();
        }
        return m_Instance;
    }

    public static void RemoveEnemy(UnitEnemy _Enemy)
    {
        GetInstance().InternalRemoveEnemy(_Enemy);
    }

    private void InternalRemoveEnemy(UnitEnemy _Enemy)
    {
        m_Enemies.Remove(_Enemy);
    }

    public static void SpawnEnemyRandomly(I_Map _Map, int _Count)
    {
        GetInstance().InternalSpawnEnemyRandomly(_Map, _Count);
    }

    private void InternalSpawnEnemyRandomly(I_Map _Map, int _Count)
    {
        for (int i = 0; i < _Count; i++)
        {
            UnitEnemy enemy = GameObject.Instantiate(Resources.Load<GameObject>("Entities/Enemy"), new Vector3(0, 0, 0), Quaternion.identity).GetComponent<UnitEnemy>();
            enemy.TeleportTo(_Map.GetRandomWalkableTile());
            m_Enemies.Add(enemy);
        }
    }

    public static void LoadEnemies(I_Map _Map, List<EnemyData> _EnemyData)
    {
        GetInstance().InternalLoadEnemies(_Map, _EnemyData);
    }

    private void InternalLoadEnemies(I_Map _Map, List<EnemyData> _EnemyData)
    {
        for (int i = 0; i < _EnemyData.Count; i++)
        {
            UnitEnemy enemy = GameObject.Instantiate(Resources.Load<GameObject>("Entities/Enemy"), new Vector3(0, 0, 0), Quaternion.identity).GetComponent<UnitEnemy>();
            enemy.LoadFromData(_Map, _EnemyData[i]);
            m_Enemies.Add(enemy);
        }
    }

    public static void SaveEnemies()
    {
        GetInstance().InternalSaveEnemies();
    }

    private void InternalSaveEnemies()
    {
        List<EnemyData> enemyData = new List<EnemyData>();
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            enemyData.Add(m_Enemies[i].GetEnemySave());
        }
        EnemiesData enemiesData = new EnemiesData
        {
            Enemies = enemyData
        };
        SaveManager.SaveEnemies(enemiesData);
    }
}
