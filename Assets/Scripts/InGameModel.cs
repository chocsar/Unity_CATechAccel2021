using UnityEngine;
using System;

public class InGameModel : MonoBehaviour
{

    private int score;
    public event Action<int> ChangeScore;

    /// <summary> 
    /// スコアの計算ロジック
    /// <param name="cellValue">合成する数値マスの値</param>
    /// </summary>
    public void SetScore(int cellValue)
    {
        score += cellValue * 2;
        ChangeScore(score);
    }

    public int GetScore(){ return score; }



}
