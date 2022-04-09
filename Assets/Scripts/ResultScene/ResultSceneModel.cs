using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResultSceneModel : MonoBehaviour
{

    public event Action<string> OnChangeResultText;
    public event Action<int> OnChangeResultScore;
    public event Action<RankElement> OnSetRankElement;

    // Start is called before the first frame update
    void Start()
    {
        // スコアのデータをランキング順にソート
        ScoreController.Instance.ScoreDataSort();
        // リザルトのスコアを表示させるトリガーを発火
        SetResultScore(ScoreController.Instance.LoadScore());
        // ランクを表示させるプレハブの生成をさせるトリガーを発火
        SetRankElement(ScoreController.Instance.GetScoreCount());
    }

    /// <summary>
    /// ハイスコアの値セットとViewへのイベントを発火
    /// </summary>
    private void SetResultScore(int score)
    {
        OnChangeResultScore?.Invoke(score);
    }

    /// <summary>
    /// ランクを表示させるプレハブの生成をさせるトリガーを発火
    /// </summary>
    private void SetRankElement(int rankCount)
    {
        for (int count = 0; count < rankCount; count++)
        {
            RankElement element = new RankElement();
            element.rank = (count + 1).ToString();
            element.score = ScoreController.Instance.GetScoreListValue(count).ToString();
            OnSetRankElement?.Invoke(element);
        }
    }
}
