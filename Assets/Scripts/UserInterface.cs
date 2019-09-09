using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnUserInterfaceButtonPressed();

public class UserInterface : MonoBehaviour
{
    private static UserInterface m_Instance;

    [SerializeField]
    private GameObject m_LoadingScreen;
    [SerializeField]
    private GameObject m_Menu;
    [SerializeField]
    private MeleeFightDisplayer m_MeleeFightDisplayer;
    [SerializeField]
    private MagicalFormulaInput m_MagicalFormulaDisplayer;
    [SerializeField]
    private ShakeDisplayer m_ShakeBellDisplayer;

    [SerializeField]
    private Transform m_SegmentContainer;
    [SerializeField]
    private GameObject m_SegmentMovementPrefab;
    [SerializeField]
    private GameObject m_SegmentSlashPrefab;

    [SerializeField]
    private GameObject m_MenuElementPrefab;

    private List<MenuElement> m_MenuElements;
    private RectTransform m_CurrentSegment;

    private void Awake()
    {
        m_Instance = this;
    }

    private static UserInterface GetInstance()
    {
        if (m_Instance == null)
        {
            GameObject.Instantiate(Resources.Load<GameObject>("UI/UserInterface"));
        }
        return m_Instance;
    }


    public static void HideMenu()
    {
        GetInstance().InternalHideMenu();
    }

    private void InternalHideMenu()
    {
        for (int i = 0; i < m_MenuElements.Count; i++)
        {
            Destroy(m_MenuElements[i].gameObject);
        }
        m_Menu.SetActive(false);
    }



    public static void DisplayMenu(List<KeyValuePair<string, OnUserInterfaceButtonPressed>> _MenuElements)
    {
        GetInstance().InternalDisplayMenu(_MenuElements);
    }

    private void InternalDisplayMenu(List<KeyValuePair<string, OnUserInterfaceButtonPressed>> _MenuElements)
    {
        m_Menu.SetActive(true);
        m_MenuElements = new List<MenuElement>();
        for (int i = 0; i < _MenuElements.Count; i++)
        {
            GameObject menuElement = Instantiate(m_MenuElementPrefab, m_Menu.transform);
            MenuElement menuElementScript = menuElement.GetComponent<MenuElement>();
            menuElementScript.SetEvent(_MenuElements[i]);
            m_MenuElements.Add(menuElementScript);
        }
    }



    public static void DisplayMovementLine(Vector2 _StartPosition, Vector2 _EndPosition)
    {
        GetInstance().InternalDisplayMovementLine(_StartPosition, _EndPosition);
    }

    private void InternalDisplayMovementLine(Vector2 _StartPosition, Vector2 _EndPosition)
    {
        DisplayLine(_StartPosition, _EndPosition, m_SegmentMovementPrefab);
    }



    public static void DisplaySlashLine(Vector2 _StartPosition, Vector2 _EndPosition)
    {
        GetInstance().InternalDisplaySlashLine(_StartPosition, _EndPosition);
    }

    private void InternalDisplaySlashLine(Vector2 _StartPosition, Vector2 _EndPosition)
    {
        DisplayLine(_StartPosition, _EndPosition, m_SegmentSlashPrefab);
    }



