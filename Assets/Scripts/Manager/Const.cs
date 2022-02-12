public class Const
{
    /// <summary> /// ステージの縦横の長さ /// </summary>
    public const int SquareSize = 4;
    /// <summary> /// セルに2か4のどちらが生成されるかを決める確率を定義する /// </summary>
    public const float ProbabilityOfSelectGeneratingCell = 0.5f;
    /// <summary> /// シーンネームを列挙型で定義する /// </summary>
    public enum SceneNames
    {
        ResultScene,
        InGameScene,
        TitleScene,
    }
    public enum SaveKeyNames
    {
        Score,
    }
}
