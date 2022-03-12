using UnityEngine;

public interface IInputable
{
    bool GetRightInput();
    bool GetLeftInput();
    bool GetUpInput();
    bool GetDownInput();
    Const.Inputs GetDirection(Vector3 position);
    void SetStartTouchPosition(Vector3 position);
}
