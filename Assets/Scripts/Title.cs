using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{

    public void LoadInGameScene()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.InGameScene);
    }

}
