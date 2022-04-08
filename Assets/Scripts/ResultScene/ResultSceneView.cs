using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSceneView : MonoBehaviour
{
    [SerializeField] private Text resultText;


    private void Start()
    {
        resultText.text = ScoreController.Instance.LoadScore().ToString();
    }




    public void SetResultText(string text)
    {
        resultText.text = text;
    }



    public void OnClickRetryButton()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.InGameScene);
    }
}
