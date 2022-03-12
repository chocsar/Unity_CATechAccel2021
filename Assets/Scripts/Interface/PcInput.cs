using UnityEngine;

public class PcInput : MonoBehaviour,IInputable
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
}
