using UnityEngine;
using UnityEngine.SceneManagement;

public class InGamePresenter : MonoBehaviour
{
    private InGameModel inGameModel;
    private InGameView inGameView;

    // ステージの縦と横の長さを定義する
    private const int squareSize = 4;
    // Cellで初期生成する値を入れた配列
    private int[] generateCellNumber = new int[2]{2,4};
    // 確率の％を定義する
    private const float resultProbability = 0.5f;
    [SerializeField] private Cell[] cells;
    private readonly int[,] stageState = new int[squareSize, squareSize];

    // 盤面の再描画を行う必要があるかのフラグ
    private bool isDirty;


    private void Start()
    {

        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        // Modelの値の変更を監視する
        inGameModel.changeScore += inGameView.SetScore;
        

        // ステージの初期状態を生成
        for (var i = 0; i < squareSize; i++)
        {
            for (var j = 0; j < squareSize; j++)
            {
                stageState[i, j] = 0;
            }
        }
        var posA = new Vector2(Random.Range(0, squareSize), Random.Range(0, squareSize));
        var posB = new Vector2((posA.x + Random.Range(1, squareSize-1)) % squareSize, (posA.y + Random.Range(1, squareSize-1)) % squareSize);
        stageState[(int)posA.x, (int)posA.y] = generateCellNumber[0];
        stageState[(int)posB.x, (int)posB.y] = Random.Range(0, 1.0f) < resultProbability ? generateCellNumber[0] : generateCellNumber[1];

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
            for (var i = 0; i < squareSize; i++)
            {
                for (var j = 0; j < squareSize; j++)
                {
                    cells[i * squareSize + j].SetText(stageState[i, j]);
                }
            }

            if (IsGameOver(stageState))
            {
                PlayerPrefs.SetInt("SCORE", inGameModel.GetScore());
                LoadResultScene();
            }
        }

    }

    
    

    private bool BorderCheck(int row, int column, int horizontal, int vertical)
    {
        // チェックマスが4x4外ならそれ以上処理を行わない
        if (row < 0 || row >= squareSize || column < 0 || column >= squareSize)
        {
            return false;
        }

        // 移動先が4x4外ならそれ以上処理は行わない
        var nextRow = row + vertical;
        var nextCol = column + horizontal;
        if (nextRow < 0 || nextRow >= squareSize || nextCol < 0 || nextCol >= squareSize)
        {
            return false;
        }

        return true;
    }

    private void Check(int row, int column, int horizontal, int vertical)
    {
        // 4x4の境界線チェック
        if (BorderCheck(row, column, horizontal, vertical) == false)
        {
            return;
        }
        // 空欄マスは移動処理をしない
        if (stageState[row, column] == 0)
        {
            return;
        }
        // 移動可能条件を満たした場合のみ移動処理
        Move(row, column, horizontal, vertical);
    }

    private void Move(int row, int column, int horizontal, int vertical)
    {
        // 4x4境界線チェック
        // 再起呼び出し以降も毎回境界線チェックはするため冒頭で呼び出しておく
        if (BorderCheck(row, column, horizontal, vertical) == false)
        {
            return;
        }
        // 移動先の位置を計算
        var nextRow = row + vertical;
        var nextCol = column + horizontal;

        // 移動元と移動先の値を取得
        var value = stageState[row, column];
        var nextValue = stageState[nextRow, nextCol];

        // 次の移動先のマスが0の場合は移動する
        if (nextValue == 0)
        {
            // 移動元のマスは空欄になるので0を埋める
            stageState[row, column] = 0;

            // 移動先のマスに移動元のマスの値を代入する
            stageState[nextRow, nextCol] = value;

            // 移動先のマスでさらに移動チェック
            Move(nextRow, nextCol, horizontal, vertical);
        }
        // 同じ値のときは合成処理
        else if (value == nextValue)
        {
            stageState[row, column] = 0;
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

        stageState[row, col] = Random.Range(0, 1f) < resultProbability ? generateCellNumber[0] : generateCellNumber[1];
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
        for (var i = 0; i < stageState.GetLength(0); i++)
        {
            for (var j = 0; j < stageState.GetLength(1); j++)
            {
                var state = stageState[i, j];
                var canMerge = false;
                if (i > 0)
                {
                    canMerge |= state == stageState[i - 1, j];
                }

                if (i < stageState.GetLength(0) - 1)
                {
                    canMerge |= state == stageState[i + 1, j];
                }

                if (j > 0)
                {
                    canMerge |= state == stageState[i, j - 1];
                }

                if (j < stageState.GetLength(1) - 1)
                {
                    canMerge |= state == stageState[i, j + 1];
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