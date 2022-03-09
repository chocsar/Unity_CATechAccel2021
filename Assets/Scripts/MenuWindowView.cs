using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuWindowView : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button restartButton;

    public event Action OnClickRestartButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() => CloseWindow());
        restartButton.onClick.AddListener(() => OnClickRestartButton?.Invoke());
    }

    /// <summary>
    /// Windowを表示する 
    /// </summary>
    public void OpenWindow()
    {
        this.gameObject.SetActive(true);

    }

    /// <summary> 
    /// Windowを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }

}
