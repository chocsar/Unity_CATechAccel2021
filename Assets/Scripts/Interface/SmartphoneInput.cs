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
    /// フリック方向のベクトルを計算する
    /// </summary>
    private void FlickDirection()
    {
        flickValue.x = endTouchPosition.x - startTouchPosition.x;
        flickValue.y = endTouchPosition.y - startTouchPosition.y;
    }

    /// <summary>
    /// フリックを開始した際の座標を代入
    /// </summary>
    public void SetStartTouchPosition()
    {
        startTouchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
    }

    /// <summary>
    /// フリックを終了した際の移動方向を計算
    /// </summary>
    public Const.InputDirection GetDirection()
    {
        Debug.Log("call");
        endTouchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        FlickDirection();
        if (flickValue.x > 200.0f)
        {
            return Const.InputDirection.Right;
        }

        if (flickValue.x < -200.0f)
        {
            return Const.InputDirection.Left;
        }

        if (flickValue.y > 200.0f)
        {
            return Const.InputDirection.Up;
        }
        if (flickValue.y < -200.0f)
        {
            return Const.InputDirection.Down;
        }
        return Const.InputDirection.None;
    }
}

