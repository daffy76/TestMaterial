using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void NuragheMaterialScene()
    {
        SceneManager.LoadScene("Test_NuragheMaterials");
        SceneManager.LoadScene(1);
    }
    public void ChiesaInterniScene()
    {
        //SceneManager.LoadScene("Test_ChiesaInterni");
        SceneManager.LoadScene(2);
    }
    public void MapScene()
    {
        //SceneManager.LoadScene("NURE");
        SceneManager.LoadScene(0);
    }



}