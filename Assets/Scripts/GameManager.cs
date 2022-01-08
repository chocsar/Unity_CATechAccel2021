using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Cell[] cells;
    [SerializeField] private Text scoreText;
    private readonly int[,] _stageState = new int[4, 4];

    /// <summary>
    /// 盤面の再描画を行う必要があるかのフラグ
    /// </summary>
    private bool isDirty;

    private int score;

    private void Start()
    {
        // ステージの初期状態を生成
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                _stageState[i, j] = 0;
            }
        }
        var posA = new Vector2(Random.Range(0, 4), Random.Range(0, 4));
        var posB = new Vector2((posA.x + Random.Range(1, 3)) % 4, (posA.y + Random.Range(1, 3))% 4);
        _stageState[(int)posA.x, (int)posA.y] = 2;
        _stageState[(int) posB.x, (int) posB.y] = Random.Range(0, 1.0f) < 0.5f ? 2 : 4;
        
        // ステージの初期状態をViewに反映
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                cells[i * 4 + j].SetText(_stageState[i, j]);
            }
        }
    }

    private void Update()
    {
        isDirty = false;
        // 入力検知
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (var col = 4; col >= 0; col--)
            {
                for (var row = 0; row < 4; row++)
                {
                    Check(row, col, 1, 0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (var row = 0; row < 4; row++)
            {
                for (var col = 0; col < 4; col++)
                {
                    Check(row, col, -1, 0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (var row = 0; row < 4; row++)
            {
                for (var col = 0; col < 4; col++)
                {
                    Check(row, col, 0, -1);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (var row = 4; row >= 0; row--)
            {
                for (var col = 0; col < 4; col++)
                {
                    Check(row, col, 0, 1);
                }
            }
        }

        if (isDirty)
        {
            CreateNewRandomCell();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    cells[i * 4 + j].SetText(_stageState[i, j]);
                }
            }

            if (IsGameOver(_stageState))
            {
                PlayerPrefs.SetInt("SCORE", score);
                LoadResultScene();
            }
        }
    }
    
    private bool BorderCheck(int row, int column, int horizontal, int vertical)
    {
        // チェックマスが4x4外ならそれ以上処理を行わない
        if (row < 0 || row >= 4 || column < 0 || column >= 4)
        {
            return false;
        }
        
        // 移動先が4x4外ならそれ以上処理は行わない
        var nextRow = row + vertical;
        var nextCol = column + horizontal;
        if (nextRow < 0 || nextRow >= 4 || nextCol < 0 || nextCol >= 4)
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
        if (_stageState[row, column] == 0)
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
        var value = _stageState[row, column];
        var nextValue = _stageState[nextRow, nextCol];
        
        // 次の移動先のマスが0の場合は移動する
        if (nextValue == 0)
        {
            // 移動元のマスは空欄になるので0を埋める
            _stageState[row, column] = 0;
            
            // 移動先のマスに移動元のマスの値を代入する
            _stageState[nextRow, nextCol] = value;
            
            // 移動先のマスでさらに移動チェック
            Move(nextRow, nextCol, horizontal, vertical);
        }
        // 同じ値のときは合成処理
        else if (value == nextValue)
        {
            _stageState[row, column] = 0;
            _stageState[nextRow, nextCol] = value * 2;
            score += value * 2;
            scoreText.text = $"Score: {score}";
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
        if (IsGameOver(_stageState))
        {
            return;
        }
        var row = Random.Range(0, 4);
        var col = Random.Range(0, 4);
        while (_stageState[row, col] != 0)
        {
            row = Random.Range(0, 4);
            col = Random.Range(0, 4);
        }

        _stageState[row, col] = Random.Range(0, 1f) < 0.5f ? 2 : 4;
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