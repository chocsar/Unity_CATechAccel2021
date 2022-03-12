using UnityEngine;

public class SmartphoneInput : IInputable
{
    /// <summary> フリックを開始した際の座標を代入する変数 </summary>
    private Vector3 startTouchPosition;
    /// <summary> フリックを終了した際の座標を代入する変数  </summary>
    private Vector3 endTouchPosition;
    /// <summary> フリックの変化量を代入する変数 </summary>
    private Vector2 flickValue;

    /// <summary>
    /// 
    /// </summary>
    private void FlickDirection()
    {
        flickValue.x = endTouchPosition.x - startTouchPosition.x;
        flickValue.y = endTouchPosition.y - startTouchPosition.y;
    }

    /// <summary>
    /// フリックを開始した際の座標を代入
    /// </summary>
    public void SetStartTouchPosition(Vector3 position)
    {
        startTouchPosition = position;
    }

    /// <summary>
    /// フリックを終了した際の移動方向を計算
    /// </summary>
    public Const.Inputs GetDirection(Vector3 position)
    {
        endTouchPosition = position;
        FlickDirection();
        if (flickValue.x > 200.0f)
        {
            return Const.Inputs.Right;
        }

        if (flickValue.x < -200.0f)
        {
            return Const.Inputs.Left;
        }

        if (flickValue.y > 200.0f)
        {
            return Const.Inputs.Up;
        }
        if (flickValue.y < -200.0f)
        {
            return Const.Inputs.Down;
        }
        return Const.Inputs.None;
    }

    public bool GetRightInput()
    {
        return false;
    }
    public bool GetLeftInput()
    {
        return false;
    }
    public bool GetUpInput()
    {
        return false;
    }
    public bool GetDownInput()
    {
        return false;
    }
}

