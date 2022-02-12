using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMonoBehaviour<SceneController>
{
    void Awake()
    {
        // 子クラスでAwakeを使う場合は
        // 必ず親クラスのAwakeをCallして
        // 複数のGameObjectにアタッチされないようにします.
        base.Awake();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
