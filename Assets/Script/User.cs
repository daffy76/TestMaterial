using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        Debug.Log(userName);
        //Debug.Log("Claims" + System.Security.Principal.WindowsIdentity.GetCurrent().Claims.ToString());
        //Debug.Log("Token:" + System.Security.Principal.WindowsIdentity.GetCurrent().Token.ToString());
        //Debug.Log("AccessToken" + System.Security.Principal.WindowsIdentity.GetCurrent().AccessToken.ToString());
        var accountToken = System.Security.Principal.WindowsIdentity.GetCurrent().Token;
        var windowsIdentity = new System.Security.Principal.WindowsIdentity(accountToken);
        Debug.Log(windowsIdentity.Name);
        Debug.Log(windowsIdentity.User);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
