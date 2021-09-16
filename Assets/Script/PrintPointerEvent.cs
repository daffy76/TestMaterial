using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintPointerEvent : MonoBehaviour, IMixedRealityPointerHandler
{
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if (eventData.Pointer is SpherePointer)
        {
            Debug.Log($"Grab start from {eventData.Pointer.PointerName}");
        }
        if (eventData.Pointer is PokePointer)
        {
            Debug.Log($"Touch start from {eventData.Pointer.PointerName}");
        }
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData) {
        Debug.Log("OnPointerClicked");

    }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) {
        Debug.Log("OnPointerDragged");

    }
    public void OnPointerUp(MixedRealityPointerEventData eventData) {
        Debug.Log("OnPointerUp");

    }
}