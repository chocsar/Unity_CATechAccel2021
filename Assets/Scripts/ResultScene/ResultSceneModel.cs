using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResultSceneModel : MonoBehaviour
{

    public event Action<string> OnChangeResultText;
    public event Action<int> OnChangeHighScore;
    public event Action<RankElement> OnSetRankElement;

    // Start is called before the first frame update
    void Start()
    {
        SetHighScore(ScoreController.Instance.LoadScore());

        ScoreController.Instance.ScoreDataSort();
        SetRankElement(ScoreController.Instance.GetScoreCount());
    }

    /// <summary>
    /// ハイスコアの値セットとViewへのイベントを発火
    /// </summary>
    public void SetHighScore(int score)
    {
        OnChangeHighScore?.Invoke(score);
    }

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
