public class Const
{
    /// <summary> ステージの縦横の長さ </summary>
    public const int SquareSize = 4;
    /// <summary> セルに2か4のどちらが生成されるかを決める確率を定義する </summary>
    public const float ProbabilityOfSelectGeneratingCell = 0.5f;
    /// <summary> フリックされた際に判定する上限値を定義する </summary>
    public const float FlickDirectionValue = 200.0f;
    /// <summary> インプットの方向を定義 </summary>
    public enum InputDirection
    {
        None,
        Right,
        Left,
        Up,
        Down, 
    }
    /// <summary> ランキングデータを保存するJSONファイルのパス </summary>
    public const string jsonDataPath = "Assets/Resources/RankingData.json";
}
