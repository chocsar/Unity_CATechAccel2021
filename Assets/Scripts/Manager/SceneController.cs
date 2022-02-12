using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMonoBehaviour<SceneController>
{
    public void LoadScene(Const.SceneNames sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}