    private void DisplayLine(Vector2 _StartPosition, Vector2 _EndPosition, GameObject _PrefabIfNull)
    {
        if (m_CurrentSegment == null)
        {
            GameObject segment = Instantiate(_PrefabIfNull, m_SegmentContainer);
            m_CurrentSegment = segment.transform as RectTransform;
        }
        Vector3 segmentRaw = _EndPosition - _StartPosition;
        m_CurrentSegment.position = new Vector3(_StartPosition.x, _StartPosition.y, 0) + (segmentRaw * 0.5f);
        m_CurrentSegment.sizeDelta = new Vector2(15, segmentRaw.magnitude);
        m_CurrentSegment.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(new Vector3(1, 0, 0), segmentRaw, new Vector3(0, 0, 1)) - 90);
    }



    public static void HideMovementLine()
    {
        GetInstance().DestroySegment();
    }

    public static void HideSlashLine()
    {
        GetInstance().DestroySegment();
    }

    private void DestroySegment()
    {
        if (m_CurrentSegment != null)
        {
            Destroy(m_CurrentSegment.gameObject);
            m_CurrentSegment = null;
        }
    }



    public static void DisplayLoadingScreen(bool _MustBeDisplayed)
    {
        GetInstance().InternalDisplayLoadingScreen(_MustBeDisplayed);
    }

    private void InternalDisplayLoadingScreen(bool _MustBeDisplayed)
    {
        m_LoadingScreen.SetActive(_MustBeDisplayed);
    }



    public static void ActivateMeleeFight(bool _MustBeActived)
    {
        GetInstance().InternalActivateMeleeFight(_MustBeActived);
    }

    private void InternalActivateMeleeFight(bool _MustBeActived)
    {
        m_MeleeFightDisplayer.gameObject.SetActive(_MustBeActived);
    }



    public static void SetMeleeFightWeakness(float _WeaknessValue, float _WeaknessSpeed, float _TimeAfterWhichWeaknessDecrease)
    {
        GetInstance().InternalSetMeleeFightWeakness(_WeaknessValue, _WeaknessSpeed, _TimeAfterWhichWeaknessDecrease);
    }

    private void InternalSetMeleeFightWeakness(float _WeaknessValue, float _WeaknessSpeed, float _TimeAfterWhichWeaknessDecrease)
    {
        m_MeleeFightDisplayer.SetWeakness((E_Direction)Random.Range(1, 5), _WeaknessValue, _WeaknessSpeed, _TimeAfterWhichWeaknessDecrease);
    }



    public static bool IsMeleeSlashHitting(Vector2 _StartPosition, Vector2 _EndPosition)
    {
        return GetInstance().InternalIsMeleeSlashHitting(_StartPosition, _EndPosition);
    }

    private bool InternalIsMeleeSlashHitting(Vector2 _StartPosition, Vector2 _EndPosition)
    {
        return m_MeleeFightDisplayer.IsSlashHitting(_StartPosition, _EndPosition);
    }



    public static void ActivateMagicalFormula(bool _MustBeActived)
    {
        GetInstance().InternalActivateMagicalFormula(_MustBeActived);
    }

    private void InternalActivateMagicalFormula(bool _MustBeActived)
    {
        m_MagicalFormulaDisplayer.gameObject.SetActive(_MustBeActived);
    }


    public static void StartMagicalFormulaDrawing(Vector2 _Position)
    {
        GetInstance().InternalStartMagicalFormulaDrawing(_Position);
    }

    private void InternalStartMagicalFormulaDrawing(Vector2 _Position)
    {
        m_MagicalFormulaDisplayer.StartDrawing(_Position);
    }



    public static void UpdateMagicalFormulaDrawing(Vector2 _Position)
    {
        GetInstance().InternalUpdateMagicalFormulaDrawing(_Position);
    }

    private void InternalUpdateMagicalFormulaDrawing(Vector2 _Position)
    {
        m_MagicalFormulaDisplayer.UpdateDrawing(_Position);
    }



    public static List<int> EndMagicalFormulaDrawing(Vector2 _Position)
    {
        return GetInstance().InternalEndMagicalFormulaDrawing(_Position);
    }

    private List<int> InternalEndMagicalFormulaDrawing(Vector2 _Position)
    {
        return m_MagicalFormulaDisplayer.EndDrawing(_Position);
    }



    public static void ActivateBellRing(bool _Value)
    {
        GetInstance().InternalActivateBellRing(_Value);
    }

    private void InternalActivateBellRing(bool _Value)
    {
        m_ShakeBellDisplayer.gameObject.SetActive(_Value);
    }



    public static void SetBellRing()
    {
        GetInstance().InternalSetBellRing();
    }

    private void InternalSetBellRing()
    {
        m_ShakeBellDisplayer.DisplayShakeNone();
    }



    public static void SetBellRingLeft()
    {
        GetInstance().InternalSetBellRingLeft();
    }

    private void InternalSetBellRingLeft()
    {
        m_ShakeBellDisplayer.DisplayShakeLeft();
    }



    public static void SetBellRingRight()
    {
        GetInstance().InternalSetBellRingRight();
    }

    private void InternalSetBellRingRight()
    {
        m_ShakeBellDisplayer.DisplayShakeRight();
    }
}
