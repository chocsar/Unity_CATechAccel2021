using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMonoBehaviour<SceneController>
{
    /// <summary> /// シーンネームを列挙型で定義する /// </summary>
    public enum SceneNames
    {
        ResultScene,
        InGameScene,
        TitleScene,
    }

    public void LoadScene(SceneNames sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}
