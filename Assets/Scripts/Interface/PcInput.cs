using UnityEngine;

public class PcInput : MonoBehaviour , IInputable
{
    public bool GetRightInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return true;
        }
        return false;
    }
    public bool GetLeftInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return true;
        }
        return false;
    }
    public bool GetUpInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return true;
        }
        return false;
    }
    public bool GetDownInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return true;
        }
        return false;
    }
}
