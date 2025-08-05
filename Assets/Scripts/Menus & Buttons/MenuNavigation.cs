using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Sets up keyboard/controller navigation for menus
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

        // Sets up navigation to top element for the first main row
        for (int i = 1; i < rowAmt + 1; i++)
        {
            nav = selectables[i].navigation;
            nav.mode = Navigation.Mode.Explicit;

            // Goes down a row
            if (i < selectables.Length - rowAmt)
                nav.selectOnDown = selectables[i + rowAmt];

            // Goes to the item left of it
            if (i != 1)
                nav.selectOnLeft = selectables[i - 1];
            
            if (i + 1 != selectables.Length)
                nav.selectOnRight = selectables[i + 1];
            
            // Goes to the item right of it
            if (i < selectables.Length - rowAmt)
                nav.selectOnDown = selectables[i + rowAmt];

            // Goes up a row to the first row
            if (i > 0)
                nav.selectOnUp = selectables[0];

            selectables[i].navigation = nav;
        }

        // General nav
        for (int i = rowAmt + 1; i < selectables.Length; i++)
        {
            nav = selectables[i].navigation;
            nav.mode = Navigation.Mode.Explicit;

            // Goes down a row
            if (i < selectables.Length - rowAmt)
                nav.selectOnDown = selectables[i + rowAmt];

            // Goes to the item left of it
            nav.selectOnLeft = selectables[i - 1];
                
            if (i + 1 != selectables.Length)
                nav.selectOnRight = selectables[i + 1];
                
            // Goes to the item right of it
            if (i < selectables.Length - rowAmt)
                nav.selectOnDown = selectables[i + rowAmt];

            // Goes up a row to the first row
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
