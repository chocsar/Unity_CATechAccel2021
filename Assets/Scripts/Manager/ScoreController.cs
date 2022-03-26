using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreController : SingletonMonoBehaviour<ScoreController>
{
    // ScoreDataに関するJsonのデータ構造を定義
    [System.Serializable]
    private struct ScoreDatas
    {
        public int highScore;
        public List<Score> scoreDatas;
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
    string inputString;

    private void Start()
    {
        LoadScoreJson();
    }

    /// <summary>
    /// スコアの保存をする
    /// </summary>
    public void SaveScore(int score)
    {
        gameScore = score;
        SaveScoreData(score);
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
        scoreData.highScore = score;
        SaveScoreJson();
    }

    /// <summary>
    /// 保存されたハイスコアを取り出して値を返す
    /// </summary>
    public int GetHighScore()
    {
        LoadScoreJson();
        return scoreData.highScore;
    }

    /// <summary>
    /// スコアのデータを新しく保存する
    /// </summary>
    private void SaveScoreData(int score)
    {
        scoreData.scoreDatas.Add(new Score { recordScore = score});
        SaveScoreJson();
    }

    /// <summary>
    /// JSONファイルへデータを新しく保存する
    /// </summary>
    private void SaveScoreJson()
    {
        StreamWriter writer = new StreamWriter(Const.jsonDataPath, false);
        writer.WriteLine(JsonUtility.ToJson(scoreData, true));
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    /// 保存されたハイスコアを取り出して値を返す
    /// </summary>
    private void LoadScoreJson()
    {
        // RankingData.jsonをテキストファイルとして読み取り、string型で受け取る
        inputString = Resources.Load<TextAsset>("RankingData").ToString();
        // 上で作成したクラスへデシリアライズ
        scoreData = JsonUtility.FromJson<ScoreDatas>(inputString);
        // ScoreDataのListへJSONのデータを代入
        for (int scoreCount = 0; scoreCount < scoreData.scoreDatas.Count; scoreCount++)
        {
            ScoreData.Add(scoreData.scoreDatas[scoreCount].recordScore);
        }
    }
}
