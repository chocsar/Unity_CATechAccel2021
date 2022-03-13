using UnityEngine;

public class PcInput : IInputable
{
    public Const.InputDirection GetDirection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return Const.InputDirection.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return Const.InputDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Const.InputDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return Const.InputDirection.Down;
        }
        return Const.InputDirection.None;
    }
}
