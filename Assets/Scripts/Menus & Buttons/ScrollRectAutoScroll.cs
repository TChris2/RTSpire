using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

// Updates scrollrects with the currently selected object’s position
[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scrollSpeed = 10f;
    private bool mouseOver = false;

    private List<Selectable> m_Selectables = new List<Selectable>();
    private ScrollRect m_ScrollRect;

    private Vector2 m_NextScrollPosition = Vector2.up;

    public bool isMenuOpen;
    [SerializeField]
    private bool hasRows;    
    public int rowAmt;

    void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
    }

    public void Start()
    {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }
        ScrollToSelected(true);
    }

    public IEnumerator AutoScroll()
    {
        while (isMenuOpen)
        {
            // Scroll via input.
            InputScroll();
            if (!mouseOver)
            {
                // Lerp scrolling code.
                m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.unscaledDeltaTime);
            }
            else
            {
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            }

            yield return null;
        }
    }

    void InputScroll()
    {
        if (m_Selectables.Count > 0)
        {
            //remove the rePlayer getaxis calls is you aren't using Rewired
            //if it still doesn't work, check your input manager settings's axes and make sure they are defined properly
            //if you're using the new input system, this is also probably where you should replace the calls to the old one
            if (Input.GetAxis("Vertical") !=  0.0f || Input.GetAxis("Horizontal") != 0.0f || Input.GetButtonDown("Horizontal") 
                || Input.GetButtonDown("Vertical") || Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                mouseOver = false;
                ScrollToSelected(false);
            }
            else
                mouseOver = true;
        }
    }

    void ScrollToSelected(bool quickScroll)
    {
        int selectedIndex = -1;
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            Selectable selectedElement = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        
            if (selectedElement)
            {
                selectedIndex = m_Selectables.IndexOf(selectedElement);
            }
        }
        if (selectedIndex > -1)
        {
            if (!hasRows)
                ScrollNormal(quickScroll, selectedIndex);
            else
                ScrollRow(quickScroll, selectedIndex);
        }
    }

    void ScrollNormal(bool quickScroll, int selectedIndex)
    {
        if (quickScroll)
        {
            m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }
        else
        {
            m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
        }
    }

    void ScrollRow(bool quickScroll, int selectedIndex)
    {
        // Offset for header items
        int headerOffset = 1;

        // Adjusts index for header offset
        int gridIndex = Mathf.Max(0, selectedIndex - headerOffset);
        int gridCount = Mathf.Max(1, m_Selectables.Count - headerOffset);

        int rowIndex = gridIndex / rowAmt;
        int totalRows = Mathf.CeilToInt(gridCount / (float)rowAmt);

        float scrollPercent = 1f - (rowIndex / (float)(totalRows - 1));
        scrollPercent = Mathf.Clamp01(scrollPercent);

        if (quickScroll)
        {
            m_ScrollRect.normalizedPosition = new Vector2(0, scrollPercent);
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }
        else
        {
            m_NextScrollPosition = new Vector2(0, scrollPercent);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        ScrollToSelected(false);
    }
}