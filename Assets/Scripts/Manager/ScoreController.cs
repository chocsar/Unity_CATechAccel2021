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

    string dataPath;

    private void Start()
    {
        //プラットフォームに応じてJSONデータソースを切り替える
#if UNITY_IOS
        dataPath = Application.dataPath + "/Raw";
#elif UNITY_ANDROID
        dataPath = "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_EDITOR
        dataPath = Const.jsonDataPath;
#else
        dataPath = Application.dataPath + "/StreamingAssets";
#endif
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
        StreamWriter writer = new StreamWriter(dataPath, false);
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
        StreamReader reader = new StreamReader(dataPath);
        inputString = reader.ReadToEnd();
        // 上で作成したクラスへデシリアライズ
        scoreData = JsonUtility.FromJson<ScoreDatas>(inputString);
        // ScoreDataのListへJSONのデータを代入
        for (int scoreCount = 0; scoreCount < scoreData.scoreDatas.Count; scoreCount++)
        {
            ScoreData.Add(scoreData.scoreDatas[scoreCount].recordScore);
        }

        reader.Close();
    }
}
