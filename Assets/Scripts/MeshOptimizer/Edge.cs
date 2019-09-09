using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    private Vertex m_FirstVertex;
    private Vertex m_SecondVertex;

    public Vertex FirstVertex
    {
        get
        {
            return m_FirstVertex;
        }
    }

    public Vertex SecondVertex
    {
        get
        {
            return m_SecondVertex;
        }
    }

    public Edge(Vertex _FirstVertex, Vertex _SecondVertex)
    {
        m_FirstVertex = _FirstVertex;
        m_SecondVertex = _SecondVertex;
    }

    public Edge(Vector3 m_FirstPosition, Vector3 m_SecondPosition)
    {
        m_FirstVertex = new Vertex(m_FirstPosition);
        m_SecondVertex = new Vertex(m_SecondPosition);
    }
    
    public void FlipEdge()
    {
        Vertex temporaryVertex = m_FirstVertex;
        m_FirstVertex = m_SecondVertex;
        m_SecondVertex = temporaryVertex;
    }

    public void Display(Color _Color, float _Time)
    {
        Debug.DrawLine(FirstVertex.Position, SecondVertex.Position, _Color, _Time);
    }

    public bool IsIntersectingWith(Edge _Edge)
    {
        bool isIntersecting = false;
        
        Vector2 p1 = new Vector2(m_FirstVertex.Position.x, m_FirstVertex.Position.z);
        Vector2 p2 = new Vector2(m_SecondVertex.Position.x, m_SecondVertex.Position.z);

        Vector2 p3 = new Vector2(_Edge.FirstVertex.Position.x, _Edge.FirstVertex.Position.z);
        Vector2 p4 = new Vector2(_Edge.SecondVertex.Position.x, _Edge.SecondVertex.Position.z);

        bool isFirstVertexIncluded = Vector3.Distance(FirstVertex.Position, _Edge.FirstVertex.Position) < 0.5f || Vector3.Distance(FirstVertex.Position, _Edge.SecondVertex.Position) < 0.5f;
        bool isSecondVertexIncluded = Vector3.Distance(SecondVertex.Position, _Edge.FirstVertex.Position) < 0.5f || Vector3.Distance(SecondVertex.Position, _Edge.SecondVertex.Position) < 0.5f;
        if (!isFirstVertexIncluded && !isSecondVertexIncluded)
        {
            float denominator = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);
            
            if (denominator != 0)
            {
                float u_a = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
                float u_b = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) / denominator;
                
                if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
                {
                    isIntersecting = true;
                }
            }
        }

        return isIntersecting;
    }
}
