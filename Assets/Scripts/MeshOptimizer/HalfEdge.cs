using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfEdge
{
    private Vertex m_Vertex;
    private Triangle m_Triangle;

    private HalfEdge m_NextEdge;
    private HalfEdge m_PreviousEdge;
    private HalfEdge m_OppositeEdge;

    public Vertex Vertex
    {
        get
        {
            return m_Vertex;
        }
        set
        {
            m_Vertex = value;
        }
    }

    public Triangle Triangle
    {
        get
        {
            return m_Triangle;
        }
        set
        {
            m_Triangle = value;
        }
    }

    public HalfEdge PreviousEdge
    {
        get
        {
            return m_PreviousEdge;
        }
        set
        {
            m_PreviousEdge = value;
        }
    }

    public HalfEdge NextEdge
    {
        get
        {
            return m_NextEdge;
        }
        set
        {
            m_NextEdge = value;
        }
    }

    public HalfEdge OppositeEdge
    {
        get
        {
            return m_OppositeEdge;
        }
        set
        {
            m_OppositeEdge = value;
        }
    }

    public HalfEdge(Vertex _Vertex)
    {
        m_Vertex = _Vertex;
    }
}
