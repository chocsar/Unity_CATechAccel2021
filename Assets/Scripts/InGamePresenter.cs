using UnityEngine;
using UnityEngine.SceneManagement;

public class InGamePresenter : MonoBehaviour
{
    private InGameModel inGameModel;
    private InGameView inGameView;

    /// <summary> /// ステージの縦横の長さ /// </summary>
    private const int squareSize = 4;
    /// <summary> /// 生成するCellの値を入れた配列 /// </summary>
    private int[] generateCellNumber = new int[2]{2,4};
    /// <summary> /// セルに2か4のどちらが生成されるかを決める確率を定義する /// </summary>
    private const float probabilityOfSelectGeneratingCell = 0.5f;
    [SerializeField] private Cell[] cells;
    private readonly int[,] stageState = new int[squareSize, squareSize];

    /// <summary> /// 盤面の再描画を行う必要があるかのフラグ /// </summary>
    private bool isDirty;


    private void Start()
    {

        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        // Modelの値の変更を監視する
        inGameModel.ChangeScore += inGameView.SetScore;
        

        // ステージの初期状態を生成
        for (var row = 0; row < squareSize; row++)
        {
            for (var col = 0; col < squareSize; col++)
            {
                stageState[row, col] = 0;
            }
        }
        var posA = new Vector2(Random.Range(0, squareSize), Random.Range(0, squareSize));
        var posB = new Vector2((posA.x + Random.Range(1, squareSize-1)) % squareSize, (posA.y + Random.Range(1, squareSize-1)) % squareSize);
        stageState[(int)posA.x, (int)posA.y] = generateCellNumber[0];
        stageState[(int)posB.x, (int)posB.y] = Random.Range(0, 1.0f) < probabilityOfSelectGeneratingCell ? generateCellNumber[0] : generateCellNumber[1];

        // ステージの初期状態をViewに反映
        for (var i = 0; i < squareSize; i++)
        {
            for (var j = 0; j < squareSize; j++)
            {
                cells[i * squareSize + j].SetText(stageState[i, j]);
            }
        }
    }

    

    private void Update()
    {

        isDirty = false;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (var col = squareSize; col >= 0; col--)
            {
                for (var row = 0; row < squareSize; row++)
                {
                    Check(row, col, 1, 0);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (var row = 0; row < squareSize; row++)
            {
                for (var col = 0; col < squareSize; col++)
                {
                    Check(row, col, -1, 0);
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (var row = 0; row < squareSize; row++)
            {
                for (var col = 0; col < squareSize; col++)
                {
                    Check(row, col, 0, -1);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (var row = squareSize; row >= 0; row--)
            {
                for (var col = 0; col < squareSize; col++)
                {
                    Check(row, col, 0, 1);
                }
            }
        }

        if (isDirty)
        {
            CreateNewRandomCell();
            for (var row = 0; row < squareSize; row++)
            {
                for (var col = 0; col < squareSize; col++)
                {
                    cells[row * squareSize + col].SetText(stageState[row, col]);
                }
            }

            if (IsGameOver(stageState))
            {
                PlayerPrefs.SetInt("SCORE", inGameModel.GetScore());
                LoadResultScene();
            }
        }

    }

    
    

    private bool BorderCheck(int row, int col, int horizontal, int vertical)
    {
        // チェックマスが4x4外ならそれ以上処理を行わない
        if (row < 0 || row >= squareSize || col < 0 || col >= squareSize)
        {
            return false;
        }

        // 移動先が4x4外ならそれ以上処理は行わない
        var nextRow = row + vertical;
        var nextCol = col + horizontal;
        if (nextRow < 0 || nextRow >= squareSize || nextCol < 0 || nextCol >= squareSize)
        {
            return false;
        }

        return true;
    }

    private void Check(int row, int col, int horizontal, int vertical)
    {
        // 4x4の境界線チェック
        if (BorderCheck(row, col, horizontal, vertical) == false)
        {
            return;
        }
        // 空欄マスは移動処理をしない
        if (stageState[row, col] == 0)
        {
            return;
        }
        // 移動可能条件を満たした場合のみ移動処理
        Move(row, col, horizontal, vertical);
    }

    private void Move(int row, int col, int horizontal, int vertical)
    {
        // 4x4境界線チェック
        // 再起呼び出し以降も毎回境界線チェックはするため冒頭で呼び出しておく
        if (BorderCheck(row, col, horizontal, vertical) == false)
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
            Move(nextRow, nextCol, horizontal, vertical);
        }
        // 同じ値のときは合成処理
        else if (value == nextValue)
        {
            stageState[row, col] = 0;
            stageState[nextRow, nextCol] = value * 2;
            inGameModel.SetScore(value);
            
        }
        // 異なる値のときは移動処理を終了
        else if (value != nextValue)
        {
            return;
        }

        isDirty = true;
    }

    private void CreateNewRandomCell()
    {
        // ゲーム終了時はスポーンしない
        if (IsGameOver(stageState))
        {
            return;
        }
        var row = Random.Range(0, squareSize);
        var col = Random.Range(0, squareSize);
        while (stageState[row, col] != 0)
        {
            row = Random.Range(0, squareSize);
            col = Random.Range(0, squareSize);
        }

        stageState[row, col] = Random.Range(0, 1f) < probabilityOfSelectGeneratingCell ? generateCellNumber[0] : generateCellNumber[1];
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
        SceneManager.LoadScene("ResultScene");
    }

}