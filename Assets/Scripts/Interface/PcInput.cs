using UnityEngine;

public class PcInput : IInputable
{
    public bool GetRightInput()
    {
        return Input.GetKeyDown(KeyCode.RightArrow);
    }

    public bool GetLeftInput()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }

    public bool GetUpInput()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public bool GetDownInput()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }

    public Const.Inputs GetDirection(Vector3 position)
    {
        return Const.Inputs.None;
    }

    public void SetStartTouchPosition(Vector3 position)
    {

    }
}
