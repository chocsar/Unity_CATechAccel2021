using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private Text text;
    
    public void SetText(int num)
    {
        text.text = num.ToString();
        gameObject.SetActive(num != 0);
        // TODO: 色が濃くなるロジックを考える
        image.color = Color.HSVToRGB(1f, 1f, 1f);
    }
}
