using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 1、链接：https://blog.csdn.net/yongh701/article/details/71082441
/// 2、这里拉近拉远的效果是通过改变视角fieldOfView来控制的。
/// 3、镜头的旋转是通过Rotate方法来实现的。
/// </summary>
public class CameraMove : MonoBehaviour
{
    public float sensitivityMouse = 2f;
    public float sensitivityKeyBoard = 0.1f;
    public float sensitivityMouseWheel = 10f;
    public GameObject cameraObj;

    void Update()
    {
        //滚轮实现镜头的拉近拉远。
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraObj.GetComponent<Camera>().fieldOfView = cameraObj.GetComponent<Camera>().fieldOfView - Input.GetAxis("Mouse ScrollWheel") * sensitivityMouseWheel * Time.deltaTime;
        }

        //鼠标右键实现视角转动。
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(-Input.GetAxis("Mouse Y") * sensitivityMouse * Time.deltaTime, Input.GetAxis("Mouse X") * sensitivityMouse * Time.deltaTime, 0); //不能在一个物体上控制两个轴的旋转，否视角会倾倒。
        }

        //ad键实现视角水平移动，ws键实现垂直移动。
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * sensitivityKeyBoard * Time.deltaTime, 0, 0);
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(0, Input.GetAxis("Vertical") * sensitivityKeyBoard * Time.deltaTime, 0);
        }
    }
}
