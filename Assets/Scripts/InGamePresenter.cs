using UnityEngine;
using System;

public class InGamePresenter : MonoBehaviour
{
    // ViewとModelを繋ぐために変数として宣言
    private InGameModel inGameModel;
    private InGameView inGameView;

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
        inGameModel.OnGameOver += LoadResultScene;


        // Presenter → Model
        OnChangeHighScore += inGameView.SetHighScore;

        //Initialize
        inGameModel.Initialize();
        // ハイスコアの値セットとViewへのイベントを発火
        inGameModel.SetHighScore(ScoreController.Instance.GetHighScore());
    }

    /// <summary>
    /// ResultSceneをロードする
    /// </summary>
    private void LoadResultScene()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.ResultScene);
    }
}