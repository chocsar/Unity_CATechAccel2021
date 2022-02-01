using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : MonoBehaviour
{
    [SerializeField] Button closeButton;

    // Windowを表示する
    public void OpenWindow()
    {
        this.gameObject.SetActive(true);

    }

    // Windowを非表示にする
    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }

}
