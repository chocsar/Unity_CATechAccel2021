using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : SingletonMonoBehaviour<ScoreController>
{
    // ScoreDataに関するJsonのデータ構造を定義
    [System.Serializable]
    private struct ScoreDatas
    {
        public int highScore;
        public Score[] scoreDatas;
    }
    [System.Serializable]
    private struct Score
    {
        public int recordScore;
    }

    // ScoreDataを保存するList
    public List<int> ScoreData;
    private int gameScore;

    ScoreDatas scoreData;

    private void Start()
    {
        // RankingData.jsonをテキストファイルとして読み取り、string型で受け取る
        string inputString = Resources.Load<TextAsset>("RankingData").ToString();
        // 上で作成したクラスへデシリアライズ
        scoreData = JsonUtility.FromJson<ScoreDatas>(inputString);
        // ScoreDataのListへJSONのデータを代入
        for (int scoreCount = 0;scoreCount < scoreData.scoreDatas.Length; scoreCount++)
        {
            ScoreData.Add(scoreData.scoreDatas[scoreCount].recordScore);
        }
    }

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
        //PlayerPrefs.SetInt(SaveKeyNames.Score.ToString(), score);
        ScoreData.Add(score);
        gameScore = score;
    }

    /// <summary>
    /// 保存されたスコアを取り出して値を返す
    /// </summary>
    public int LoadScore()
    {
        return gameScore;
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
        return scoreData.highScore;
    }
}
