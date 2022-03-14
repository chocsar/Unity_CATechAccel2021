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
    /// フリックを終了した際の移動方向を計算
    /// </summary>
    public Const.InputDirection GetDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // フリックを開始した際の座標を代入
            startTouchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }
        // フリックを終了していなければNoneとして値を返して処理を終了
        if (!Input.GetMouseButtonUp(0)) return Const.InputDirection.None;
        // フリックを開始した際の座標を代入
        endTouchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        // フリック方向のベクトルを計算する
        FlickDirection();
        return CheckDirection();
    }

    /// <summary>
    /// どちら向きへフリックされたかの判定部分
    /// </summary>
    private Const.InputDirection CheckDirection()
    {
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
        // フリックされたかの判定をした結果規定量を満たさなかった際に処理をNoneとして終える
        return Const.InputDirection.None;
    }
}

