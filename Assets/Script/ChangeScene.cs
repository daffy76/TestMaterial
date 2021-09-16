using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void NuragheMaterialScene()
    {
        SceneManager.LoadScene("Test_NuragheMaterials");
    }
    public void ChiesaInterniScene()
    {
        SceneManager.LoadScene("Test_ChiesaInterni");
    }
    public void MapScene()
    {
        SceneManager.LoadScene("NURE");
    }



}