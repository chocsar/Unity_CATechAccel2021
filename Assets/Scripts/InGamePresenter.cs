using UnityEngine;

public class InGamePresenter : MonoBehaviour
{
    /// ViewとModelを繋ぐために変数として宣言
    private InGameModel inGameModel;
    private InGameView inGameView;

    private void Start()
    {

        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        // Model → View
        // Modelの値の変更を監視する
        inGameModel.OnChangeScore += inGameView.SetScore;
        // ステージのCell状の値の変更を監視する
        inGameModel.OnChangeStageState += inGameView.ApplyStageView;

        // View → Model
        // Viewの右矢印が押されているかを監視する
        inGameView.OnInputRight += inGameModel.MoveCellsRight;
        inGameView.OnInputLeft += inGameModel.MoveCellsLeft;
        inGameView.OnInputUp += inGameModel.MoveCellsUp;
        inGameView.OnInputDown += inGameModel.MoveCellsDown;

        // Model → Presenter
        inGameModel.OnGameOver += LoadResultScene;
    }

    /// <summary>
    /// ResultSceneをロードする
    /// </summary>
    private void LoadResultScene()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.ResultScene);
    }
}