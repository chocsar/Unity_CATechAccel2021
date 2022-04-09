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

    /// <summary> ScoreDataを保存するList </summary>
    public List<int> scoreDataList;
    /// <summary> ゲームのスコアの保存をする変数 </summary>
    private int gameScore;
    /// <summary> スコアのデータを保存するJsonのデータ構造 </summary>
    ScoreDatas scoreData;
    /// <summary> スコアのデータを保存するJsonのデータパスを代入 </summary>
    string dataPath;

    private void Start()
    {
        //プラットフォームに応じてJSONデータソースを切り替える
#if UNITY_ANDROID
        dataPath = "jar:file://" + Application.persistentDataPath + "!/assets" + Const.jsonFileName;
#else
        dataPath = Application.persistentDataPath + Const.jsonFileName;
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
        if (!File.Exists(dataPath) )
        {
            File.Copy(Const.jsonDataPath, dataPath);
        }

        // RankingData.jsonをテキストファイルとして読み取り、string型で受け取る
        StreamReader reader = new StreamReader(dataPath);
        string inputString = reader.ReadToEnd();
        // 上で作成したクラスへデシリアライズ
        scoreData = JsonUtility.FromJson<ScoreDatas>(inputString);
        // ScoreDataのListへJSONのデータを代入
        for (int scoreCount = 0; scoreCount < scoreData.scoreDatas.Count; scoreCount++)
        {
            scoreDataList.Add(scoreData.scoreDatas[scoreCount].recordScore);
        }

        reader.Close();
    }

    /// <summary>
    /// スコアデータリストのデータ数を返す変数
    /// </summary>
    public int GetScoreCount()
    {
        return scoreDataList.Count;
    }

    /// <summary>
    ///  スコアのデータリストをソートする
    /// </summary>
    public void ScoreDataSort()
    {
        scoreDataList.Sort();
        scoreDataList.Reverse();
    }

    /// <summary>
    /// ScoreListの値を返す
    /// </summary>
    public int GetScoreListValue(int rank)
    {
        return scoreDataList[rank];
    }
}
