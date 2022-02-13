using UnityEngine;

public class InGamePresenter : MonoBehaviour
{
    private InGameModel inGameModel;
    private InGameView inGameView;

    /// <summary> /// 生成するCellの値を入れた配列 /// </summary>
    private int[] generateCellNumbers = new int[2]{2,4};
    [SerializeField] private Cell[] cells;
    private readonly int[,] stageState = new int[Const.SquareSize, Const.SquareSize];

    /// <summary> /// 盤面の再描画を行う必要があるかのフラグ /// </summary>
    private bool isDirty;


    private void Start()
    {

        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        // Modelの値の変更を監視する
        inGameModel.ChangeScore += inGameView.SetScore;


        // ステージの初期状態を生成
        InitializeStage();
        // ステージの初期状態をViewに反映
        ApplyStageView();
    }

    

    private void Update()
    {

        isDirty = false;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCellsRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCellsLeft();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCellsUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCellsDown();
        }

        if (isDirty)
        {
            CreateNewRandomCell();
            ApplyStageView();

            if (IsGameOver(stageState))
            {
                ScoreController.Instance.SaveScore(inGameModel.GetScore());
                LoadResultScene();
            }
        }

    }

    
    

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
        inGameModel.SetScore(value);
    }

    private void CreateNewRandomCell()
    {
        // ゲーム終了時はスポーンしない
        if (IsGameOver(stageState))
        {
            return;
        }
        var row = Random.Range(0, Const.SquareSize);
        var col = Random.Range(0, Const.SquareSize);
        while (stageState[row, col] != 0)
        {
            row = Random.Range(0, Const.SquareSize);
            col = Random.Range(0, Const.SquareSize);
        }

        stageState[row, col] = Random.Range(0, 1f) < Const.ProbabilityOfSelectGeneratingCell ? generateCellNumbers[0] : generateCellNumbers[1];
    }

    /// <summary>
    /// ステージの初期状態をViewに反映
    /// </summary>
    private void ApplyStageView()
    {
        for (var row = 0; row < Const.SquareSize; row++)
        {
            for (var col = 0; col < Const.SquareSize; col++)
            {
                cells[row * Const.SquareSize + col].SetText(stageState[row, col]);
            }
        }
    }

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

    private void LoadResultScene()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.ResultScene);
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
        var posA = new Vector2(Random.Range(0, Const.SquareSize), Random.Range(0, Const.SquareSize));
        var posB = new Vector2((posA.x + Random.Range(1, Const.SquareSize - 1)) % Const.SquareSize, (posA.y + Random.Range(1, Const.SquareSize - 1)) % Const.SquareSize);
        stageState[(int)posA.x, (int)posA.y] = generateCellNumbers[0];
        stageState[(int)posB.x, (int)posB.y] = Random.Range(0, 1.0f) < Const.ProbabilityOfSelectGeneratingCell ? generateCellNumbers[0] : generateCellNumbers[1];
    }

    /// <summary>
    /// 矢印キーが押された際に実行される処理
    /// </summary>
    private void MoveCellsRight()
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
    }

    private void MoveCellsLeft()
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
    }

    private void MoveCellsUp()
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
    }

    private void MoveCellsDown()
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
    }



}