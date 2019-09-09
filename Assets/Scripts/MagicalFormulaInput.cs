using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalFormulaInput : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_Cursor;

    [SerializeField]
    private List<RectTransform> m_MagicalPoints;

    [SerializeField]
    private Transform m_SegmentContainer;

    private List<int> m_MagicalFormula;
    private List<RectTransform> m_Segments;
    private List<RectTransform> m_SegmentsToDestroy;
    private RectTransform m_CurrentSegment;

    private bool m_HasStartDrawing = false;

    public void StartDrawing(Vector2 _CursorPosition)
    {
        m_HasStartDrawing = true;
        m_Cursor.gameObject.SetActive(true);
        m_MagicalFormula = new List<int>();
        m_Segments = new List<RectTransform>();
        m_CurrentSegment = null;
        m_Cursor.position = transform.position;
    }

    public void UpdateDrawing(Vector2 _CursorPosition)
    {
        if (m_HasStartDrawing)
        {
            if ((transform as RectTransform).rect.Contains(_CursorPosition - new Vector2(transform.position.x, transform.position.y)))
            {
                m_Cursor.position = _CursorPosition;
            }
            for (int i = 0; i < m_MagicalPoints.Count; i++)
            {
                if (Vector2.Distance(_CursorPosition, m_MagicalPoints[i].position) < 50)
                {
                    if (m_MagicalFormula.Count > 0)
                    {
                        if (m_MagicalFormula[m_MagicalFormula.Count - 1] != i)
                        {
                            PlaceAndRotateSegmentToFit(m_MagicalPoints[m_MagicalFormula[m_MagicalFormula.Count - 1]].position, m_MagicalPoints[i].position);
                            m_Segments.Add(m_CurrentSegment);
                            m_MagicalFormula.Add(i);
                            GameObject segment = Instantiate(Resources.Load<GameObject>("UI/MagicalSegment"), m_SegmentContainer);
                            m_CurrentSegment = segment.transform as RectTransform;
                        }
                    }
                    else
                    {
                        m_MagicalFormula.Add(i);
                        GameObject segment = Instantiate(Resources.Load<GameObject>("UI/MagicalSegment"), m_SegmentContainer);
                        m_CurrentSegment = segment.transform as RectTransform;
                    }
                }
            }

            if (m_MagicalFormula.Count > 0)
            {
                if (m_CurrentSegment != null)
                {
                    PlaceAndRotateSegmentToFit(m_MagicalPoints[m_MagicalFormula[m_MagicalFormula.Count - 1]].position, m_Cursor.position);
                }
            }
        }
    }

    public List<int> EndDrawing(Vector2 _CursorPosition)
    {
        if (m_HasStartDrawing)
        {
            m_HasStartDrawing = false;
            m_Cursor.gameObject.SetActive(false);
            if (m_CurrentSegment != null)
            {
                Destroy(m_CurrentSegment.gameObject);
                m_CurrentSegment = null;
                m_SegmentsToDestroy = new List<RectTransform>();
                m_SegmentsToDestroy.AddRange(m_Segments);
                for (int i = 0; i < m_SegmentsToDestroy.Count; i++)
                {
                    Destroy(m_SegmentsToDestroy[i].gameObject);
                }
            }
        }
        else
        {
            m_MagicalFormula = new List<int>();
        }
        return m_MagicalFormula;
    }

    private void PlaceAndRotateSegmentToFit(Vector3 _Start, Vector3 _End)
    {
        Vector3 segmentRaw = _End - _Start;
        m_CurrentSegment.position = _Start + (segmentRaw * 0.5f);
        m_CurrentSegment.sizeDelta = new Vector2(segmentRaw.magnitude, 30);
        m_CurrentSegment.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(new Vector3(1, 0, 0), segmentRaw, new Vector3(0, 0, 1)));
    }
}
