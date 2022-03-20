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

    private IInputable input;

    private void Start()
    {
         menuButton.onClick.AddListener(() => OnClickMenuButton?.Invoke());
        //プラットフォームに応じて入力クラスを切り替える
#if UNITY_IOS || UNITY_ANDROID
        input = new SmartphoneInput();
#elif UNITY_EDITOR
        input = new PcInput();
#endif
    }

    private void Update()
    {
        switch (input.GetDirection())
        {
            // セルの右移動処理が実行された場合
            case Const.InputDirection.Right:
                OnInputRight?.Invoke();
                break;
            // セルの左移動処理が実行された場合 
            case Const.InputDirection.Left:
                OnInputLeft?.Invoke();
                break;
            // セルの上移動処理が実行された場合 
            case Const.InputDirection.Up:
                OnInputUp?.Invoke();
                break;
            // セルの下移動処理が実行された場合 
            case Const.InputDirection.Down:
                OnInputDown?.Invoke();
                break;
            default:
                break;
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
