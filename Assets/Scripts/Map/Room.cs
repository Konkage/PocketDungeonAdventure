using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private Vector2Int m_BottomLeftCorner;
    private Vector2Int m_Size;

    private List<Room> m_LinkedRooms;
    private List<I_Tile> m_TilesInRoom;
    private List<I_Unit> m_UnitsInRoom;

    public Room(Vector2Int _BottomLeftCorner, Vector2Int _Size)
    {
        m_BottomLeftCorner = _BottomLeftCorner;
        m_Size = _Size;
        m_LinkedRooms = new List<Room>();
        m_TilesInRoom = new List<I_Tile>();
        m_UnitsInRoom = new List<I_Unit>();
    }

    public int GetLeft()
    {
        return m_BottomLeftCorner.x;
    }

    public int GetRight()
    {
        return m_BottomLeftCorner.x + m_Size.x;
    }

    public int GetUp()
    {
        return m_BottomLeftCorner.y + m_Size.y;
    }

    public int GetDown()
    {
        return m_BottomLeftCorner.y;
    }

    public Vector2Int GetBottomLeftCorner()
    {
        return m_BottomLeftCorner;
    }

    public Vector2Int GetSize()
    {
        return m_Size;
    }

    public void AddLinkedRoom(Room _Room)
    {
        m_LinkedRooms.Add(_Room);
    }

    public bool IsLinkedWith(Room _Room)
    {
        return m_LinkedRooms.Contains(_Room);
    }

    public List<Room> GetLinkedRooms()
    {
        return m_LinkedRooms;
    }

    public List<I_Tile> GetAllTiles()
    {
        return m_TilesInRoom;
    }

    public void AddTile(I_Tile _Tile)
    {
        m_TilesInRoom.Add(_Tile);
    }

    public void AddUnit(I_Unit _Unit)
    {
        if (!m_UnitsInRoom.Contains(_Unit))
        {
            m_UnitsInRoom.Add(_Unit);
        }
    }

    public void RemoveUnit(I_Unit _Unit)
    {
        m_UnitsInRoom.Remove(_Unit);
    }
}
