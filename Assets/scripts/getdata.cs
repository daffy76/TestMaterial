using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class Cube
{
    public int id;
    public string name;
    public string color;
    public int x;
    public int y;
    public int z;
}

public class DataCollection
{
    public List<Cube> items;
}


public class getdata : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetComments());
    }

     // Update is called once per frame
    void Update()
    {
        
    }



    public IEnumerator GetComments() {
        UnityWebRequest www = UnityWebRequest.Get("https://run.mocky.io/v3/21a323ec-0db5-4bd0-9edb-dd815340cc15");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.Log(www.error);
        } else {            
            DataCollection dataCollection = JsonUtility.FromJson<DataCollection>(www.downloadHandler.text);
            
            dataCollection.items.ForEach(item => {

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = item.name;
                cube.transform.localPosition = new Vector3(item.x,item.y,item.z);

                //Get the Renderer component from the new cube
                var cubeRenderer = cube.GetComponent<Renderer>();
               

                //Call SetColor using the shader property name "_Color" and setting the color to red

                if (item.color == "red")
                {
                    cube.GetComponent<Renderer>().material.color = Color.red;
                }

                if (item.color == "blue")
                {
                    cube.GetComponent<Renderer>().material.color = Color.blue;

                }

                if (item.color == "yellow")
                {
                    cube.GetComponent<Renderer>().material.color = Color.yellow;
                }

               
            });
            
        }
    }

}
