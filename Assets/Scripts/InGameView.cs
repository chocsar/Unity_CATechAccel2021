using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameView : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }



}
