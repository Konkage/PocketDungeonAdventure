using System.Collections.Generic;
using UnityEngine;

public interface I_Tile
{
    void SetNeighbour(E_Direction _Direction, I_Tile _Tile);

    I_Tile GetNeighbour(E_Direction _Direction);

    bool IsWalkable();

    void SetWalkable(bool _IsWalkable);

    Vector3 GetPosition();

    I_Unit GetUnit();

    void SetUnit(I_Unit _Unit);

    void AddTileEvent(TileEvent _TileEvent);

    void OnUnitEnter(I_Unit _Unit);

    Room GetRoom();

    void SetRoom(Room _Room);

    void SetTileLinks();

    List<MeshFilter> GetWallMesh();

    List<KeyValuePair<List<E_Direction>, Vertex>> GetCorners();

    string GetPrefabName();

    void SetPrefabName(string _PrefabName);

    void SetMap(I_Map _Map);

    I_Map GetMap();
}
