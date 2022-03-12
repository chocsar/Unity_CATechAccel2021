using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameView : MonoBehaviour
{
    // 変数の宣言
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Cell[] cells;
    [SerializeField] private Button menuButton;

    // C# Action
    public event Action OnInputRight;
    public event Action OnInputLeft;
    public event Action OnInputUp;
    public event Action OnInputDown;
    public event Action OnClickMenuButton;

    IInputable input;

    private void Start()
    {
         menuButton.onClick.AddListener(() => OnClickMenuButton?.Invoke());

        // もしIOSまたはandroidなら
        #if UNITY_IOS || UNITY_ANDROID
        //input = new SmartphoneInput();
        // それ以外なら
        #else
        input = new PcInput();
        #endif
    }

    private void Update()
    {
        if (input.GetRightInput())
        {
            OnInputRight?.Invoke();
        }
        else if (input.GetLeftInput())
        {
            OnInputLeft?.Invoke();
        }
        else if (input.GetUpInput())
        {
            OnInputUp?.Invoke();
        }
        else if (input.GetDownInput())
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
