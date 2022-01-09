using UnityEngine;
using System;

public class InGameModel : MonoBehaviour
{

    private int score;
    public event Action<int> changeScore;

    /// <summary>
    /// スコアの計算ロジック
    /// </summary>
    /// <param name="cellValue">合成する数値マスの値</param>
    public void SetScore(int cellValue)
    {
        score += cellValue * 2;
        changeScore(score);
    }

    public int GetScore(){ return score; }



}
