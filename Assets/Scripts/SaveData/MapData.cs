using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapData
{
    public List<TileData> Tiles;
    public List<EnemyData> Enemies;
    public List<RoomData> Rooms;

    public int MapSizeX;
    public int MapSizeZ;
    public int StairsPositionX;
    public int StairsPositionZ;
    public int StairsTriggerPositionX;
    public int StairsTriggerPositionZ;
}
