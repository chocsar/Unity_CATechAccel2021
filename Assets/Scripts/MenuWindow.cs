using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : MonoBehaviour
{
    [SerializeField] Button CloseButton;

    /// <summary>
    /// Windowを表示する
    /// </summary>
    public void OpenWindow()
    {
        gameObject.SetActive(true);

    }

    /// <summary>
    /// Windowを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

}
