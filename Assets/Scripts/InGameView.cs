using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameView : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Cell[] cells;
    public event Action MoveCellsRight;
    public event Action MoveCellsLeft;
    public event Action MoveCellsUp;
    public event Action MoveCellsDown;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCellsRight?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCellsLeft?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCellsUp?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCellsDown?.Invoke();
        }

    }

    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
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
