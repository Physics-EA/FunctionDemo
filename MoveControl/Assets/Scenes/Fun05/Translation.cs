using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 链接：http://tieba.baidu.com/p/4426505818
/// 1、这里移动的是模型的坐标。
/// </summary>
public class Translation : MonoBehaviour
{
    public GameObject target;
    private Vector3 screenPoint;
    private Vector3 offset;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {

            screenPoint = Camera.main.WorldToScreenPoint(target.transform.position);//将target世界坐标转换成屏幕坐标，只是为了获得z轴方向的值。
            offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));//鼠标的位置转换成世界坐标。offset为target与鼠标在世界坐标系下的差向量。

            Debug.Log(offset);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);//当前屏幕坐标系下鼠标的位置。
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;//将在屏幕坐标系下鼠标的位置转换成世界坐标，加上向量差就是当前target的位置。
            target.transform.position = curPosition;
        }
    }
}
