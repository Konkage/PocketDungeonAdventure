using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    private Vertex m_FirstCorner;
    private Vertex m_SecondCorner;
    private Vertex m_ThirdCorner;
    
    private HalfEdge m_HalfEdge;

    public Vertex FirstCorner
    {
        get
        {
            return m_FirstCorner;
        }
        set
        {
            m_FirstCorner = value;
        }
    }

    public Vertex SecondCorner
    {
        get
        {
            return m_SecondCorner;
        }
        set
        {
            m_SecondCorner = value;
        }
    }

    public Vertex ThirdCorner
    {
        get
        {
            return m_ThirdCorner;
        }
        set
        {
            m_ThirdCorner = value;
        }
    }

    public HalfEdge HalfEdge
    {
        get
        {
            return m_HalfEdge;
        }
        set
        {
            m_HalfEdge = value;
        }
    }

    public Triangle(Vertex _FirstCorner, Vertex _SecondCorner, Vertex _ThirdCorner)
    {
        m_FirstCorner = _FirstCorner;
        m_SecondCorner = _SecondCorner;
        m_ThirdCorner = _ThirdCorner;
    }

    public Triangle(Vector3 _FirstCorner, Vector3 _SecondCorner, Vector3 _ThirdCorner)
    {
        m_FirstCorner = new Vertex(_FirstCorner);
        m_SecondCorner = new Vertex(_SecondCorner);
        m_ThirdCorner = new Vertex(_ThirdCorner);
    }

    public Triangle(HalfEdge _HalfEdge)
    {
        m_HalfEdge = _HalfEdge;
    }
    
    public void ChangeOrientation()
    {
        Vertex temporaryPoint = m_FirstCorner;
        m_FirstCorner = m_SecondCorner;
        m_SecondCorner = temporaryPoint;
    }

    public void Display(Color _Color, float _Time)
    {
        Debug.DrawLine(m_FirstCorner.Position, m_SecondCorner.Position, _Color, _Time);
        Debug.DrawLine(m_SecondCorner.Position, m_ThirdCorner.Position, _Color, _Time);
        Debug.DrawLine(m_ThirdCorner.Position, m_FirstCorner.Position, _Color, _Time);
    }
}
