using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager
{
    public static void SavePlayer(PlayerData _PlayerData)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/PlayerSave.dat");
        binaryFormatter.Serialize(saveFile, _PlayerData);
        saveFile.Close();
    }

    public static void SaveEnemies(EnemiesData _EnemiesData)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/EnemySave.dat");
        binaryFormatter.Serialize(saveFile, _EnemiesData);
        saveFile.Close();
    }

    public static void SaveMap(MapData _MapData)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/MapSave.dat");
        binaryFormatter.Serialize(saveFile, _MapData);
        saveFile.Close();
    }

    public static PlayerData LoadPlayer()
    {
        PlayerData playerData = null;
        if (File.Exists(Application.persistentDataPath + "/PlayerSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerSave.dat", FileMode.Open);
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
        return playerData;
    }

    public static MapData LoadMap()
    {
        MapData mapData = null;
        if (File.Exists(Application.persistentDataPath + "/MapSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MapSave.dat", FileMode.Open);
            mapData = (MapData)bf.Deserialize(file);
            file.Close();
        }
        return mapData;
    }

    public static List<EnemyData> LoadEnemies()
    {
        List<EnemyData> enemiesData = null;
        if (File.Exists(Application.persistentDataPath + "/EnemySave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/EnemySave.dat", FileMode.Open);
            EnemiesData data = (EnemiesData)bf.Deserialize(file);
            enemiesData = data.Enemies;
            file.Close();
        }
        return enemiesData;
    }
}
