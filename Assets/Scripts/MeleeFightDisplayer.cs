using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeFightDisplayer : MonoBehaviour
{
    [SerializeField]
    private Image m_DefenseCircleUp;
    [SerializeField]
    private Image m_DefenseCircleDown;
    [SerializeField]
    private Image m_WeaknessCircleUp;
    [SerializeField]
    private Image m_WeaknessCircleDown;

    private bool m_HasShownWeakness = false;
    private float m_WeaknessValue = 0;
    private float m_TimeAfterWhichWeaknessDecrease = 0;
    private float m_WeaknessSpeed = 0;
    private E_Direction m_WeaknessDirection;

    private void Update()
    {
        if (m_HasShownWeakness)
        {
            if (m_TimeAfterWhichWeaknessDecrease > 0)
            {
                m_TimeAfterWhichWeaknessDecrease -= Time.unscaledDeltaTime;
            }
            if (m_TimeAfterWhichWeaknessDecrease < 0)
            {
                m_WeaknessValue -= m_WeaknessSpeed * Time.unscaledDeltaTime;
                if (m_WeaknessValue <= 0)
                {
                    m_WeaknessValue = 0;
                    m_HasShownWeakness = false;
                }
                SetImagesFillPercent(m_WeaknessValue);
            }
        }
        else
        {
            SetWeakness((E_Direction)Random.Range(1, 5), 0.3f, 0.2f, 1.0f);
        }
    }

    public void SetWeakness(E_Direction _Direction, float _WeaknessValue, float _WeaknessSpeed, float _TimeAfterWhichWeaknessDecrease)
    {
        SetImagesRotations(_Direction);
        SetImagesFillPercent(_WeaknessValue);
        m_WeaknessValue = _WeaknessValue;
        m_WeaknessSpeed = _WeaknessSpeed;
        m_TimeAfterWhichWeaknessDecrease = _TimeAfterWhichWeaknessDecrease;
        m_HasShownWeakness = true;
        m_WeaknessDirection = _Direction;
    }

    private void SetImagesRotations(E_Direction _Direction)
    {
        int defenseFillOrigin = 0;
        int weaknessFillOrigin = 0;
        if (_Direction == E_Direction.North)
        {
            defenseFillOrigin = 0;
            weaknessFillOrigin = 2;
        }
        else if (_Direction == E_Direction.South)
        {
            defenseFillOrigin = 2;
            weaknessFillOrigin = 0;
        }
        else if (_Direction == E_Direction.East)
        {
            defenseFillOrigin = 3;
            weaknessFillOrigin = 1;
        }
        else if (_Direction == E_Direction.West)
        {
            defenseFillOrigin = 1;
            weaknessFillOrigin = 3;
        }
        m_DefenseCircleUp.fillOrigin = defenseFillOrigin;
        m_DefenseCircleDown.fillOrigin = defenseFillOrigin;
        m_WeaknessCircleUp.fillOrigin = weaknessFillOrigin;
        m_WeaknessCircleDown.fillOrigin = weaknessFillOrigin;
    }

    private void SetImagesFillPercent(float _FillPercent)
    {
        float fillPercent = _FillPercent * 0.5f;
        m_DefenseCircleUp.fillAmount = 0.5f - fillPercent;
        m_DefenseCircleDown.fillAmount = 0.5f - fillPercent;
        m_WeaknessCircleUp.fillAmount = fillPercent;
        m_WeaknessCircleDown.fillAmount = fillPercent;
    }
    
    public bool IsSlashHitting(Vector3 _Start, Vector3 _End)
    {
        bool isSlashTouching = false;
        bool isSlashValid = false;

        Vector3 slash = _End - _Start;
        if (m_WeaknessDirection == E_Direction.North)
        {
            if (Mathf.Abs(slash.x) < Mathf.Abs(slash.y))
            {
                isSlashValid = slash.y < 0;
            }
        }
        else if (m_WeaknessDirection == E_Direction.South)
        {
            if (Mathf.Abs(slash.x) < Mathf.Abs(slash.y))
            {
                isSlashValid = slash.y > 0;
            }
        }
        else if (m_WeaknessDirection == E_Direction.East)
        {
            if (Mathf.Abs(slash.x) > Mathf.Abs(slash.y))
            {
                isSlashValid = slash.x < 0;
            }
        }
        else if (m_WeaknessDirection == E_Direction.West)
        {
            if (Mathf.Abs(slash.x) > Mathf.Abs(slash.y))
            {
                isSlashValid = slash.x > 0;
            }
        }

        if (isSlashValid)
        {
            RectTransform rectTransform = transform as RectTransform;
            Vector3 centerPoint = new Vector3(rectTransform.position.x, rectTransform.position.y, 0);
            Vector3 middlePoint = new Vector2();
            if (m_WeaknessDirection == E_Direction.North)
            {
                middlePoint = centerPoint + new Vector3(0, rectTransform.rect.yMax * rectTransform.lossyScale.y, 0);
            }
            else if (m_WeaknessDirection == E_Direction.South)
            {
                middlePoint = centerPoint + new Vector3(0, rectTransform.rect.yMin * rectTransform.lossyScale.y, 0);
            }
            else if (m_WeaknessDirection == E_Direction.East)
            {
                middlePoint = centerPoint + new Vector3(rectTransform.rect.xMax * rectTransform.lossyScale.y, 0, 0);
            }
            else if (m_WeaknessDirection == E_Direction.West)
            {
                middlePoint = centerPoint + new Vector3(rectTransform.rect.xMin * rectTransform.lossyScale.y, 0, 0);
            }
            Vector3 middleAxis = middlePoint - centerPoint;
            Vector3 firstPoint = centerPoint + (Quaternion.Euler(0, 0, m_WeaknessValue * 180) * middleAxis);
            Vector3 secondPoint = centerPoint + (Quaternion.Euler(0, 0, -m_WeaknessValue * 180) * middleAxis);
            isSlashTouching = DoSegmentsIntersect(_Start, _End, firstPoint, middlePoint) || DoSegmentsIntersect(_Start, _End, secondPoint, middlePoint);
        }
        return isSlashTouching;
    }

    private bool DoSegmentsIntersect(Vector2 _FirstPoint, Vector2 _SecondPoint, Vector2 _ThirdPoint, Vector2 _FourthPoint)
    {
        bool doSegmentsIntersect = false;
        
        float progressDiviser = (_FourthPoint.y - _ThirdPoint.y) * (_SecondPoint.x - _FirstPoint.x) - (_FourthPoint.x - _ThirdPoint.x) * (_SecondPoint.y - _FirstPoint.y);
        if (progressDiviser != 0)
        {
            float progressOnFirstSegment = ((_FourthPoint.x - _ThirdPoint.x) * (_FirstPoint.y - _ThirdPoint.y) - (_FourthPoint.y - _ThirdPoint.y) * (_FirstPoint.x - _ThirdPoint.x)) / progressDiviser;
            float progressOnSecondSegment = ((_SecondPoint.x - _FirstPoint.x) * (_FirstPoint.y - _ThirdPoint.y) - (_SecondPoint.y - _FirstPoint.y) * (_FirstPoint.x - _ThirdPoint.x)) / progressDiviser;

            bool isIntersectionOnFirstSegment = 0 <= progressOnFirstSegment && progressOnFirstSegment <= 1;
            bool isIntersectionOnSecondSegment = 0 <= progressOnSecondSegment && progressOnSecondSegment <= 1;
            doSegmentsIntersect = isIntersectionOnFirstSegment && isIntersectionOnSecondSegment;
        }
        return doSegmentsIntersect;
    }
}
