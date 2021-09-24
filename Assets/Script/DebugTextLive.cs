using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTextLive : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    //TextMeshPro textMesh2;

    // Use this for initialization
    void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //textMesh2 = gameObject.GetComponent<TextMeshPro>();
        //textMesh2.text = "Ciao";

        var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        Debug.Log(userName);


    }


void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    public void LogMessage(string message, string stackTrace, LogType type)
    {
        if (textMesh.text.Length > 300)
        {
            textMesh.text = message + "\n";
        }
        else
        {
            textMesh.text += message + "\n";
        }
    }
}