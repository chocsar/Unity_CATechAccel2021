using UnityEngine;
using System;

public class InGameModel : MonoBehaviour
{
    // 変数の宣言
    private int score;
    private int highScore;
    /// <summary> 生成するCellの値を入れた配列 </summary>
    private int[] generateCellNumbers = new int[2] { 2, 4 };
    private readonly int[,] stageState = new int[Const.SquareSize, Const.SquareSize];
    /// <summary> 盤面の再描画を行う必要があるかのフラグ </summary>
    private bool isDirty;

    // C# Action
    public event Action<int> OnChangeScore;
    public event Action<int[,]> OnChangeStageState;
    public event Action OnGameOver;
    public event Action<int> OnChangeHighScore;

    // <summary>
    /// ゲームの初期状態を生成
    /// </summary>
    public void Initialize()
    {
        // ステージの初期状態を生成
        InitializeStage();
        // ステージの初期状態をViewに反映
        OnChangeStageState?.Invoke(stageState);
    }

    /// <summary>
    /// ステージの境界線をチェックして処理の継続をするか判定する
    /// </summary>
    private bool CheckBorder(int row, int col, int horizontal, int vertical)
    {
        // チェックマスが4x4外ならそれ以上処理を行わない
        if (row < 0 || row >= Const.SquareSize || col < 0 || col >= Const.SquareSize)
        {
            return false;
        }

        // 移動先が4x4外ならそれ以上処理は行わない
        var nextRow = row + vertical;
        var nextCol = col + horizontal;
        if (nextRow < 0 || nextRow >= Const.SquareSize || nextCol < 0 || nextCol >= Const.SquareSize)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// セル自体が入力された矢印キーの移動方向へ移動できるか確認
    /// </summary>
    private bool CheckCell(int row, int col, int horizontal, int vertical)
    {
        // 4x4の境界線チェック
        if (CheckBorder(row, col, horizontal, vertical) == false)
        {
            return false;
        }
        // 空欄マスは移動処理をしない
        if (stageState[row, col] == 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// セル自体が入力された矢印キーの移動方向へ移動させる
    /// </summary>
    private void MoveCell(int row, int col, int horizontal, int vertical)
    {
        // 4x4境界線チェック
        // 再起呼び出し以降も毎回境界線チェックはするため冒頭で呼び出しておく
        if (CheckBorder(row, col, horizontal, vertical) == false)
        {
            return;
        }
        // 移動先の位置を計算
        var nextRow = row + vertical;
        var nextCol = col + horizontal;

        // 移動元と移動先の値を取得
        var value = stageState[row, col];
        var nextValue = stageState[nextRow, nextCol];

        // 次の移動先のマスが0の場合は移動する
        if (nextValue == 0)
        {
            // 移動元のマスは空欄になるので0を埋める
            stageState[row, col] = 0;

            // 移動先のマスに移動元のマスの値を代入する
            stageState[nextRow, nextCol] = value;

            // 移動先のマスでさらに移動チェック
            MoveCell(nextRow, nextCol, horizontal, vertical);
        }
        // 同じ値のときは合成処理
        else if (value == nextValue)
        {
            MergeCell(row, col, nextRow, nextCol, value);
        }
        // 異なる値のときは移動処理を終了
        else if (value != nextValue)
        {
            return;
        }
        isDirty = true;
    }

    /// <summary>
    /// セルの合成処理
    /// </summary>
    private void MergeCell(int row, int col, int nextRow, int nextCol, int value)
    {
        stageState[row, col] = 0;
        stageState[nextRow, nextCol] = value * 2;
        SetScore(value);
    }

    /// <summary>
    /// 新しい「2,4」のセルを空いたセルへ生成させる
    /// </summary>
    private void CreateNewRandomCell()
    {
        // ゲーム終了時はスポーンしない
        if (IsGameOver(stageState))
        {
            return;
        }
        var row = UnityEngine.Random.Range(0, Const.SquareSize);
        var col = UnityEngine.Random.Range(0, Const.SquareSize);
        while (stageState[row, col] != 0)
        {
            row = UnityEngine.Random.Range(0, Const.SquareSize);
            col = UnityEngine.Random.Range(0, Const.SquareSize);
        }

        stageState[row, col] = UnityEngine.Random.Range(0, 1f) < Const.ProbabilityOfSelectGeneratingCell ? generateCellNumbers[0] : generateCellNumbers[1];
    }

    /// <summary>
    /// ゲームオーバー状態か判定する
    /// </summary>
    private bool IsGameOver(int[,] stageState)
    {
        // 空いている場所があればゲームオーバーにはならない
        for (var i = 0; i < stageState.GetLength(0); i++)
        {
            for (var j = 0; j < stageState.GetLength(1); j++)
            {
                if (stageState[i, j] <= 0)
                {
                    return false;
                }
            }
        }

        // 合成可能なマスが一つでもあればゲームオーバーにはならない
        for (var row = 0; row < stageState.GetLength(0); row++)
        {
            for (var col = 0; col < stageState.GetLength(1); col++)
            {
                var state = stageState[row, col];
                var canMerge = false;
                if (row > 0)
                {
                    canMerge |= state == stageState[row - 1, col];
                }

                if (row < stageState.GetLength(0) - 1)
                {
                    canMerge |= state == stageState[row + 1, col];
                }

                if (col > 0)
                {
                    canMerge |= state == stageState[row, col - 1];
                }

                if (col < stageState.GetLength(1) - 1)
                {
                    canMerge |= state == stageState[row, col + 1];
                }

                if (canMerge)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// ステージの初期状態を生成
    /// </summary>
    private void InitializeStage()
    {
        for (var row = 0; row < Const.SquareSize; row++)
        {
            for (var col = 0; col < Const.SquareSize; col++)
            {
                stageState[row, col] = 0;
            }
        }
        var posA = new Vector2(UnityEngine.Random.Range(0, Const.SquareSize), UnityEngine.Random.Range(0, Const.SquareSize));
        var posB = new Vector2((posA.x + UnityEngine.Random.Range(1, Const.SquareSize - 1)) % Const.SquareSize, (posA.y + UnityEngine.Random.Range(1, Const.SquareSize - 1)) % Const.SquareSize);
        stageState[(int)posA.x, (int)posA.y] = generateCellNumbers[0];
        stageState[(int)posB.x, (int)posB.y] = UnityEngine.Random.Range(0, 1.0f) < Const.ProbabilityOfSelectGeneratingCell ? generateCellNumbers[0] : generateCellNumbers[1];
    }

    /// <summary>
    /// 右矢印キーが押された際に実行される処理
    /// </summary>
    public void MoveCellsRight()
    {
        for (var col = Const.SquareSize; col >= 0; col--)
        {
            for (var row = 0; row < Const.SquareSize; row++)
            {
                if (CheckCell(row, col, 1, 0))
                {
                    // 移動可能条件を満たした場合のみ移動処理
                    MoveCell(row, col, 1, 0);
                }
            }
        }
        PostProcessAfterMove();
    }

    /// <summary>
    /// 左矢印キーが押された際に実行される処理
    /// </summary>
    public void MoveCellsLeft()
    {
        for (var row = 0; row < Const.SquareSize; row++)
        {
            for (var col = 0; col < Const.SquareSize; col++)
            {
                if (CheckCell(row, col, -1, 0))
                {
                    // 移動可能条件を満たした場合のみ移動処理
                    MoveCell(row, col, -1, 0);
                }
            }
        }
        PostProcessAfterMove();
    }

    /// <summary>
    /// 上矢印キーが押された際に実行される処理
    /// </summary>
    public void MoveCellsUp()
    {
        for (var row = 0; row < Const.SquareSize; row++)
        {
            for (var col = 0; col < Const.SquareSize; col++)
            {
                if (CheckCell(row, col, 0, -1))
                {
                    // 移動可能条件を満たした場合のみ移動処理
                    MoveCell(row, col, 0, -1);
                }
            }
        }
        PostProcessAfterMove();
    }

    /// <summary>
    /// 下矢印キーが押された際に実行される処理
    /// </summary>
    public void MoveCellsDown()
    {
        for (var row = Const.SquareSize; row >= 0; row--)
        {
            for (var col = 0; col < Const.SquareSize; col++)
            {
                if (CheckCell(row, col, 0, 1))
                {
                    // 移動可能条件を満たした場合のみ移動処理
                    MoveCell(row, col, 0, 1);
                }
            }
        }
        PostProcessAfterMove();
    }

    /// <summary> 
    /// スコアの計算ロジック
    /// <param name="cellValue">合成する数値マスの値</param>
    /// </summary>
    public void SetScore(int cellValue)
    {
        score += cellValue * 2;
        OnChangeScore?.Invoke(score);
    }

    public int GetScore(){ return score; }

    /// <summary>
    ///  ゲームの1ターン分サイクルの実行
    /// </summary>
    public void PostProcessAfterMove()
    {
        if (!isDirty) { return; }
        CreateNewRandomCell();
        OnChangeStageState?.Invoke(stageState);

        if (IsGameOver(stageState)){ OnGameOver?.Invoke(); }
        isDirty = false;
    }

    /// <summary>
    ///  ハイスコアが更新されるかの確認してハイスコアの値を返す
    /// </summary>
    public bool IsHighScore()
    {
        return score > highScore;
    }

    /// <summary>
    /// ハイスコアの値セットとViewへのイベントを発火
    /// </summary>
    public void SetHighScore(int score)
    {
        highScore = score;
        // ハイスコアの値をViewに反映
        OnChangeHighScore?.Invoke(score);
    }
}
