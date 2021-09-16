using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magic : MonoBehaviour, IMixedRealityTouchHandler, IMixedRealityInputHandler
{
    public GameObject target;
    public UnityEvent OtherFunctions;
    
    void Start()
    {
        Debug.Log("Started");
        // target = GameObject.Find("TestCuboMesh");
    }
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        Debug.Log("TouchStarted");
        target.transform.localScale = new Vector3(2, 2, 2);

    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        Debug.Log("OnTouchCompleted");
        target.transform.localScale = new Vector3(1, 1, 1);
    }
    public void OnTouchUpdated(HandTrackingInputEventData eventData)
    {
        Debug.Log("OnTouchUpdated");
    }
    public void OnInputUp(InputEventData eventData)
    {
        Debug.Log("OnInputUp");
        target.GetComponent<MeshRenderer>().material.color = Color.white;

    }

    public void OnInputDown(InputEventData eventData)
    {
        Debug.Log("OnInputDown");
        target.GetComponent<MeshRenderer>().material.color = Color.red;

    }

    public void TestMessage() {
        Debug.Log("Ciao");
    }
    

}
