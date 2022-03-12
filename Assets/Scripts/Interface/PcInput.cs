using UnityEngine;

public class PcInput : IInputable
{
    public Const.Inputs GetDirection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return Const.Inputs.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return Const.Inputs.Left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Const.Inputs.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return Const.Inputs.Down;
        }
        return Const.Inputs.None;
    }

    public void SetStartTouchPosition()
    {

    }
}
