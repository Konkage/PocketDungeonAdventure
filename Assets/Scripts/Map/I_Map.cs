using UnityEngine;

public interface I_Map
{
    I_Tile GetRandomWalkableTile();

    void GenerateRandomMap(Vector2Int _MapSize);
    void LoadMap(MapData _MapData);
    
    I_Tile GetTileAt(int _X, int _Z);
    I_Tile GetTileAt(Vector2Int _Position);

    void SaveMap();
}
