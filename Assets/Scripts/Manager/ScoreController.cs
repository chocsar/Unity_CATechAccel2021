using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : SingletonMonoBehaviour<ScoreController>
{
    /// <summary> /// PlayerPrefsで保存するキーの名前を列挙型で定義 /// </summary>
    public enum SaveKeyNames
    {
        Score,
        HighScore
    }
    /// <summary>
    /// スコアの保存をする
    /// </summary>
    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt(SaveKeyNames.Score.ToString(), score);
    }

    /// <summary>
    /// 保存されたスコアを取り出して値を返す
    /// </summary>
    public int LoadScore()
    {
        return PlayerPrefs.GetInt(SaveKeyNames.Score.ToString(), 0);
    }

    /// <summary>
    /// スコアの更新があれば保存をする
    /// </summary>
    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(SaveKeyNames.HighScore.ToString(), score);
    }

    /// <summary>
    /// 保存されたハイスコアを取り出して値を返す
    /// </summary>
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(SaveKeyNames.HighScore.ToString(), 0);
    }

    /// <summary>
    ///  ハイスコアが更新されるかの確認してハイスコアの値を返す
    /// </summary>
    public void CheckHighScore(int score)
    {
        if(score > GetHighScore())
        {
            SaveHighScore(score);
        }
    }
}
