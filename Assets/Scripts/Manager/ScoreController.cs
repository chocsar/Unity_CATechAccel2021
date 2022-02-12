using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : SingletonMonoBehaviour<ScoreController>
{
    /// <summary>
    /// スコアの保存をする
    /// </summary>
    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt(Const.SaveKeyNames.Score.ToString(), score);
    }

    /// <summary>
    /// 保存されたスコアを取り出して値を返す
    /// </summary>
    public int LoadScore()
    {
        return PlayerPrefs.GetInt(Const.SaveKeyNames.Score.ToString(), 0);
    }
}
