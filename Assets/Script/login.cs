using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;
using DataEntities;

public class login : MonoBehaviour
{

    private string URL = "http://hololens.dairy-tools.com";
    public UserConnect userConnected = new UserConnect();
    

    // Start is called before the first frame update
    void Start()
    {

        // Username
        GameObject username = GameObject.Find("username");
        InputField inputUsername = username.GetComponent<InputField>();

        // Password
        GameObject password = GameObject.Find("password");
        InputField inputPassword = password.GetComponent<InputField>();

        // Submit
        GameObject submit = GameObject.Find("submit");
        Button btn_submit = submit.GetComponent<Button>();
        btn_submit.onClick.AddListener(delegate () {
            StartCoroutine(this.Login("gelso", "hololens"));
        });


        if (File.Exists(Application.persistentDataPath + "/user.json"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/user.json");

            this.userConnected = JsonConvert.DeserializeObject<UserConnect>(reader.ReadToEnd()); //MPPPER

            reader.Close();

            this.showMenuByRole();

        }
         
    }

    // Update is called once per frame
    void Update()
    {   
        
    }

    private IEnumerator Login(string username, string password)
    {
        string url = this.URL + "/api/login.php" + "?username=" + username + "&pwd=" + password;

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
        
            JsonUtility.FromJsonOverwrite(www.downloadHandler.text, this.userConnected); //MAPPER

            Debug.Log("last connection" + this.userConnected.Datelastconnection);

            if (userConnected.sessionid != "error")
             {

                Debug.Log("Login OK: " +  this.userConnected.sessionid);
                this.saveSessionData();

                this.showMenuByRole();
            }
            else
            {
                Debug.Log("Login FAILS: ");
                this.showErrorMessage();
            }
 
        } 
    }

    private void showMenuByRole()
    {
        switch (this.userConnected.role)
        {
            case "admin":
                Debug.Log("show menu admin");
                break;
            case "veterinarian":
                Debug.Log("show menu veterinarian");
                break;
            default:
                Debug.Log("role" + this.userConnected.role + " is not handle");
            break;
        }

        InvokeRepeating("isAlive", 10, 10);

    }

    private void saveSessionData()
    {
        string path = Application.persistentDataPath + "/user.json";

        StreamWriter writer = new StreamWriter(path, false);

        writer.Write(JsonUtility.ToJson(this.userConnected, true));

        writer.Close();
    }


    private void showErrorMessage()
    {
        Debug.Log("username or password are wronge !");
    }


    private void isAlive()
    {
        StartCoroutine(this.isSessionExpired((bool isAlive) => {

            Debug.Log("isAlive: - setinterval " + isAlive);

            if (!isAlive)
            {
                Debug.Log("session Expired");

                //SHOW LOGIN
            }
        }));

    }


    private IEnumerator  isSessionExpired(System.Action<bool> callback)
    {

        Debug.Log(this.userConnected.sessionid);

        string url = this.URL + "/api/isalive.php?sessionid=" + this.userConnected.sessionid;

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
        
            if (www.downloadHandler.text.IndexOf("ALIVE") > 0)
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
        }

    }


    private IEnumerator logout(System.Action<bool> callback)
    {
        string url = this.URL + "/api/logout.php?sessionid=" + this.userConnected.sessionid;

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            this.userConnected = null;

            //SHOW LOGIN
        }
    }


    //TODO 
    // Delete file 
    // User Singleton > https://www.studica.com/blog/how-to-create-a-singleton-in-unity-3d
    // UNITY MAKE IT WORKS 
    // MONTARE IL TUTTO
    // AGGIUNGERE JSON FATTORIE associate.
    //SHOW LOGIN
}