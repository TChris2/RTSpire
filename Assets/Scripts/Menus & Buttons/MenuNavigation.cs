using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sets ups keyboard/controllor navigation
public class MenuNavigation : MonoBehaviour
{
    [SerializeField]
    private Button[] TopBarBtns;
    void Start()
    {
        UpdateContentNav();
    }

    public void UpdateContentNav()
    {
        // Get all active Selectables under the content
        Selectable[] selectables = GetComponentsInChildren<Selectable>();

        for (int i = 0; i < selectables.Length; i++)
        {
            Navigation nav = selectables[i].navigation;
            nav.mode = Navigation.Mode.Explicit;

            // Set selectOnDown (next)
            if (i < selectables.Length - 1)
                nav.selectOnDown = selectables[i + 1];

            // Set selectOnUp (previous)
            if (i > 0)
                nav.selectOnUp = selectables[i - 1];

            selectables[i].navigation = nav;
        }
    }

    public void UpdateTypeMenuNav(int rowAmt)
    {
        // Get all active Selectables under the content
        Selectable[] selectables = GetComponentsInChildren<Selectable>();
        
        // Set up navigation for the first content item
        Navigation nav = selectables[0].navigation;
        nav.selectOnDown = selectables[1];
        selectables[0].navigation = nav;

        for (int i = 1; i < rowAmt + 1; i++)
        {
            nav = selectables[i].navigation;
            nav.mode = Navigation.Mode.Explicit;

            // Set selectOnDown (next)
            if (i < selectables.Length - rowAmt)
                nav.selectOnDown = selectables[i + rowAmt];

            // Set selectOnUp (previous)
            if (i > 0)
                nav.selectOnUp = selectables[0];

            selectables[i].navigation = nav;
        }

        for (int i = rowAmt + 1; i < selectables.Length; i++)
        {
            nav = selectables[i].navigation;
            nav.mode = Navigation.Mode.Explicit;

            // Set selectOnDown (next)
            if (i < selectables.Length - rowAmt)
                nav.selectOnDown = selectables[i + rowAmt];

            // Set selectOnUp (previous)
            if (i > 0)
                nav.selectOnUp = selectables[i - rowAmt];

            selectables[i].navigation = nav;
        }
    }

    // Updates navigation to top bar and for it
    public void UpdateTopBarNavigation()
    {
        Selectable selectable = GetComponentInChildren<Selectable>();

        // Updates navigation to top bar buttons
        for (int i = 0; i < TopBarBtns.Length; i++)
        {
            Navigation topBarBtnNav = TopBarBtns[i].navigation;
            topBarBtnNav.selectOnDown = selectable;
            TopBarBtns[i].navigation = topBarBtnNav;
        }

        // Set up navigation to top bar for the first content item
        Navigation contentNav = selectable.navigation;
        contentNav.selectOnUp = TopBarBtns[0];
        selectable.navigation = contentNav;
    }
}
