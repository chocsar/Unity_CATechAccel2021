using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class ResultSceneView : MonoBehaviour
{
    [SerializeField] private Text resultText;
    [SerializeField] private Button retryButton;

    [SerializeField] private RectTransform rankElementPrefab;

    [SerializeField] private RectTransform scrollbarContent;


    // C#Action
    public event Action OnClickRetryButton;

    private void Start()
    {
        retryButton.onClick.AddListener(() => OnClickRetryButton?.Invoke());

        ScoreController.Instance.ScoreDataSort();
        SetRankElement(ScoreController.Instance.GetScoreCount());
    }

    /// <summary>
    /// ResultSceneのプレイヤースコアを表示させる
    /// </summary>
    public void SetResultText(int score)
    {
        resultText.text = score.ToString();
    }

    /// <summary>
    /// ランクを表示させるRankElementPrefabを生成させる関数
    /// </summary>
    private void SetRankElement(int rankCount)
    {
        for (int count = 0; count < rankCount; count++)
        {
            // Itemを生成 , Contentの子として登録  
            var rankElement = Instantiate(rankElementPrefab, scrollbarContent);
            RankElement element = rankElement.GetComponent<RankElement>();
            element.rankText.text = (count + 1).ToString();
            element.scoreText.text = ScoreController.Instance.GetScoreListValue(count).ToString();
        }
    }
}
