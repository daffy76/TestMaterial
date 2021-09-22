using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]


public class MenuUI : MonoBehaviour
{
    public void NuragheMaterialScene()
    {
        //SceneManager.LoadScene("Test_NuragheMaterials");
        SceneManager.LoadScene(2);
    }
    public void ChiesaInterniScene()
    {
        //SceneManager.LoadScene("Test_ChiesaInterni");
        SceneManager.LoadScene(3);
    }
    public void MapScene()
    {
        //SceneManager.LoadScene("NURE");
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif    
    }
}

