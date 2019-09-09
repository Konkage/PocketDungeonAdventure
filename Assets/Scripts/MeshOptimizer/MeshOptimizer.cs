using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MeshOptimizer
{
    public static Mesh GenerateOptimisedMesh(List<Vertex> _AllVertices, List<Vertex> _ExternalLoop, List<List<Vertex>> _AllLoops)
    {
        Mesh mesh = new Mesh();
        List<Triangle> triangles = new List<Triangle>();
        List<Edge> internalEdges = new List<Edge>();

        for (int i = 0; i < _AllLoops.Count; i++)
        {
            for (int j = 0; j < _AllLoops[i].Count - 1; j++)
            {
                internalEdges.Add(new Edge(_AllLoops[i][j], _AllLoops[i][j + 1]));
            }
            internalEdges.Add(new Edge(_AllLoops[i][_AllLoops[i].Count - 1], _AllLoops[i][0]));
        }
        for (int i = 0; i < _ExternalLoop.Count - 1; i++)
        {
            internalEdges.Add(new Edge(_ExternalLoop[i], _ExternalLoop[i + 1]));
        }
        internalEdges.Add(new Edge(_ExternalLoop[_ExternalLoop.Count - 1], _ExternalLoop[0]));

        List<Edge> verifEdge = new List<Edge>();
        List<Edge> internalEdgeToNotVerify = new List<Edge>();
        internalEdgeToNotVerify.AddRange(internalEdges);
        
        
        int baseInternalEdgesCount = internalEdges.Count;

        for (int i = 0; i < baseInternalEdgesCount; i++)
        {
            for (int j = 0; j < _ExternalLoop.Count; j++)
            {
                if (internalEdges[i].FirstVertex.Position != _ExternalLoop[j].Position && internalEdges[i].SecondVertex.Position != _ExternalLoop[j].Position && IsPointGoingRightWay(internalEdges[i], _ExternalLoop[j]))
                {
                    Edge secondEdge = new Edge(_ExternalLoop[j], internalEdges[i].SecondVertex);
                    Edge thirdEdge = new Edge(internalEdges[i].FirstVertex, _ExternalLoop[j]);
                    Triangle triangle = new Triangle(internalEdges[i].FirstVertex, _ExternalLoop[j], internalEdges[i].SecondVertex);
                    if (!IsTriangleAlreadyExisting(triangle, triangles) && CanCreateEdge(secondEdge, internalEdges) && CanCreateEdge(thirdEdge, internalEdges))
                    {
                        internalEdges.Add(secondEdge);
                        internalEdges.Add(thirdEdge);
                        AddEdgeToVerif(secondEdge, verifEdge, internalEdgeToNotVerify);
                        AddEdgeToVerif(thirdEdge, verifEdge, internalEdgeToNotVerify);
                        triangles.Add(triangle);
                    }
                }
            }

            for (int loopIndex = 0; loopIndex < _AllLoops.Count; loopIndex++)
            {
                for (int j = 0; j < _AllLoops[loopIndex].Count; j++)
                {
                    if (internalEdges[i].FirstVertex.Position != _AllLoops[loopIndex][j].Position && internalEdges[i].SecondVertex.Position != _AllLoops[loopIndex][j].Position && IsPointGoingRightWay(internalEdges[i], _AllLoops[loopIndex][j]))
                    {
                        Edge secondEdge = new Edge(_AllLoops[loopIndex][j], internalEdges[i].SecondVertex);
                        Edge thirdEdge = new Edge(internalEdges[i].FirstVertex, _AllLoops[loopIndex][j]);
                        Triangle triangle = new Triangle(internalEdges[i].FirstVertex, _AllLoops[loopIndex][j], internalEdges[i].SecondVertex);
                        if (!IsTriangleAlreadyExisting(triangle, triangles) && CanCreateEdge(secondEdge, internalEdges) && CanCreateEdge(thirdEdge, internalEdges))
                        {
                            internalEdges.Add(secondEdge);
                            internalEdges.Add(thirdEdge);
                            AddEdgeToVerif(secondEdge, verifEdge, internalEdgeToNotVerify);
                            AddEdgeToVerif(thirdEdge, verifEdge, internalEdgeToNotVerify);
                            triangles.Add(triangle);
                        }
                    }
                }
            }
        }

        List<Edge> edgesToLink;
        int security = 0;
        while (verifEdge.Count > 0 && security < 10000)
        {
            edgesToLink = new List<Edge>();
            edgesToLink.AddRange(verifEdge);

            for (int i = 0; i < edgesToLink.Count; i++)
            {
                for (int j = 0; j < edgesToLink.Count; j++)
                {
                    if (edgesToLink[i].SecondVertex.Position == edgesToLink[j].FirstVertex.Position && edgesToLink[i].FirstVertex.Position != edgesToLink[j].SecondVertex.Position)
                    {
                        if (IsPointGoingRightWay(edgesToLink[i], edgesToLink[j].SecondVertex))
                        {
                            Edge missingEdge = new Edge(edgesToLink[i].FirstVertex, edgesToLink[j].SecondVertex);
                            Triangle triangle = new Triangle(edgesToLink[i].FirstVertex, edgesToLink[j].SecondVertex, edgesToLink[i].SecondVertex);
                            if (!IsTriangleAlreadyExisting(triangle, triangles) && CanCreateEdge(missingEdge, internalEdges))
                            {
                                internalEdges.Add(missingEdge);
                                triangles.Add(triangle);
                                AddEdgeToVerif(edgesToLink[i], verifEdge, internalEdgeToNotVerify);
                                AddEdgeToVerif(edgesToLink[j], verifEdge, internalEdgeToNotVerify);
                                AddEdgeToVerif(missingEdge, verifEdge, internalEdgeToNotVerify);
                            }
                        }
                    }
                }
            }
            security++;
        }


        for (int i = 0; i < verifEdge.Count; i++)
        {
            verifEdge[i].Display(Color.blue, 60);
        }

        List<Vertex> allLoopVertices = new List<Vertex>();
        for (int i = 0; i < _AllLoops.Count; i++)
        {
            allLoopVertices.AddRange(_AllLoops[i]);
        }


        Vector3[] meshVertices = new Vector3[_ExternalLoop.Count + allLoopVertices.Count];
        for (int i = 0; i < _ExternalLoop.Count + allLoopVertices.Count; i++)
        {
            if (i < _ExternalLoop.Count)
            {
                meshVertices[i] = _ExternalLoop[i].Position;
                _ExternalLoop[i].Index = i;
            }
            else
            {
                meshVertices[i] = allLoopVertices[i - _ExternalLoop.Count].Position;
                allLoopVertices[i - _ExternalLoop.Count].Index = i;
            }
        }


        int[] meshTriangles = new int[triangles.Count * 3];
        for (int i = 0; i < triangles.Count; i++)
        {
            meshTriangles[i * 3] = triangles[i].FirstCorner.Index;
            meshTriangles[i * 3 + 1] = triangles[i].SecondCorner.Index;
            meshTriangles[i * 3 + 2] = triangles[i].ThirdCorner.Index;
        }

        mesh.vertices = meshVertices;
        mesh.triangles = meshTriangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private static bool IsTriangleAlreadyExisting(Triangle _Triangle, List<Triangle> _Triangles)
    {
        bool isAlreadyExisting = false;
        Vertex firstPoint = _Triangle.FirstCorner;
        Vertex secondPoint = _Triangle.SecondCorner;
        Vertex thirdPoint = _Triangle.ThirdCorner;
        for (int i = 0; i < _Triangles.Count && !isAlreadyExisting; i++)
        {
            if (IsTriangleContainingVertex(_Triangles[i], firstPoint) && IsTriangleContainingVertex(_Triangles[i], secondPoint) && IsTriangleContainingVertex(_Triangles[i], thirdPoint))
            {
                isAlreadyExisting = true;
            }
        }
        return isAlreadyExisting;
    }

    private static bool IsTriangleContainingVertex(Triangle _Triangle, Vertex _Vertex)
    {
        return (_Triangle.FirstCorner.Position == _Vertex.Position) || (_Triangle.SecondCorner.Position == _Vertex.Position) || (_Triangle.ThirdCorner.Position == _Vertex.Position);
    }

    private static void AddEdgeToVerif(Edge _Edge, List<Edge> _VerifEdge, List<Edge> _InternalEdges)
    {
        bool hasRemoved = false;

        bool isInternal = false;
        for (int j = 0; j < _InternalEdges.Count && !isInternal; j++)
        {
            if ((_InternalEdges[j].FirstVertex.Position == _Edge.FirstVertex.Position && _InternalEdges[j].SecondVertex.Position == _Edge.SecondVertex.Position) || (_InternalEdges[j].SecondVertex.Position == _Edge.FirstVertex.Position && _InternalEdges[j].FirstVertex.Position == _Edge.SecondVertex.Position))
            {
                isInternal = true;
            }
        }
        if (!isInternal)
        {
            for (int i = 0; i < _VerifEdge.Count && !hasRemoved; i++)
            {
                if ((_VerifEdge[i].FirstVertex.Position == _Edge.FirstVertex.Position && _VerifEdge[i].SecondVertex.Position == _Edge.SecondVertex.Position) || (_VerifEdge[i].SecondVertex.Position == _Edge.FirstVertex.Position && _VerifEdge[i].FirstVertex.Position == _Edge.SecondVertex.Position))
                {
                    hasRemoved = true;
                    _VerifEdge.RemoveAt(i);
                }
            }
            if (!hasRemoved)
            {
                _VerifEdge.Add(_Edge);
            }
        }
    }

    private static bool IsPointGoingRightWay(Edge _Edge, Vertex _PointToAdd)
    {
        Vector3 edgeDirection = _Edge.SecondVertex.Position - _Edge.FirstVertex.Position;
        Vector3 wantedEdgeDirection = _PointToAdd.Position - _Edge.FirstVertex.Position;
        return Vector3.SignedAngle(edgeDirection, wantedEdgeDirection, Vector3.down) >= 0;
    }

    private static bool CanCreateEdge(Edge _Edge, List<Edge> _StartingEdges)
    {
        bool isValid = true;
        for (int i = 0; i < _StartingEdges.Count && isValid; i++)
        {
            isValid = !_Edge.IsIntersectingWith(_StartingEdges[i]);
        }
        return isValid;
    }
}