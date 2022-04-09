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
    public void GenerateRankElement(RankElement element)
    {
        // Itemを生成 , Contentの子として登録  
        var rankElement = Instantiate(rankElementPrefab, scrollbarContent);
        RankElement rankElementValue = rankElement.GetComponent<RankElement>();
        rankElementValue.rankText.text = element.rank;
        rankElementValue.scoreText.text = element.score;
    }
}
