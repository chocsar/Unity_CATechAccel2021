using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameView : MonoBehaviour
{
    // 変数の宣言
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Cell[] cells;

    // C# Action
    public event Action OnInputRight;
    public event Action OnInputLeft;
    public event Action OnInputUp;
    public event Action OnInputDown;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnInputRight?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnInputLeft?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnInputUp?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnInputDown?.Invoke();
        }

    }

    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void SetHighScore(int score)
    {
        highScoreText.text = $"Score: {score}";
    }

    /// <summary>
    /// ステージの初期状態をViewに反映
    /// </summary>
    public void ApplyStageView(int[,] stageState)
    {
        for (var row = 0; row < Const.SquareSize; row++)
        {
            for (var col = 0; col < Const.SquareSize; col++)
            {
                cells[row * Const.SquareSize + col].SetText(stageState[row, col]);
            }
        }
    }

}
