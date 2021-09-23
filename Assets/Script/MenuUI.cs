using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]


public class MenuUI : MonoBehaviour
{
  /*   public void HomeScene()
    {
        SceneManager.LoadScene(0);
    }
   public void ChiesaInterniScene()
    {
        //SceneManager.LoadScene("Test_ChiesaInterni");
        SceneManager.LoadScene(1);
    }
    public void NuragheMaterialScene()
    {
        //SceneManager.LoadScene("Test_NuragheMaterials");
        SceneManager.LoadScene(2);
    }
    public void MapScene()
    {
        //SceneManager.LoadScene("NURE");
        SceneManager.LoadScene(3);
    }
 */
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif    
    }
    public async void TransitionToScene(string scene)
    {
        IMixedRealitySceneSystem sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
        ISceneTransitionService transition = MixedRealityToolkit.Instance.GetService<ISceneTransitionService>();

        // Fades out
        // Runs LoadContent task
        // Fades back in
 
 //       await sceneSystem.UnloadContent("InternoChiesa");
 //       await sceneSystem.UnloadContent("Mappa");
 //       await sceneSystem.UnloadContent("NuragheMaterial");

        await transition.DoSceneTransition(
               () => sceneSystem.LoadContent(scene, LoadSceneMode.Single)
           ); 
    }


}

