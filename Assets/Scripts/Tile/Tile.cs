using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, I_Tile
{
    [SerializeField]
    private bool m_IsWalkable;

    [SerializeField]
    private bool m_IsActivable;

    [SerializeField]
    private GameObject m_UpPart;

    private List<TileEvent> m_Events = new List<TileEvent>();
    
    [Space(10)]
    [SerializeField]
    private GameObject m_NorthIfIsWalkable;
    [SerializeField]
    private GameObject m_SouthIfIsWalkable;
    [SerializeField]
    private GameObject m_EastIfIsWalkable;
    [SerializeField]
    private GameObject m_WestIfIsWalkable;

    [Space(10)]
    [SerializeField]
    private GameObject m_NorthEastIfIsWalkable;
    [SerializeField]
    private GameObject m_NorthWestIfIsWalkable;
    [SerializeField]
    private GameObject m_SouthEastIfIsWalkable;
    [SerializeField]
    private GameObject m_SouthWestIfIsWalkable;

    [Space(10)]
    [SerializeField]
    private GameObject m_NorthIsNotWalkableButEastAndNorthEastAre;
    [SerializeField]
    private GameObject m_NorthIsNotWalkableButWestAndNorthWestAre;
    [SerializeField]
    private GameObject m_SouthIsNotWalkableButEastAndSouthEastAre;
    [SerializeField]
    private GameObject m_SouthIsNotWalkableButWestAndSouthWestAre;
    [SerializeField]
    private GameObject m_EastIsNotWalkableButNorthAndNorthEastAre;
    [SerializeField]
    private GameObject m_EastIsNotWalkableButSouthAndSouthEastAre;
    [SerializeField]
    private GameObject m_WestIsNotWalkableButNorthAndNorthWestAre;
    [SerializeField]
    private GameObject m_WestIsNotWalkableButSouthAndSouthWestAre;

    [Space(10)]
    [SerializeField]
    private GameObject m_NorthEastIsNotWalkableButNorthIs;
    [SerializeField]
    private GameObject m_NorthEastIsNotWalkableButEastIs;
    [SerializeField]
    private GameObject m_SouthEastIsNotWalkableButSouthIs;
    [SerializeField]
    private GameObject m_SouthEastIsNotWalkableButEastIs;
    [SerializeField]
    private GameObject m_NorthWestIsNotWalkableButNorthIs;
    [SerializeField]
    private GameObject m_NorthWestIsNotWalkableButWestIs;
    [SerializeField]
    private GameObject m_SouthWestIsNotWalkableButSouthIs;
    [SerializeField]
    private GameObject m_SouthWestIsNotWalkableButWestIs;

    private I_Map m_Map;
    private I_Unit m_Unit;
    private Room m_Room;
    private string m_PrefabName;

    private Dictionary<E_Direction, I_Tile> m_Neighbours = new Dictionary<E_Direction, I_Tile>();

    public I_Tile GetNeighbour(E_Direction _Direction)
    {
        I_Tile tile;
        m_Neighbours.TryGetValue(_Direction, out tile);
        return tile;
    }

    public void SetNeighbour(E_Direction _Direction, I_Tile _Tile)
    {
        m_Neighbours[_Direction] = _Tile;
    }

    public bool IsWalkable()
    {
        return m_IsWalkable;
    }

    public void SetWalkable(bool _IsWalkable)
    {
        m_IsWalkable = _IsWalkable;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public I_Unit GetUnit()
    {
        return m_Unit;
    }

    public void SetUnit(I_Unit _Unit)
    {
        m_Unit = _Unit;
    }

    public void OnUnitEnter(I_Unit _Unit)
    {
        for (int i = 0; i< m_Events.Count; i++)
        {
            m_Events[i].ActivateEvent(_Unit);
        }
    }

    public Room GetRoom()
    {
        return m_Room;
    }

    public void SetRoom(Room _Room)
    {
        m_Room = _Room;
    }

    public void SetTileLinks()
    {
        if (!m_IsWalkable && !m_IsActivable)
        {
            I_Tile northTile = GetNeighbour(E_Direction.North);
            I_Tile southTile = GetNeighbour(E_Direction.South);
            I_Tile eastTile = GetNeighbour(E_Direction.East);
            I_Tile westTile = GetNeighbour(E_Direction.West);
            I_Tile northEastTile = GetNeighbour(E_Direction.NorthEast);
            I_Tile northWestTile = GetNeighbour(E_Direction.NorthWest);
            I_Tile southEastTile = GetNeighbour(E_Direction.SouthEast);
            I_Tile southWestTile = GetNeighbour(E_Direction.SouthWest);

            bool isNorthWalkable = northTile != null && northTile.IsWalkable();
            bool isSouthWalkable = southTile != null && southTile.IsWalkable();
            bool isEastWalkable = eastTile != null && eastTile.IsWalkable();
            bool isWestWalkable = westTile != null && westTile.IsWalkable();
            bool isNorthEastWalkable = northEastTile != null && northEastTile.IsWalkable();
            bool isNorthWestWalkable = northWestTile != null && northWestTile.IsWalkable();
            bool isSouthEastWalkable = southEastTile != null && southEastTile.IsWalkable();
            bool isSouthWestWalkable = southWestTile != null && southWestTile.IsWalkable();

            m_NorthIfIsWalkable.SetActive(isNorthWalkable);
            m_SouthIfIsWalkable.SetActive(isSouthWalkable);
            m_EastIfIsWalkable.SetActive(isEastWalkable);
            m_WestIfIsWalkable.SetActive(isWestWalkable);

            m_NorthEastIfIsWalkable.SetActive(isNorthEastWalkable && isNorthWalkable && isEastWalkable);
            m_NorthWestIfIsWalkable.SetActive(isNorthWestWalkable && isNorthWalkable && isWestWalkable);
            m_SouthEastIfIsWalkable.SetActive(isSouthEastWalkable && isSouthWalkable && isEastWalkable);
            m_SouthWestIfIsWalkable.SetActive(isSouthWestWalkable && isSouthWalkable && isWestWalkable);

            m_NorthIsNotWalkableButEastAndNorthEastAre.SetActive(!isNorthWalkable && isEastWalkable && isNorthEastWalkable);
            m_NorthIsNotWalkableButWestAndNorthWestAre.SetActive(!isNorthWalkable && isWestWalkable && isNorthWestWalkable);
            m_SouthIsNotWalkableButEastAndSouthEastAre.SetActive(!isSouthWalkable && isEastWalkable && isSouthEastWalkable);
            m_SouthIsNotWalkableButWestAndSouthWestAre.SetActive(!isSouthWalkable && isWestWalkable && isSouthWestWalkable);
            m_EastIsNotWalkableButNorthAndNorthEastAre.SetActive(!isEastWalkable && isNorthWalkable && isNorthEastWalkable);
            m_EastIsNotWalkableButSouthAndSouthEastAre.SetActive(!isEastWalkable && isSouthWalkable && isSouthEastWalkable);
            m_WestIsNotWalkableButNorthAndNorthWestAre.SetActive(!isWestWalkable && isNorthWalkable && isNorthWestWalkable);
            m_WestIsNotWalkableButSouthAndSouthWestAre.SetActive(!isWestWalkable && isSouthWalkable && isSouthWestWalkable);

            m_NorthEastIsNotWalkableButNorthIs.SetActive(!isNorthEastWalkable && isNorthWalkable);
            m_NorthEastIsNotWalkableButEastIs.SetActive(!isNorthEastWalkable && isEastWalkable);
            m_NorthWestIsNotWalkableButNorthIs.SetActive(!isNorthWestWalkable && isNorthWalkable);
            m_NorthWestIsNotWalkableButWestIs.SetActive(!isNorthWestWalkable && isWestWalkable);
            m_SouthEastIsNotWalkableButSouthIs.SetActive(!isSouthEastWalkable && isSouthWalkable);
            m_SouthEastIsNotWalkableButEastIs.SetActive(!isSouthEastWalkable && isEastWalkable);
            m_SouthWestIsNotWalkableButSouthIs.SetActive(!isSouthWestWalkable && isSouthWalkable);
            m_SouthWestIsNotWalkableButWestIs.SetActive(!isSouthWestWalkable && isWestWalkable);
        }
    }

    public List<KeyValuePair<List<E_Direction>, Vertex>> GetCorners()
    {
        List<KeyValuePair<List<E_Direction>, Vertex>> corners = new List<KeyValuePair<List<E_Direction>, Vertex>>();

        if (!m_IsWalkable)
        {
            if (m_NorthEastIfIsWalkable.activeInHierarchy)
            {
                corners.Add(new KeyValuePair<List<E_Direction>, Vertex>(new List<E_Direction> { E_Direction.South, E_Direction.West , E_Direction.South, E_Direction.West }, new Vertex(transform.position + new Vector3(0.4f, 1, 0.4f))));
            }
            if (m_NorthWestIfIsWalkable.activeInHierarchy)
            {
                corners.Add(new KeyValuePair<List<E_Direction>, Vertex>(new List<E_Direction> { E_Direction.South, E_Direction.East, E_Direction.South, E_Direction.East }, new Vertex(transform.position + new Vector3(-0.4f, 1, 0.4f))));
            }
            if (m_SouthEastIfIsWalkable.activeInHierarchy)
            {
                corners.Add(new KeyValuePair<List<E_Direction>, Vertex>(new List<E_Direction> { E_Direction.North, E_Direction.West, E_Direction.North, E_Direction.West }, new Vertex(transform.position + new Vector3(0.4f, 1, -0.4f))));
            }
            if (m_SouthWestIfIsWalkable.activeInHierarchy)
            {
                corners.Add(new KeyValuePair<List<E_Direction>, Vertex>(new List<E_Direction> { E_Direction.North, E_Direction.East, E_Direction.North, E_Direction.East }, new Vertex(transform.position + new Vector3(-0.4f, 1, -0.4f))));
            }

            if (m_NorthEastIsNotWalkableButNorthIs.activeInHierarchy)
            {
                corners.Add(new KeyValuePair<List<E_Direction>, Vertex>(new List<E_Direction> { E_Direction.North, E_Direction.West, E_Direction.South, E_Direction.East }, new Vertex(transform.position + new Vector3(0.6f, 1, 0.4f))));
            }
            if (m_NorthEastIsNotWalkableButEastIs.activeInHierarchy)
            {
                corners.Add(new KeyValuePair<List<E_Direction>, Vertex>(new List<E_Direction> { E_Direction.South, E_Direction.East, E_Direction.North, E_Direction.West }, new Vertex(transform.position + new Vector3(0.4f, 1, 0.6f))));
            }
            if (m_NorthWestIsNotWalkableButNorthIs.activeInHierarchy)
            {
                corners.Add(new KeyValuePair<List<E_Direction>, Vertex>(new List<E_Direction> { E_Direction.North, E_Direction.East, E_Direction.South, E_Direction.West }, new Vertex(transform.position + new Vector3(-0.6f, 1, 0.4f))));
            }
            if (m_NorthWestIsNotWalkableButWestIs.activeInHierarchy)
            {
                corners.Add(new KeyValuePair<List<E_Direction>, Vertex>(new List<E_Direction> { E_Direction.South, E_Direction.West, E_Direction.North, E_Direction.East }, new Vertex(transform.position + new Vector3(-0.4f, 1, 0.6f))));
            }
        }

        return corners;
    }

    public List<MeshFilter> GetWallMesh()
    {
        List<MeshFilter> meshes = new List<MeshFilter>();
        if (m_UpPart != null)
        {
            meshes.Add(m_UpPart.GetComponent<MeshFilter>());
        }
        if (!m_IsWalkable)
        {
            if (m_NorthIfIsWalkable.activeInHierarchy)
            {
                meshes.Add(m_NorthIfIsWalkable.GetComponent<MeshFilter>());
            }
            if (m_SouthIfIsWalkable.activeInHierarchy)
            {
                meshes.Add(m_SouthIfIsWalkable.GetComponent<MeshFilter>());
            }
            if (m_EastIfIsWalkable.activeInHierarchy)
            {
                meshes.Add(m_EastIfIsWalkable.GetComponent<MeshFilter>());
            }
            if (m_WestIfIsWalkable.activeInHierarchy)
            {
                meshes.Add(m_WestIfIsWalkable.GetComponent<MeshFilter>());
            }
            
            if (m_NorthEastIfIsWalkable.activeInHierarchy)
            {
                meshes.Add(m_NorthEastIfIsWalkable.GetComponent<MeshFilter>());
            }
            if (m_NorthWestIfIsWalkable.activeInHierarchy)
            {
                meshes.Add(m_NorthWestIfIsWalkable.GetComponent<MeshFilter>());
            }
            if (m_SouthEastIfIsWalkable.activeInHierarchy)
            {
                meshes.Add(m_SouthEastIfIsWalkable.GetComponent<MeshFilter>());
            }
            if (m_SouthWestIfIsWalkable.activeInHierarchy)
            {
                meshes.Add(m_SouthWestIfIsWalkable.GetComponent<MeshFilter>());
            }
            if (m_NorthIsNotWalkableButEastAndNorthEastAre.activeInHierarchy)
            {
                meshes.Add(m_NorthIsNotWalkableButEastAndNorthEastAre.GetComponent<MeshFilter>());
            }
            if (m_NorthIsNotWalkableButWestAndNorthWestAre.activeInHierarchy)
            {
                meshes.Add(m_NorthIsNotWalkableButWestAndNorthWestAre.GetComponent<MeshFilter>());
            }
            if (m_SouthIsNotWalkableButEastAndSouthEastAre.activeInHierarchy)
            {
                meshes.Add(m_SouthIsNotWalkableButEastAndSouthEastAre.GetComponent<MeshFilter>());
            }
            if (m_SouthIsNotWalkableButWestAndSouthWestAre.activeInHierarchy)
            {
                meshes.Add(m_SouthIsNotWalkableButWestAndSouthWestAre.GetComponent<MeshFilter>());
            }
            if (m_EastIsNotWalkableButNorthAndNorthEastAre.activeInHierarchy)
            {
                meshes.Add(m_EastIsNotWalkableButNorthAndNorthEastAre.GetComponent<MeshFilter>());
            }
            if (m_EastIsNotWalkableButSouthAndSouthEastAre.activeInHierarchy)
            {
                meshes.Add(m_EastIsNotWalkableButSouthAndSouthEastAre.GetComponent<MeshFilter>());
            }
            if (m_WestIsNotWalkableButNorthAndNorthWestAre.activeInHierarchy)
            {
                meshes.Add(m_WestIsNotWalkableButNorthAndNorthWestAre.GetComponent<MeshFilter>());
            }
            if (m_WestIsNotWalkableButNorthAndNorthWestAre.activeInHierarchy)
            {
                meshes.Add(m_WestIsNotWalkableButNorthAndNorthWestAre.GetComponent<MeshFilter>());
            }
            if (m_WestIsNotWalkableButSouthAndSouthWestAre.activeInHierarchy)
            {
                meshes.Add(m_WestIsNotWalkableButSouthAndSouthWestAre.GetComponent<MeshFilter>());
            }
            if (m_NorthEastIsNotWalkableButNorthIs.activeInHierarchy)
            {
                meshes.Add(m_NorthEastIsNotWalkableButNorthIs.GetComponent<MeshFilter>());
            }
            if (m_NorthEastIsNotWalkableButEastIs.activeInHierarchy)
            {
                meshes.Add(m_NorthEastIsNotWalkableButEastIs.GetComponent<MeshFilter>());
            }
            if (m_NorthWestIsNotWalkableButNorthIs.activeInHierarchy)
            {
                meshes.Add(m_NorthWestIsNotWalkableButNorthIs.GetComponent<MeshFilter>());
            }
            if (m_NorthWestIsNotWalkableButWestIs.activeInHierarchy)
            {
                meshes.Add(m_NorthWestIsNotWalkableButWestIs.GetComponent<MeshFilter>());
            }
            if (m_SouthEastIsNotWalkableButSouthIs.activeInHierarchy)
            {
                meshes.Add(m_SouthEastIsNotWalkableButSouthIs.GetComponent<MeshFilter>());
            }
            if (m_SouthEastIsNotWalkableButEastIs.activeInHierarchy)
            {
                meshes.Add(m_SouthEastIsNotWalkableButEastIs.GetComponent<MeshFilter>());
            }
            if (m_SouthWestIsNotWalkableButSouthIs.activeInHierarchy)
            {
                meshes.Add(m_SouthWestIsNotWalkableButSouthIs.GetComponent<MeshFilter>());
            }
            if (m_SouthWestIsNotWalkableButWestIs.activeInHierarchy)
            {
                meshes.Add(m_SouthWestIsNotWalkableButWestIs.GetComponent<MeshFilter>());
            }
        }
        return meshes;
    }

    public void AddTileEvent(TileEvent _TileEvent)
    {
        m_Events.Add(_TileEvent);
    }

    public string GetPrefabName()
    {
        return m_PrefabName;
    }

    public void SetPrefabName(string _PrefabName)
    {
        m_PrefabName = _PrefabName;
    }

    public void SetMap(I_Map _Map)
    {
        m_Map = _Map;
    }

    public I_Map GetMap()
    {
        return m_Map;
    }
}
