using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    //This code enables you to access the MainManager object from any other script.  
    public static MainManager Instance;
    //This is the static class member declaration.Note the keyword static after the keyword public. This keyword means that the values stored in this class member will be shared by all the instances of that class. 

    //Awake method, is called as soon as the object is created
    private void Awake()
    {
        //  conditional statement to check whether or not Instance is null. The very first time that you launch the Menu scene, no MainManager will have filled the Instance variable. This means it will be null, so the condition will not be met, and the script will continue as you previously wrote it.
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        // stores “this” in the class member Instance — the current instance of MainManager. You can now call MainManager.Instance from any other script
        DontDestroyOnLoad(gameObject);
    }

    public string UserName = "Davide"; // new variable declared




     //per salvare con json
    // per salvare i dati dell'utente
    
//lo user che lo user seleziona
    

        //metodo per salvare il dato
    
        //creo un'istanza dei dati salvati e riempio la variabile team color

        // trasformo l'istanza in un json con l'utility, creo json e gli butto dentro data

        // metodo per scrivere il file, con il path e la variabile da scrivere ovvero json

}
