using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitController : MonoBehaviour
{
    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
