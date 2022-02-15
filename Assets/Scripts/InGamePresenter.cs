using UnityEngine;

public class InGamePresenter : MonoBehaviour
{
    private InGameModel inGameModel;
    private InGameView inGameView;

    private void Start()
    {

        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        // Model → View
        // Modelの値の変更を監視する
        inGameModel.ChangeScore += inGameView.SetScore;
        // ステージのCell状の値の変更を監視する
        inGameModel.ApplyStageView += inGameView.ApplyStageView;

        // View → Model
        // Viewの右矢印が押されているかを監視する
        inGameView.MoveCellsRight += inGameModel.MoveCellsRight;
        inGameView.MoveCellsLeft += inGameModel.MoveCellsLeft;
        inGameView.MoveCellsUp += inGameModel.MoveCellsUp;
        inGameView.MoveCellsDown += inGameModel.MoveCellsDown;
    }

}