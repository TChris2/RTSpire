using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleFrameSetter : MonoBehaviour
{
    private Renderer planeRenderer;
    float ranNum;
    void Start()
    {
        planeRenderer = GetComponent<Renderer>();
        do
        {
        ranNum = Random.Range(1, 400);
        } while (ranNum == 4 && ranNum == 5 && ranNum == 66 && ranNum == 78 && ranNum == 5 && ranNum == 81 && ranNum == 125 && ranNum == 143 
        && ranNum == 149 && ranNum == 150);
        Material mat = Resources.Load<Material>(ranNum.ToString());
        planeRenderer.material = mat;
    }
}
