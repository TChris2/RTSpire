using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keeps track on how many cupcakes the player has atm
public class CupcakeCounter : MonoBehaviour
{
    // Current amt of cupcakes the player has
    public float cupcakeCount;
    // The max the player can throw out
    [SerializeField]
    private float cupcakeMax = 1;
    // Text
    private TMPro.TMP_Text CupcakeCounterText;
    
    void Start()
    {
        // Gets text
        CupcakeCounterText = GetComponent<TMPro.TMP_Text>();
        // Sets max amt of cupcakes
        cupcakeCount = cupcakeMax;
        CupcakeCounterText.text = $"{cupcakeMax}";
    }

    // Decreases the counter
    public void CounterDown() 
    {
        cupcakeCount -= 1;
        CupcakeCounterText.text = $"{cupcakeCount}";
    }

    // Increases the counter
    public void CounterUp() 
    {
        cupcakeCount += 1;
        CupcakeCounterText.text = $"{cupcakeCount}";
    }
}
