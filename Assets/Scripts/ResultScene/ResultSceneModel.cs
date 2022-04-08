using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResultSceneModel : MonoBehaviour
{

    public event Action<string> OnChangeResultText;

    // Start is called before the first frame update
    void Start()
    {
        SetResultText(ScoreController.Instance.LoadScore().ToString());
    }

    /// <summary> 
    /// 
    /// </summary>
    public void SetResultText(string resultText)
    {
        Debug.Log(resultText);
        OnChangeResultText?.Invoke(resultText);
    }
}
