using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : SingletonMonoBehaviour<ScoreController>
{
    void Awake()
    {
        // 子クラスでAwakeを使う場合は
        // 必ず親クラスのAwakeをCallして
        // 複数のGameObjectにアタッチされないようにします.
        base.Awake();
    }

    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt("SCORE", score);
    }

    public int LoadScore()
    {
        return PlayerPrefs.GetInt("SCORE", 0);
    }
}
