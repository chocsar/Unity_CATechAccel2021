using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResultSceneModel : MonoBehaviour
{

    public event Action<string> OnChangeResultText;
    public event Action<int> OnChangeHighScore;

    // Start is called before the first frame update
    void Start()
    {
        SetHighScore(ScoreController.Instance.LoadScore());
    }

    /// <summary>
    /// ハイスコアの値セットとViewへのイベントを発火
    /// </summary>
    public void SetHighScore(int score)
    {
        OnChangeHighScore?.Invoke(score);
    }
}
