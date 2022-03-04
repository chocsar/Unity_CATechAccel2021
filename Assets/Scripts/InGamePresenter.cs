using UnityEngine;
using System;

public class InGamePresenter : MonoBehaviour
{
    // ViewとModelを繋ぐために変数として宣言
    private InGameModel inGameModel;
    private InGameView inGameView;
    [SerializeField] private MenuWindowView menuWindowView;

    // C# Action
    public event Action<int> OnChangeHighScore;

    private void Start()
    {
        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        //以下各構造の紐付け
        // Model → View
        // Modelの値の変更を監視する
        inGameModel.OnChangeScore += inGameView.SetScore;
        // ステージのCell状の値の変更を監視する
        inGameModel.OnChangeStageState += inGameView.ApplyStageView;
        inGameModel.OnChangeHighScore += inGameView.SetHighScore;

        // View → Model
        // Viewの右矢印が押されているかを監視する
        inGameView.OnInputRight += inGameModel.MoveCellsRight;
        inGameView.OnInputLeft += inGameModel.MoveCellsLeft;
        inGameView.OnInputUp += inGameModel.MoveCellsUp;
        inGameView.OnInputDown += inGameModel.MoveCellsDown;

        // Model → Presenter
        inGameModel.OnGameOver += OnGameOverProcess;
        inGameModel.OnGameRestart += LoadGameScene;

        // Presenter → Model
        OnChangeHighScore += inGameView.SetHighScore;

        // ManagerView → MenuView
        inGameView.OnClickMenuButton += menuWindowView.OpenWindow;

        // MenuView → Model
        menuWindowView.OnClickRestartButton += inGameModel.GameRestart;

        //Initialize
        inGameModel.Initialize();
        // ハイスコアの値セットとViewへのイベントを発火
        inGameModel.SetHighScore(ScoreController.Instance.GetHighScore());
    }

    /// <summary>
    ///  ゲームオーバーになった後にスコアの保存と保存しているハイスコアの値変更、シーンのロードを行う
    /// </summary>
    private void OnGameOverProcess()
    {
        // ハイスコアが更新されているか確認して、更新されていれば上書き
        if (inGameModel.IsHighScore()) { SaveHighScore(inGameModel.GetScore()); }
        // スコアの保存
        ScoreController.Instance.SaveScore(inGameModel.GetScore());
        // シーンのロード
        LoadResultScene();
    }

    /// <summary>
    /// ResultSceneをロードする
    /// </summary>
    private void LoadResultScene()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.ResultScene);
    }

    private void LoadGameScene()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.InGameScene);
    }

    /// <summary>
    /// ハイスコアの保存を実施
    /// </summary>
    private void SaveHighScore(int score)
    {
        ScoreController.Instance.SaveHighScore(score);
    }
}