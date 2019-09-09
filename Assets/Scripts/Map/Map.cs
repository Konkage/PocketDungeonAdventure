using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map : MonoBehaviour, I_Map
{
    private Dictionary<Vector2Int, I_Tile> m_Tiles;

    private List<Room> m_Rooms;

    [SerializeField]
    private MeshFilter m_WallsMeshFilter;
    [SerializeField]
    private MeshFilter m_FloorsMeshFilter;
    private List<I_Tile> m_Walls;
    private List<I_Tile> m_Floors;
    private Vector2Int m_MapSize;
    private Vector2Int m_StairsPosition;
    private Vector2Int m_StairsTriggerPosition;

    public I_Tile GetRandomWalkableTile()
    {
        List<I_Tile> tiles = m_Rooms[Random.Range(0, m_Rooms.Count)].GetAllTiles();
        return tiles[Random.Range(0, tiles.Count)];
    }

    public void LoadMap(MapData _MapData)
    {
        m_Walls = new List<I_Tile>();
        m_Floors = new List<I_Tile>();
        m_Tiles = new Dictionary<Vector2Int, I_Tile>();
        if (_MapData != null)
        {
            m_MapSize = new Vector2Int(_MapData.MapSizeX, _MapData.MapSizeZ);

            m_Rooms = new List<Room>();
            List<RoomData> roomsToLoad = _MapData.Rooms;
            for (int i = 0; i < roomsToLoad.Count; i++)
            {
                m_Rooms.Add(new Room(new Vector2Int(roomsToLoad[i].m_PosX, roomsToLoad[i].m_PosZ), new Vector2Int(roomsToLoad[i].m_SizeX, roomsToLoad[i].m_SizeZ)));
            }

            List<TileData> tilesToLoad = _MapData.Tiles;
            for (int i = 0; i < tilesToLoad.Count; i++)
            {
                Room room = null;
                if (tilesToLoad[i].m_RoomIndex >= 0)
                {
                    room = m_Rooms[tilesToLoad[i].m_RoomIndex];
                }

                GenerateTile("Tiles/TileEmpty", tilesToLoad[i].m_PosX, tilesToLoad[i].m_PosZ, room);
            }

            m_StairsPosition = new Vector2Int(_MapData.StairsPositionX, _MapData.StairsPositionZ);
            m_StairsTriggerPosition = new Vector2Int(_MapData.StairsTriggerPositionX, _MapData.StairsTriggerPositionZ);
            I_Tile stairs = GenerateTile("Tiles/TileStairs", _MapData.StairsPositionX, _MapData.StairsPositionZ, null);
            I_Tile stairsTrigger = GenerateTile("Tiles/TileShakeBell", _MapData.StairsTriggerPositionX, _MapData.StairsTriggerPositionZ, null);

            Tile trucMoche = stairsTrigger as Tile;
            TileEventShake trucEncorePlusMoche = trucMoche.GetComponent<TileEventShake>();
            Tile trucSuperMoche = stairs as Tile;
            I_Activable trucSuperPlusMoche = trucSuperMoche.GetComponent<I_Activable>();
            trucEncorePlusMoche.SetTileToActivate(trucSuperPlusMoche);


            for (int x = -1; x <= m_MapSize.x + 1; x++)
            {
                for (int z = -1; z <= m_MapSize.y + 1; z++)
                {
                    I_Tile tile;
                    if (!m_Tiles.TryGetValue(new Vector2Int(x, z), out tile))
                    {
                        tile = GenerateTile("Tiles/TileWall", x, z, null);
                    }
                    tile.SetMap(this);
                }
            }
            SmoothMap();
        }
    }

    public void GenerateRandomMap(Vector2Int _MapSize)
    {
        m_MapSize = _MapSize;
        m_Walls = new List<I_Tile>();
        m_Floors = new List<I_Tile>();
        m_Tiles = new Dictionary<Vector2Int, I_Tile>();
        
        GenerateRooms(_MapSize, 10, new Vector2Int(3, 3), new Vector2Int(10, 10));
        for (int x = -1; x <= _MapSize.x + 1; x++)
        {
            for (int z = -1; z <= _MapSize.y + 1; z++)
            {
                I_Tile tile;
                if (!m_Tiles.TryGetValue(new Vector2Int(x, z), out tile))
                {
                    tile = GenerateTile("Tiles/TileWall", x, z, null);
                }
                tile.SetMap(this);
            }
        }
        SmoothMap();
    }

    private void SmoothMap()
    {
        foreach (KeyValuePair<Vector2Int, I_Tile> tile in m_Tiles)
        {
            tile.Value.SetTileLinks();
        }

        List<KeyValuePair<List<E_Direction>, Vertex>> allCorners = new List<KeyValuePair<List<E_Direction>, Vertex>>();

        List<MeshFilter> allWallsMeshes = new List<MeshFilter>();
        for (int i = 0; i < m_Walls.Count; i++)
        {
            allWallsMeshes.AddRange(m_Walls[i].GetWallMesh());
            allCorners.AddRange(m_Walls[i].GetCorners());
        }

        List<Edge> edges = new List<Edge>();
        for (int i = 0; i < allCorners.Count; i++)
        {
            List<E_Direction> directions = allCorners[i].Key;
            Vertex position = allCorners[i].Value;

            Vertex closestEastWest = position;
            Vertex closestNorthSouth = position;
            float closestEastWestDistance = -1;
            float closestNorthSouthDistance = -1;

            for (int direction = 0; direction < 2; direction++)
            {
                if (directions[direction] == E_Direction.North)
                {
                    for (int j = 0; j < allCorners.Count; j++)
                    {
                        if (allCorners[j].Key.Contains(E_Direction.South))
                        {
                            if (allCorners[j].Value.Position.x == position.Position.x && allCorners[j].Value.Position.z > position.Position.z)
                            {
                                float currentDistance = Vector3.Distance(allCorners[j].Value.Position, position.Position);
                                if (closestNorthSouthDistance < 0 || closestNorthSouthDistance > currentDistance)
                                {
                                    closestNorthSouthDistance = currentDistance;
                                    closestNorthSouth = allCorners[j].Value;
                                }
                            }
                        }
                    }
                }
                else if (directions[direction] == E_Direction.South)
                {
                    for (int j = 0; j < allCorners.Count; j++)
                    {
                        if (allCorners[j].Key.Contains(E_Direction.North))
                        {
                            if (allCorners[j].Value.Position.x == position.Position.x && allCorners[j].Value.Position.z < position.Position.z)
                            {
                                float currentDistance = Vector3.Distance(allCorners[j].Value.Position, position.Position);
                                if (closestNorthSouthDistance < 0 || closestNorthSouthDistance > currentDistance)
                                {
                                    closestNorthSouthDistance = currentDistance;
                                    closestNorthSouth = allCorners[j].Value;
                                }
                            }
                        }
                    }
                }
                else if (directions[direction] == E_Direction.East)
                {
                    for (int j = 0; j < allCorners.Count; j++)
                    {
                        if (allCorners[j].Key.Contains(E_Direction.West))
                        {
                            if (allCorners[j].Value.Position.z == position.Position.z && allCorners[j].Value.Position.x > position.Position.x)
                            {
                                float currentDistance = Vector3.Distance(allCorners[j].Value.Position, position.Position);
                                if (closestEastWestDistance < 0 || closestEastWestDistance > currentDistance)
                                {
                                    closestEastWestDistance = currentDistance;
                                    closestEastWest = allCorners[j].Value;
                                }
                            }
                        }
                    }
                }
                else if (directions[direction] == E_Direction.West)
                {
                    for (int j = 0; j < allCorners.Count; j++)
                    {
                        if (allCorners[j].Key.Contains(E_Direction.East))
                        {
                            if (allCorners[j].Value.Position.z == position.Position.z && allCorners[j].Value.Position.x < position.Position.x)
                            {
                                float currentDistance = Vector3.Distance(allCorners[j].Value.Position, position.Position);
                                if (closestEastWestDistance < 0 || closestEastWestDistance > currentDistance)
                                {
                                    closestEastWestDistance = currentDistance;
                                    closestEastWest = allCorners[j].Value;
                                }
                            }
                        }
                    }
                }
            }

            if (closestNorthSouthDistance >= 0 && closestEastWestDistance >= 0)
            {
                Edge northSouthEdge = new Edge(position, closestNorthSouth);
                Edge eastWestEdge = new Edge(position, closestEastWest);
                if (directions[3] == E_Direction.East && position.Position.z < closestNorthSouth.Position.z || directions[3] == E_Direction.West && position.Position.z > closestNorthSouth.Position.z)
                {
                    northSouthEdge.FlipEdge();
                }
                if (directions[2] == E_Direction.North && position.Position.x > closestEastWest.Position.x || directions[2] == E_Direction.South && position.Position.x < closestEastWest.Position.x)
                {
                    eastWestEdge.FlipEdge();
                }
                northSouthEdge.FirstVertex.NextVertex = northSouthEdge.SecondVertex;
                northSouthEdge.SecondVertex.PreviousVertex = northSouthEdge.FirstVertex;
                edges.Add(northSouthEdge);
                eastWestEdge.FirstVertex.NextVertex = eastWestEdge.SecondVertex;
                eastWestEdge.SecondVertex.PreviousVertex = eastWestEdge.FirstVertex;
                edges.Add(eastWestEdge);
            }
        }

        List<Vertex> allVertices = new List<Vertex>();
        for (int i = 0; i < allCorners.Count; i++)
        {
            allVertices.Add(allCorners[i].Value);
        }

        List<Vertex> allVerticesToProcess = new List<Vertex>();
        allVerticesToProcess.AddRange(allVertices);

        List<List<Vertex>> allLoops = new List<List<Vertex>>();
        int security = 0;
        while (allVerticesToProcess.Count > 0 && security < 10000)
        {
            allLoops.Add(GetVertexLoop(allVerticesToProcess));
            security++;
        }

        List<Vertex> externalPoints = new List<Vertex> { new Vertex(new Vector3(-10, 1, -10)), new Vertex(new Vector3(10 + m_MapSize.x, 1, -10)), new Vertex(new Vector3(10 + m_MapSize.x, 1, 10 + m_MapSize.y)), new Vertex(new Vector3(-10, 1, 10 + m_MapSize.y)) };

        Mesh optimizedRoof = MeshOptimizer.GenerateOptimisedMesh(allVertices, externalPoints, allLoops);

        List<MeshFilter> allFloorsMeshes = new List<MeshFilter>();
        for (int i = 0; i < m_Floors.Count; i++)
        {
            allFloorsMeshes.AddRange(m_Floors[i].GetWallMesh());
        }


        CombineInstance[] combineWalls = new CombineInstance[allWallsMeshes.Count + 1];

        for (int i = 0; i < allWallsMeshes.Count; i++)
        {
            combineWalls[i].mesh = allWallsMeshes[i].sharedMesh;
            combineWalls[i].transform = allWallsMeshes[i].transform.localToWorldMatrix;
            allWallsMeshes[i].gameObject.SetActive(false);
        }
        combineWalls[allWallsMeshes.Count].mesh = optimizedRoof;
        combineWalls[allWallsMeshes.Count].transform = transform.localToWorldMatrix;

        Mesh optimisedMesh = new Mesh();
        optimisedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        optimisedMesh.CombineMeshes(combineWalls);
        m_WallsMeshFilter.mesh = optimisedMesh;



        CombineInstance[] combineFloors = new CombineInstance[allFloorsMeshes.Count];
        for (int i = 0; i < allFloorsMeshes.Count; i++)
        {
            combineFloors[i].mesh = allFloorsMeshes[i].sharedMesh;
            combineFloors[i].transform = allFloorsMeshes[i].transform.localToWorldMatrix;
            allFloorsMeshes[i].gameObject.SetActive(false);
        }
        m_FloorsMeshFilter.mesh = new Mesh();
        m_FloorsMeshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        m_FloorsMeshFilter.mesh.CombineMeshes(combineFloors);

        UserInterface.DisplayLoadingScreen(false);
    }

    private List<Vertex> GetVertexLoop(List<Vertex> _AllVertices)
    {
        List<Vertex> vertexLoop = new List<Vertex>();

        List<Vertex> points = new List<Vertex>();
        for (int i = 0; i < _AllVertices.Count; i++)
        {
            points.Add(_AllVertices[i]);
        }
        Vertex firstVertex = points[0];
        Vertex currentVertex = firstVertex;
        while (currentVertex != null && currentVertex.NextVertex != firstVertex)
        {
            vertexLoop.Add(currentVertex);
            currentVertex = currentVertex.NextVertex;
        }
        if (currentVertex != null)
        {
            vertexLoop.Add(currentVertex);
        }

        for (int i = _AllVertices.Count - 1; i >= 0; i--)
        {
            if (vertexLoop.Contains(_AllVertices[i]))
            {
                _AllVertices.RemoveAt(i);
            }
        }

        return vertexLoop;
    }

    private void GenerateRooms(Vector2Int _MapSize, int _RoomNumbers, Vector2Int _MinRoomSize, Vector2Int _MaxRoomSize)
    {
        m_Rooms = new List<Room>();
        m_Rooms.Add(GenerateSpecialRoom(_MapSize));
        while (m_Rooms.Count < _RoomNumbers)
        {
            Room currentRoom = GenerateRoom(_MapSize, _MinRoomSize, _MaxRoomSize);
            if (!HasCollisionsWithOtherRooms(currentRoom))
            {
                m_Rooms.Add(currentRoom);
            }
        }

        for (int i = 0; i < m_Rooms.Count; i++)
        {
            for (int x = m_Rooms[i].GetLeft(); x < m_Rooms[i].GetRight() + 1; x++)
            {
                for (int y = m_Rooms[i].GetDown(); y < m_Rooms[i].GetUp() + 1; y++)
                {
                    GenerateTile("Tiles/TileEmpty", x, y, m_Rooms[i]);
                }
            }
        }

        GeneratePathBetweenClosestRooms(_MinRoomSize.x + 1);

        List<Room> unreachableRooms = GetUnreachableRooms();
        while (unreachableRooms.Count > 0)
        {
            LinkToClosestRoom(unreachableRooms[0], unreachableRooms);
            unreachableRooms = GetUnreachableRooms();
        }
    }

    private void LinkToClosestRoom(Room _RoomToLink, List<Room> _RoomsToAvoid)
    {
        Room closestRoom = null;
        int closestDistance = -1;
        for (int i = 0; i < m_Rooms.Count; i++)
        {
            if (!_RoomsToAvoid.Contains(m_Rooms[i]))
            {
                int distanceToTile = GetDistanceBetweenRooms(_RoomToLink, m_Rooms[i]);
                if (closestDistance < 0 || closestDistance > distanceToTile)
                {
                    closestRoom = m_Rooms[i];
                    closestDistance = distanceToTile;
                }
            }
        }
        LinkComplexeRooms(_RoomToLink, closestRoom);
    }

    private void LinkComplexeRooms(Room _FirstRoom, Room _SecondRoom)
    {
        int horizontalDistance = 0;
        if (_FirstRoom.GetRight() >= _SecondRoom.GetLeft())
        {
            horizontalDistance += _FirstRoom.GetRight() - _SecondRoom.GetLeft();
        }
        else if (_FirstRoom.GetLeft() <= _SecondRoom.GetRight())
        {
            horizontalDistance += _SecondRoom.GetRight() - _FirstRoom.GetLeft();
        }
        int verticalDistance = 0;
        if (_FirstRoom.GetUp() >= _SecondRoom.GetDown())
        {
            verticalDistance += _FirstRoom.GetUp() - _SecondRoom.GetDown();
        }
        else if (_FirstRoom.GetDown() <= _SecondRoom.GetUp())
        {
            verticalDistance += _SecondRoom.GetUp() - _FirstRoom.GetDown();
        }
        if (verticalDistance > horizontalDistance)
        {
            int startX = 0;
            int endX = 0;
            int startY = 0;
            int endY = 0;

            if (_FirstRoom.GetUp() < _SecondRoom.GetDown())
            {
                startY = _FirstRoom.GetUp() + 1;
                endY = _SecondRoom.GetDown();
                startX = Random.Range(_FirstRoom.GetLeft(), _FirstRoom.GetRight() + 1);
                endX = Random.Range(_SecondRoom.GetLeft(), _SecondRoom.GetRight() + 1);
            }
            else
            {
                startY = _SecondRoom.GetUp() + 1;
                endY = _FirstRoom.GetDown();
                startX = Random.Range(_SecondRoom.GetLeft(), _SecondRoom.GetRight() + 1);
                endX = Random.Range(_FirstRoom.GetLeft(), _FirstRoom.GetRight() + 1);
            }

            int middleY = startY;
            if (startY + 1 < endY - 1)
            {
                middleY = Random.Range(startY + 1, endY - 1);
            }

            int x = startX;
            int y = startY;
            while (y < endY)
            {
                I_Tile tile;
                if (!m_Tiles.TryGetValue(new Vector2Int(x, y), out tile))
                {
                    GenerateTile("Tiles/TileEmpty", x, y, null);
                }
                if (middleY > 0 && y == middleY)
                {
                    if (x > endX)
                    {
                        x--;
                    }
                    else if (x < endX)
                    {
                        x++;
                    }
                    else
                    {
                        y++;
                    }
                }
                else
                {
                    y++;
                }
            }

            _FirstRoom.AddLinkedRoom(_SecondRoom);
            _SecondRoom.AddLinkedRoom(_FirstRoom);
        }
        else
        {
            int startX = 0;
            int endX = 0;
            int startY = 0;
            int endY = 0;

            if (_FirstRoom.GetRight() < _SecondRoom.GetLeft())
            {
                startX = _FirstRoom.GetRight() + 1;
                endX = _SecondRoom.GetLeft();
                startY = Random.Range(_FirstRoom.GetDown(), _FirstRoom.GetUp() + 1);
                endY = Random.Range(_SecondRoom.GetDown(), _SecondRoom.GetUp() + 1);
            }
            else
            {
                startX = _SecondRoom.GetRight() + 1;
                endX = _FirstRoom.GetLeft();
                startY = Random.Range(_SecondRoom.GetDown(), _SecondRoom.GetUp() + 1);
                endY = Random.Range(_FirstRoom.GetDown(), _FirstRoom.GetUp() + 1);
            }

            int middleX = startX;
            if (startX + 1 < endX - 1)
            {
                middleX = Random.Range(startX + 1, endX - 1);
            }

            int x = startX;
            int y = startY;
            while (x < endX)
            {
                I_Tile tile;
                if (!m_Tiles.TryGetValue(new Vector2Int(x, y), out tile))
                {
                    GenerateTile("Tiles/TileEmpty", x, y, null);
                }
                if (middleX > 0 && x == middleX)
                {
                    if (y > endY)
                    {
                        y--;
                    }
                    else if (y < endY)
                    {
                        y++;
                    }
                    else
                    {
                        x++;
                    }
                }
                else
                {
                    x++;
                }
            }

            _FirstRoom.AddLinkedRoom(_SecondRoom);
            _SecondRoom.AddLinkedRoom(_FirstRoom);
        }
    }

    private int GetDistanceBetweenRooms(Room _FirstRoom, Room _SecondRoom)
    {
        int distance = 0;
        if (_FirstRoom.GetRight() > _SecondRoom.GetLeft())
        {
            distance += _FirstRoom.GetRight() - _SecondRoom.GetLeft();
        }
        else if (_FirstRoom.GetLeft() < _SecondRoom.GetRight())
        {
            distance += _SecondRoom.GetRight() - _FirstRoom.GetLeft();
        }

        if (_FirstRoom.GetUp() > _SecondRoom.GetDown())
        {
            distance += _FirstRoom.GetUp() - _SecondRoom.GetDown();
        }
        else if (_FirstRoom.GetDown() < _SecondRoom.GetUp())
        {
            distance += _SecondRoom.GetUp() - _FirstRoom.GetDown();
        }
        return distance;
    }

    private List<Room> GetUnreachableRooms()
    {
        List<Room> reachedRooms = new List<Room>();
        List<Room> reachableRooms = new List<Room> { m_Rooms[0] };
        List<Room> unreachableRooms = new List<Room>();
        while (reachableRooms.Count > 0)
        {
            if (!reachedRooms.Contains(reachableRooms[0]))
            {
                reachedRooms.Add(reachableRooms[0]);
                List<Room> linkedRooms = reachableRooms[0].GetLinkedRooms();
                for (int j = 0; j < linkedRooms.Count; j++)
                {
                    reachableRooms.Add(linkedRooms[j]);
                }
            }
            reachableRooms.RemoveAt(0);
        }

        for (int i = 0; i < m_Rooms.Count; i++)
        {
            if (!reachedRooms.Contains(m_Rooms[i]))
            {
                unreachableRooms.Add(m_Rooms[i]);
            }
        }
        return unreachableRooms;
    }

    private void GeneratePathBetweenClosestRooms(int _MaxDistance)
    {
        for (int i = 0; i < m_Rooms.Count; i++)
        {
            for (int j = 0; j < m_Rooms.Count; j++)
            {
                if (!m_Rooms[i].IsLinkedWith(m_Rooms[j]))
                {
                    bool hasVerticalCollision = (m_Rooms[i].GetUp() >= m_Rooms[j].GetDown() && m_Rooms[i].GetUp() <= m_Rooms[j].GetUp()) || (m_Rooms[i].GetDown() >= m_Rooms[j].GetDown() && m_Rooms[i].GetDown() <= m_Rooms[j].GetUp());
                    bool hasHorizontalCollision = (m_Rooms[i].GetRight() >= m_Rooms[j].GetLeft() && m_Rooms[i].GetRight() <= m_Rooms[j].GetRight()) || (m_Rooms[i].GetLeft() >= m_Rooms[j].GetLeft() && m_Rooms[i].GetLeft() <= m_Rooms[j].GetRight());
                    bool hasVerticalInclusion = (m_Rooms[i].GetUp() >= m_Rooms[j].GetUp() && m_Rooms[i].GetDown() <= m_Rooms[j].GetDown()) || (m_Rooms[i].GetDown() >= m_Rooms[j].GetDown() && m_Rooms[i].GetUp() <= m_Rooms[j].GetUp());
                    bool hasHorizontalInclusion = (m_Rooms[i].GetRight() >= m_Rooms[j].GetRight() && m_Rooms[i].GetLeft() <= m_Rooms[j].GetLeft()) || (m_Rooms[i].GetLeft() >= m_Rooms[j].GetLeft() && m_Rooms[i].GetRight() <= m_Rooms[j].GetRight());
                    if (hasVerticalCollision || hasVerticalInclusion)
                    {
                        if ((m_Rooms[i].GetRight() < m_Rooms[j].GetLeft() && m_Rooms[i].GetRight() + _MaxDistance >= m_Rooms[j].GetLeft()) || (m_Rooms[j].GetRight() < m_Rooms[i].GetLeft() && m_Rooms[j].GetRight() + _MaxDistance >= m_Rooms[i].GetLeft()))
                        {
                            GeneratePathBetweenRooms(m_Rooms[i], m_Rooms[j]);
                        }
                    }
                    else if (hasHorizontalCollision || hasHorizontalInclusion)
                    {
                        if ((m_Rooms[i].GetUp() < m_Rooms[j].GetDown() && m_Rooms[i].GetUp() + _MaxDistance >= m_Rooms[j].GetDown()) || (m_Rooms[j].GetUp() < m_Rooms[i].GetDown() && m_Rooms[j].GetUp() + _MaxDistance >= m_Rooms[i].GetDown()))
                        {
                            GeneratePathBetweenRooms(m_Rooms[i], m_Rooms[j]);
                        }
                    }
                }
            }
        }
    }

    private void GeneratePathBetweenRooms(Room _FirstRoom, Room _SecondRoom)
    {
        bool hasVerticalCollision = (_FirstRoom.GetUp() >= _SecondRoom.GetDown() && _FirstRoom.GetUp() <= _SecondRoom.GetUp()) || (_FirstRoom.GetDown() >= _SecondRoom.GetDown() && _FirstRoom.GetDown() <= _SecondRoom.GetUp());
        bool hasHorizontalCollision = (_FirstRoom.GetRight() >= _SecondRoom.GetLeft() && _FirstRoom.GetRight() <= _SecondRoom.GetRight()) || (_FirstRoom.GetLeft() >= _SecondRoom.GetLeft() && _FirstRoom.GetLeft() <= _SecondRoom.GetRight());
        bool hasVerticalInclusion = (_FirstRoom.GetUp() >= _SecondRoom.GetUp() && _FirstRoom.GetDown() <= _SecondRoom.GetDown()) || (_FirstRoom.GetDown() >= _SecondRoom.GetDown() && _FirstRoom.GetUp() <= _SecondRoom.GetUp());
        bool hasHorizontalInclusion = (_FirstRoom.GetRight() >= _SecondRoom.GetRight() && _FirstRoom.GetLeft() <= _SecondRoom.GetLeft()) || (_FirstRoom.GetLeft() >= _SecondRoom.GetLeft() && _FirstRoom.GetRight() <= _SecondRoom.GetRight());
        if (hasHorizontalCollision || hasHorizontalInclusion)
        {
            int startX = 0;
            int endX = 0;
            int startY = 0;
            int endY = 0;

            if (_FirstRoom.GetUp() < _SecondRoom.GetDown())
            {
                startY = _FirstRoom.GetUp() + 1;
                endY = _SecondRoom.GetDown();
                startX = Random.Range(_FirstRoom.GetLeft(), _FirstRoom.GetRight() + 1);
                endX = Random.Range(_SecondRoom.GetLeft(), _SecondRoom.GetRight() + 1);
            }
            else
            {
                startY = _SecondRoom.GetUp() + 1;
                endY = _FirstRoom.GetDown();
                startX = Random.Range(_SecondRoom.GetLeft(), _SecondRoom.GetRight() + 1);
                endX = Random.Range(_FirstRoom.GetLeft(), _FirstRoom.GetRight() + 1);
            }

            int middleY = -1;
            if (startY + 1 < endY - 1)
            {
                middleY = Random.Range(startY + 1, endY - 1);
            }
            else
            {
                int minX = _FirstRoom.GetLeft();
                if (minX < _SecondRoom.GetLeft())
                {
                    minX = _SecondRoom.GetLeft();
                }
                int maxX = _FirstRoom.GetRight();
                if (maxX > _SecondRoom.GetRight())
                {
                    maxX = _SecondRoom.GetRight();
                }
                startX = Random.Range(minX, maxX + 1);
                endX = startX;
            }

            int x = startX;
            int y = startY;
            while (y < endY)
            {
                I_Tile tile;
                if (!m_Tiles.TryGetValue(new Vector2Int(x, y), out tile))
                {
                    GenerateTile("Tiles/TileEmpty", x, y, null);
                }
                if (middleY > 0 && y == middleY)
                {
                    if (x > endX)
                    {
                        x--;
                    }
                    else if (x < endX)
                    {
                        x++;
                    }
                    else
                    {
                        y++;
                    }
                }
                else
                {
                    y++;
                }
            }
            _FirstRoom.AddLinkedRoom(_SecondRoom);
            _SecondRoom.AddLinkedRoom(_FirstRoom);
        }
        else if (hasVerticalCollision || hasVerticalInclusion)
        {
            int startX = 0;
            int endX = 0;
            int startY = 0;
            int endY = 0;

            if (_FirstRoom.GetRight() < _SecondRoom.GetLeft())
            {
                startX = _FirstRoom.GetRight() + 1;
                endX = _SecondRoom.GetLeft();
                startY = Random.Range(_FirstRoom.GetDown(), _FirstRoom.GetUp() + 1);
                endY = Random.Range(_SecondRoom.GetDown(), _SecondRoom.GetUp() + 1);
            }
            else
            {
                startX = _SecondRoom.GetRight() + 1;
                endX = _FirstRoom.GetLeft();
                startY = Random.Range(_SecondRoom.GetDown(), _SecondRoom.GetUp() + 1);
                endY = Random.Range(_FirstRoom.GetDown(), _FirstRoom.GetUp() + 1);
            }

            int middleX = -1;
            if (startX + 1 < endX - 1)
            {
                middleX = Random.Range(startX + 1, endX - 1);
            }
            else
            {
                int minY = _FirstRoom.GetDown();
                if (minY < _SecondRoom.GetDown())
                {
                    minY = _SecondRoom.GetDown();
                }
                int maxY = _FirstRoom.GetUp();
                if (maxY > _SecondRoom.GetUp())
                {
                    maxY = _SecondRoom.GetUp();
                }
                startY = Random.Range(minY, maxY + 1);
                endY = startY;
            }

            int x = startX;
            int y = startY;
            while (x < endX)
            {
                I_Tile tile;
                if (!m_Tiles.TryGetValue(new Vector2Int(x, y), out tile))
                {
                    GenerateTile("Tiles/TileEmpty", x, y, null);
                }
                if (middleX > 0 && x == middleX)
                {
                    if (y > endY)
                    {
                        y--;
                    }
                    else if (y < endY)
                    {
                        y++;
                    }
                    else
                    {
                        x++;
                    }
                }
                else
                {
                    x++;
                }
            }
            _FirstRoom.AddLinkedRoom(_SecondRoom);
            _SecondRoom.AddLinkedRoom(_FirstRoom);
        }
    }

    private Room GenerateSpecialRoom(Vector2Int _MapSize)
    {
        Vector2Int bottomLeftCorner = new Vector2Int(Random.Range(1, _MapSize.x - 10), Random.Range(1, _MapSize.y - 10));
        Vector2Int size = new Vector2Int(6, 6);
        Room room = new Room(bottomLeftCorner, size);

        I_Tile stairs = GenerateTile("Tiles/TileStairs", bottomLeftCorner.x + 3, bottomLeftCorner.y + 3, room);
        m_StairsPosition = new Vector2Int(bottomLeftCorner.x + 3, bottomLeftCorner.y + 3);
        I_Tile activator = null;
        m_StairsTriggerPosition = new Vector2Int();
        int rand = Random.Range(0, 4);
        if (rand == 0)
        {
            m_StairsTriggerPosition = new Vector2Int(bottomLeftCorner.x + 1, bottomLeftCorner.y + Random.Range(1, 6));
        }
        else if (rand == 1)
        {
            m_StairsTriggerPosition = new Vector2Int(bottomLeftCorner.x + 5, bottomLeftCorner.y + Random.Range(1, 6));
        }
        else if (rand == 2)
        {
            m_StairsTriggerPosition = new Vector2Int(bottomLeftCorner.x + Random.Range(1, 6), bottomLeftCorner.y + 1);
        }
        else if (rand == 3)
        {
            m_StairsTriggerPosition = new Vector2Int(bottomLeftCorner.x + Random.Range(1, 6), bottomLeftCorner.y + 5);
        }
        activator = GenerateTile("Tiles/TileShakeBell", m_StairsTriggerPosition.x, m_StairsTriggerPosition.y, room);
        Tile trucMoche = activator as Tile;
        TileEventShake trucEncorePlusMoche = trucMoche.GetComponent<TileEventShake>();
        Tile trucSuperMoche = stairs as Tile;
        I_Activable trucSuperPlusMoche = trucSuperMoche.GetComponent<I_Activable>();
        trucEncorePlusMoche.SetTileToActivate(trucSuperPlusMoche);

        return room;
    }

    private Room GenerateRoom(Vector2Int _MapSize, Vector2Int _MinRoomSize, Vector2Int _MaxRoomSize)
    {
        Vector2Int bottomLeftCorner = new Vector2Int(Random.Range(1, _MapSize.x - _MinRoomSize.x), Random.Range(1, _MapSize.y - _MinRoomSize.y));
        Vector2Int maxSize = _MaxRoomSize;
        if (bottomLeftCorner.x + maxSize.x >= _MapSize.x)
        {
            maxSize.x = _MapSize.x - bottomLeftCorner.x - 1;
        }
        if (bottomLeftCorner.y + maxSize.y >= _MapSize.y)
        {
            maxSize.y = _MapSize.y - bottomLeftCorner.y - 1;
        }
        Vector2Int size = new Vector2Int(Random.Range(_MinRoomSize.x, maxSize.x + 1), Random.Range(_MinRoomSize.y, maxSize.y + 1));
        return new Room(bottomLeftCorner, size);
    }

    private bool HasCollisionsWithOtherRooms(Room _Room)
    {
        bool hasCollisions = false;
        for (int i = 0; i < m_Rooms.Count && !hasCollisions; i++)
        {
            hasCollisions = RoomsAreInCollision(_Room, m_Rooms[i]);
        }
        return hasCollisions;
    }

    private bool RoomsAreInCollision(Room _FirstRoom, Room _SecondRoom)
    {
        bool hasVerticalCollision = (_FirstRoom.GetUp() + 1 >= _SecondRoom.GetDown() && _FirstRoom.GetUp() + 1 <= _SecondRoom.GetUp()) || (_FirstRoom.GetDown() - 1 >= _SecondRoom.GetDown() && _FirstRoom.GetDown() - 1 <= _SecondRoom.GetUp());
        bool hasHorizontalCollision = (_FirstRoom.GetRight() + 1 >= _SecondRoom.GetLeft() && _FirstRoom.GetRight() + 1 <= _SecondRoom.GetRight()) || (_FirstRoom.GetLeft() - 1 >= _SecondRoom.GetLeft() && _FirstRoom.GetLeft() - 1 <= _SecondRoom.GetRight());
        bool hasVerticalInclusion = (_FirstRoom.GetUp() + 1 >= _SecondRoom.GetUp() && _FirstRoom.GetDown() - 1 <= _SecondRoom.GetDown()) || (_FirstRoom.GetDown() - 1 >= _SecondRoom.GetDown() && _FirstRoom.GetUp() + 1 <= _SecondRoom.GetUp());
        bool hasHorizontalInclusion = (_FirstRoom.GetRight() + 1 >= _SecondRoom.GetRight() && _FirstRoom.GetLeft() - 1 <= _SecondRoom.GetLeft()) || (_FirstRoom.GetLeft() - 1 >= _SecondRoom.GetLeft() && _FirstRoom.GetRight() + 1 <= _SecondRoom.GetRight());
        bool hasCollisions = (hasVerticalCollision && hasHorizontalCollision) || (hasVerticalCollision && hasHorizontalInclusion) || (hasVerticalInclusion && hasHorizontalCollision) || (hasVerticalInclusion && hasHorizontalInclusion);
        return hasCollisions;
    }

    private I_Tile GenerateTile(string _Path, int _X, int _Z, Room _Room)
    {
        I_Tile tileScript;
        if (!m_Tiles.TryGetValue(new Vector2Int(_X, _Z), out tileScript))
        {
            GameObject prefab = Resources.Load<GameObject>(_Path);
            if (prefab != null)
            {
                tileScript = null;
                GameObject tileGameObject = Instantiate(prefab, new Vector3(_X, 0, _Z), Quaternion.identity, transform);
                tileGameObject.name = "Tile [" + _X + ", " + _Z + "]";
                if (tileGameObject != null)
                {
                    tileScript = tileGameObject.GetComponent<I_Tile>();
                    if (tileScript != null)
                    {
                        tileScript.SetPrefabName(_Path);
                        m_Tiles[new Vector2Int(_X, _Z)] = tileScript;
                        if (_Path == "Tiles/TileWall")
                        {
                            m_Walls.Add(tileScript);
                        }
                        else if (_Path == "Tiles/TileEmpty")
                        {
                            m_Floors.Add(tileScript);
                        }
                        if (_Room != null)
                        {
                            tileScript.SetRoom(_Room);
                            _Room.AddTile(tileScript);
                        }
                        I_Tile northTile;
                        I_Tile southTile;
                        I_Tile eastTile;
                        I_Tile westTile;
                        I_Tile northEastTile = null;
                        I_Tile northWestTile = null;
                        I_Tile southEastTile = null;
                        I_Tile southWestTile = null;
                        if (m_Tiles.TryGetValue(new Vector2Int(_X, _Z + 1), out northTile))
                        {
                            if (northEastTile == null)
                            {
                                northEastTile = northTile.GetNeighbour(E_Direction.East);
                            }
                            if (northWestTile == null)
                            {
                                northWestTile = northTile.GetNeighbour(E_Direction.West);
                            }
                        }
                        if (m_Tiles.TryGetValue(new Vector2Int(_X, _Z - 1), out southTile))
                        {
                            if (southEastTile == null)
                            {
                                southEastTile = southTile.GetNeighbour(E_Direction.East);
                            }
                            if (southWestTile == null)
                            {
                                southWestTile = southTile.GetNeighbour(E_Direction.West);
                            }
                        }
                        if (m_Tiles.TryGetValue(new Vector2Int(_X + 1, _Z), out eastTile))
                        {
                            if (northEastTile == null)
                            {
                                northEastTile = eastTile.GetNeighbour(E_Direction.North);
                            }
                            if (southEastTile == null)
                            {
                                southEastTile = eastTile.GetNeighbour(E_Direction.South);
                            }
                        }
                        if (m_Tiles.TryGetValue(new Vector2Int(_X - 1, _Z), out westTile))
                        {
                            if (northWestTile == null)
                            {
                                northWestTile = westTile.GetNeighbour(E_Direction.North);
                            }
                            if (southWestTile == null)
                            {
                                southWestTile = westTile.GetNeighbour(E_Direction.South);
                            }
                        }

                        if (northTile != null)
                        {
                            tileScript.SetNeighbour(E_Direction.North, northTile);
                            northTile.SetNeighbour(E_Direction.South, tileScript);
                        }
                        if (southTile != null)
                        {
                            tileScript.SetNeighbour(E_Direction.South, southTile);
                            southTile.SetNeighbour(E_Direction.North, tileScript);
                        }
                        if (eastTile != null)
                        {
                            tileScript.SetNeighbour(E_Direction.East, eastTile);
                            eastTile.SetNeighbour(E_Direction.West, tileScript);
                        }
                        if (westTile != null)
                        {
                            tileScript.SetNeighbour(E_Direction.West, westTile);
                            westTile.SetNeighbour(E_Direction.East, tileScript);
                        }
                        if (northEastTile != null)
                        {
                            tileScript.SetNeighbour(E_Direction.NorthEast, northEastTile);
                            northEastTile.SetNeighbour(E_Direction.SouthWest, tileScript);
                        }
                        if (northWestTile != null)
                        {
                            tileScript.SetNeighbour(E_Direction.NorthWest, northWestTile);
                            northWestTile.SetNeighbour(E_Direction.SouthEast, tileScript);
                        }
                        if (southEastTile != null)
                        {
                            tileScript.SetNeighbour(E_Direction.SouthEast, southEastTile);
                            southEastTile.SetNeighbour(E_Direction.NorthWest, tileScript);
                        }
                        if (southWestTile != null)
                        {
                            tileScript.SetNeighbour(E_Direction.SouthWest, southWestTile);
                            southWestTile.SetNeighbour(E_Direction.NorthEast, tileScript);
                        }
                    }
                }
            }
        }
        return tileScript;
    }

    public I_Tile GetTileAt(int _X, int _Z)
    {
        I_Tile tile;
        m_Tiles.TryGetValue(new Vector2Int(_X, _Z), out tile);
        return tile;
    }

    public I_Tile GetTileAt(Vector2Int _Position)
    {
        I_Tile tile;
        m_Tiles.TryGetValue(_Position, out tile);
        return tile;
    }
    
    public void SaveMap()
    {
        MapData mapData = new MapData();
        List<TileData> tiles = new List<TileData>();
        for (int i = 0; i < m_Floors.Count; i++)
        {
            TileData tileData = new TileData();
            tileData.m_PosX = Mathf.RoundToInt(m_Floors[i].GetPosition().x);
            tileData.m_PosZ = Mathf.RoundToInt(m_Floors[i].GetPosition().z);
            Room room = m_Floors[i].GetRoom();
            int roomIndex = -1;
            for (int j = 0; j < m_Rooms.Count && roomIndex < 0; j++)
            {
                if (room == m_Rooms[j])
                {
                    roomIndex = j;
                }
            }
            tileData.m_RoomIndex = roomIndex;
            tiles.Add(tileData);
        }
        mapData.Tiles = tiles;
        List<RoomData> roomData = new List<RoomData>();
        for (int i = 0; i < m_Rooms.Count; i++)
        {
            RoomData room = new RoomData();
            room.m_PosX = m_Rooms[i].GetBottomLeftCorner().x;
            room.m_PosZ = m_Rooms[i].GetBottomLeftCorner().y;
            room.m_SizeX = m_Rooms[i].GetSize().x;
            room.m_SizeZ = m_Rooms[i].GetSize().y;
            roomData.Add(room);
        }
        mapData.Rooms = roomData;
        mapData.MapSizeX = m_MapSize.x;
        mapData.MapSizeZ = m_MapSize.y;
        mapData.StairsPositionX = m_StairsPosition.x;
        mapData.StairsPositionZ = m_StairsPosition.y;
        mapData.StairsTriggerPositionX = m_StairsTriggerPosition.x;
        mapData.StairsTriggerPositionZ = m_StairsTriggerPosition.y;
        SaveManager.SaveMap(mapData);
    }
}
