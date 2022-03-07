using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameDisplayController : MonoBehaviour
{
    [SerializeField] GameObject textContainer;
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] TextMeshProUGUI interactionText;
    private void Awake()
    {
        BoundaryCollisionDetector.OnHouseTriggerEnter += OnHouseTriggerEnter;
        BoundaryCollisionDetector.OnHouseTriggerExit += OnHouseTriggerExit;
        SceneManager.OnHouseCameraChanged += OnHouseCameraChanged;
        PhoneboxBoundaryDetector.OnPhoneboxTriggerEnter += OnPhoneboxTriggerEnter;
        PhoneboxBoundaryDetector.OnPhoneboxTriggerExit += OnPhoneboxTriggerExit;
        CameraConfigs.HideUI += HideUI;
        CameraConfigs.ShowUI += ShowUI;
    }
    private void OnDestroy()
    {
        BoundaryCollisionDetector.OnHouseTriggerEnter -= OnHouseTriggerEnter;
        BoundaryCollisionDetector.OnHouseTriggerExit -= OnHouseTriggerExit;
        SceneManager.OnHouseCameraChanged -= OnHouseCameraChanged;
        PhoneboxBoundaryDetector.OnPhoneboxTriggerEnter -= OnPhoneboxTriggerEnter;
        PhoneboxBoundaryDetector.OnPhoneboxTriggerExit -= OnPhoneboxTriggerExit;
        CameraConfigs.HideUI -= HideUI;
        CameraConfigs.ShowUI -= ShowUI;
    }

    private void HideUI()
    {
        textContainer.SetActive(false);
    }

    private void ShowUI()
    {
        textContainer.SetActive(true);
    }
    private void OnHouseTriggerEnter(HouseData houseData)
    {
        textComponent.gameObject.SetActive(true);
        interactionText.gameObject.SetActive(true);
        textComponent.text = $"the {houseData.houseName.ToLower()} residence";
        interactionText.text = "press E to take a closer look";
    }

    private void OnHouseTriggerExit()
    {
        textComponent.text = "";
        textComponent.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(false);
        interactionText.text = "press E to get back in the car";
    }

    private void OnPhoneboxTriggerEnter()
    {
        textComponent.text = "old phonebooth";
        textComponent.gameObject.SetActive(true);
        interactionText.text = "press E to make a call";
        interactionText.gameObject.SetActive(true);
    }

    private void OnPhoneboxTriggerExit()
    {
        interactionText.text = "press E to take a closer look";
        textComponent.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(false);
    }

    private void OnHouseCameraChanged(CameraMode currentMode)
    {
        if(currentMode == CameraMode.Overworld)
        {
            interactionText.text = "press E to take a closer look";
        }
        else if(currentMode == CameraMode.Exterior)
        {
            interactionText.text = "press E to get back in the car";
        }
    }
}
