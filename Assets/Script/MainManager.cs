using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static MainManager Instance;
    public Color TeamColor; // new variable declared

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadColor();
    }

    [System.Serializable] //per salvare con json
    class SaveData // per salvare i dati dell'utente
    {
        public Color TeamColor; //il colore che lo user seleziona
    }

    public void SaveColor() //metodo per salvare il dato
    {
        //creo un'istanza dei dati salvati e riempio la variabile team color
        SaveData data = new SaveData();
        data.TeamColor = TeamColor;

        // trasformo l'istanza in un json con l'utility, creo json e gli butto dentro data
        string json = JsonUtility.ToJson(data);

        // metodo per scrivere il file, con il path e la variabile da scrivere ovvero json
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadColor()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            TeamColor = data.TeamColor;
        }
    }
}
