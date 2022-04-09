﻿using UnityEngine;
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

        for(int i = 0; i < 10; i++)
        {
            // Itemを生成 , Contentの子として登録  
            var item = Instantiate(rankElementPrefab, scrollbarContent);

        }


    }

    public void SetResultText(int score)
    {
        resultText.text = score.ToString();
    }

}