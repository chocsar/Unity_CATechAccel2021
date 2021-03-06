using UnityEngine;
using System;
using UniRx;

public class InGamePresenter : MonoBehaviour
{
    // ViewとModelを繋ぐために変数として宣言
    private InGameModel inGameModel;
    private InGameView inGameView;
    [SerializeField] private MenuWindowView menuWindowView;

    private void Start()
    {
        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        //以下各構造の紐付け
        // Model → View
        // Modelの値の変更を監視する
        inGameModel.ReactiveScore.Subscribe(score => inGameView.SetScore(score)).AddTo(this);
        inGameModel.ReactiveHighScore.Subscribe(highScore => inGameView.SetHighScore(highScore)).AddTo(this);
        // ステージのCell状の値の変更を監視する
        inGameModel.OnChangeStageState.Subscribe(state => inGameView.ApplyStageView(state)).AddTo(this);

        // View → Model
        // Viewの右矢印が押されているかを監視する
        inGameView.OnInputRight += inGameModel.MoveCellsRight;
        inGameView.OnInputLeft += inGameModel.MoveCellsLeft;
        inGameView.OnInputUp += inGameModel.MoveCellsUp;
        inGameView.OnInputDown += inGameModel.MoveCellsDown;

        // Model → Presenter
        inGameModel.OnGameOver.Subscribe(_ => OnGameOverProcess()).AddTo(this);

        // ManagerView → MenuView
        inGameView.OnClickMenuButton += menuWindowView.OpenWindow;

        // MenuView → Presenter
        menuWindowView.OnClickRestartButton += OnGameRestartProcess;

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
        // ハイスコアかを判定してスコアとハイスコアの保存
        OnSaveScores();
        // シーンのロード
        LoadResultScene();
    }

    /// <summary>
    /// GameRestartされた際の処理
    /// </summary>
    private void OnGameRestartProcess()
    {
        // ハイスコアかを判定してスコアとハイスコアの保存
        OnSaveScores();
        // シーンのロード
        LoadGameScene();
    }

    /// <summary>
    /// ResultSceneをロードする
    /// </summary>
    private void LoadResultScene()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.ResultScene);
    }

    /// <summary>
    /// ハイスコアかを判定してスコアとハイスコアの保存
    /// </summary>
    private void OnSaveScores()
    {
        // ハイスコアが更新されているか確認して、更新されていれば上書き
        if (inGameModel.IsHighScore()) { SaveHighScore(inGameModel.GetScore()); }
        // スコアの保存
        ScoreController.Instance.SaveScore(inGameModel.GetScore());
    }


    /// <summary>
    /// InGameSceneをロードする
    /// </summary>
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