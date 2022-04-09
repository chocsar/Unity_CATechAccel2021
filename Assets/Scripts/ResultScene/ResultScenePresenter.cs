using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScenePresenter : MonoBehaviour
{
    // ViewとModelを繋ぐために変数として宣言
    private ResultSceneModel resultSceneModel;
    private ResultSceneView resultSceneView;

    // Start is called before the first frame update
    void Start()
    {
        resultSceneModel = GetComponent<ResultSceneModel>();
        resultSceneView = GetComponent<ResultSceneView>();

        // View → Presenter
        resultSceneView.OnClickRetryButton += LoadGameScene;

        // Model → View
        resultSceneModel.OnChangeResultScore += resultSceneView.SetResultText;
        resultSceneModel.OnSetRankElement += resultSceneView.GenerateRankElement;
    }

    /// <summary>
    /// InGameSceneをロードする
    /// </summary>
    private void LoadGameScene()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.InGameScene);
    }
}
