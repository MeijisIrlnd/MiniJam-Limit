using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogType
{
    House, 
    Police
};

public class YellowPagesEntry : MonoBehaviour
{
    [SerializeField] public bool ignore = false;
    
    [SerializeField] public DialogType dialogType;
    [SerializeField] public HouseData linkedHouseData;
}
