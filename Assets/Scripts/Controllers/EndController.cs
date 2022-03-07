using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{
    [SerializeField] HouseData houseData;
    [SerializeField] DialogHandler handler;
    private void Awake()
    {
        SceneManager.StartEndSequence += StartEndSequence;
        
    }

    private void OnDestroy()
    {
        SceneManager.StartEndSequence -= StartEndSequence;
    }

    private void StartEndSequence()
    {
        handler.Show(houseData.GetDialogForTime(TimeOfDay.Night));
        // When this is done, show (credits??)
    }

}
