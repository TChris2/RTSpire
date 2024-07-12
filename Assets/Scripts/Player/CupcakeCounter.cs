using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeCounter : MonoBehaviour
{
    public float cupcakeCount;
    [SerializeField]
    private float cupcakeMax = 1;
    private TMPro.TMP_Text CupcakeCounterText;
    
    void Start()
    {
        CupcakeCounterText = GetComponent<TMPro.TMP_Text>();
        cupcakeCount = cupcakeMax;
        CupcakeCounterText.text = $"{cupcakeMax}";
    }

    public void CounterDown() 
    {
        cupcakeCount -= 1;
        CupcakeCounterText.text = $"{cupcakeCount}";
    }

    public void CounterUp() 
    {
        cupcakeCount += 1;
        CupcakeCounterText.text = $"{cupcakeCount}";
    }
}
