using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField]
    private Text resultText;

    private void Start()
    {
        resultText.text = ScoreController.Instance.LoadScore().ToString();
    }








    public void OnClickRetryButton()
    {
        SceneController.Instance.LoadScene(SceneController.SceneNames.InGameScene);
    }
}
