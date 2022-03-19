using System;
using UnityEngine;

public class SmartphoneInput : IInputable
{
    /// <summary> フリックを開始した際の座標を代入する変数 </summary>
    private Vector3 startTouchPosition;
    /// <summary> フリックを終了した際の座標を代入する変数  </summary>
    private Vector3 endTouchPosition;
    /// <summary> フリックの変化量を代入する変数 </summary>
    private Vector2 flickValue;
    /// <summary> フリック中か判定する変数 </summary>
    private bool isFlick = false;

    /// <summary>
    /// フリック方向のベクトルを計算する
    /// </summary>
    private void CheckFlickValue()
    {
        flickValue.x = endTouchPosition.x - startTouchPosition.x;
        flickValue.y = endTouchPosition.y - startTouchPosition.y;
    }

    /// <summary>
    /// フリックを終了した際の移動方向を計算
    /// </summary>
    public Const.InputDirection GetDirection()
    {
        // 画面をタップしていない場合は処理を終了させる
        if (Input.touchCount == 0)
        {
            return Const.InputDirection.None;
        }
        Touch touch = Input.GetTouch(0);
        // タップし始めたときの処理
        if (touch.phase == TouchPhase.Began)
        {
            // フリックを開始した際の座標を代入
            startTouchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            isFlick = true;
        }
        else if (touch.phase == TouchPhase.Moved && isFlick)
        {
            // タッチ移動
            // フリックを開始した際の座標を代入
            SetEndPosition();
            // フリック方向のベクトルを計算する
            CheckFlickValue();
            return CheckDirection();
        }
        else if (touch.phase == TouchPhase.Ended && isFlick)
        {
            // フリックを開始した際の座標を代入
            SetEndPosition();
            // フリック方向のベクトルを計算する
            CheckFlickValue();
            return CheckDirection();
        }
        return Const.InputDirection.None;
    }

    /// <summary>
    /// どちら向きへフリックされたかの判定部分
    /// </summary>
    private Const.InputDirection CheckDirection()
    {
        // フリックの値が規定量に満たさない場合に処理を終了させる
        if (Math.Abs(flickValue.x) <= Const.FlickDirectionValue && Math.Abs(flickValue.y) <= Const.FlickDirectionValue)
        {
            return Const.InputDirection.None;
        }
        // フリックを終了した判定にする
        isFlick = false;
        // もしflickValue.yよりflickValue.xが大きければy軸方向の処理を優先させる
        if (Math.Abs(flickValue.x) > Math.Abs(flickValue.y))
        {
            if (flickValue.x > Const.FlickDirectionValue)
            {
                return Const.InputDirection.Right;
            }
            if (flickValue.x < -Const.FlickDirectionValue)
            {
                return Const.InputDirection.Left;
            }
        }
        else
        {
            if (flickValue.y > Const.FlickDirectionValue)
            {
                return Const.InputDirection.Up;
            }
            if (flickValue.y < -Const.FlickDirectionValue)
            {
                return Const.InputDirection.Down;
            }
        }
        // もしフリックされたかの判定をした結果規定量を満たさなかった際に処理をNoneとして終える
        return Const.InputDirection.None;
    }

    /// <summary>
    /// タッチを終了した座標を代入する
    /// </summary>
    private void SetEndPosition()
    {
        endTouchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
    }
}

