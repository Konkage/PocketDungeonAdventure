using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    private Vector3 m_Position;

    private Vertex m_PreviousVertex;
    private Vertex m_NextVertex;

    private int m_IndexInVerticesList;

    public int Index
    {
        get
        {
            return m_IndexInVerticesList;
        }
        set
        {
            m_IndexInVerticesList = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return m_Position;
        }
    }

    public Vertex PreviousVertex
    {
        get
        {
            return m_PreviousVertex;
        }
        set
        {
            m_PreviousVertex = value;
        }
    }

    public Vertex NextVertex
    {
        get
        {
            return m_NextVertex;
        }
        set
        {
            m_NextVertex = value;
        }
    }

    public Vertex(Vector3 position)
    {
        m_Position = position;
    }
}
