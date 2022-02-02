using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : MonoBehaviour
{
    [SerializeField] Button closeButton;

    /// <summary> /// Windowを表示する /// </summary>
    public void OpenWindow()
    {
        this.gameObject.SetActive(true);

    }

    /// <summary> /// Windowを非表示にする /// </summary>
    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }

}
