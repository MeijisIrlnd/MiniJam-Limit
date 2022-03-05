using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    void OnMouseDown()
    {
        // Swap this with a raycast.. 
        // Except what triggers the raycast?? 
        Debug.Log("Window Clicked!");
    }
}
