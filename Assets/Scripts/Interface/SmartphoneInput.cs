using UnityEngine;

public class SmartphoneInput : MonoBehaviour
{
    private Vector3 startTouchPos;
    private Vector3 endTouchPos;

    private float flickValue_x;
    private float flickValue_y;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }
        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            FlickDirection();
            GetDirection();
        }
    }

    private void FlickDirection()
    {
        flickValue_x = endTouchPos.x - startTouchPos.x;
        flickValue_y = endTouchPos.y - startTouchPos.y;
        Debug.Log("x スワイプ量は" + flickValue_x);
        Debug.Log("y スワイプ量は" + flickValue_y);
    }

    private void GetDirection()
    {
        if (flickValue_x > 200.0f)
        {
            Debug.Log("right");
        }

        if (flickValue_x < -200.0f)
        {
            Debug.Log("left ");
        }

        if (flickValue_y > 200.0f)
        {
            Debug.Log("up");
        }
        if (flickValue_y < -200.0f)
        {
            Debug.Log("down");
        }
    }

}

