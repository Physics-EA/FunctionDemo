using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 1、链接：http://blog.sina.com.cn/s/blog_04f8decc01015uy6.html
/// 2、
/// </summary>
public class CameraControl : MonoBehaviour
{
    public int sensivityKeyBoard = 20;

 
    void Update()
    {
        //旋转
        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse X") != 0)
            {
                transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sensivityKeyBoard * Time.deltaTime);
            }
            else if (Input.GetAxis("Mouse Y") != 0)
            {
                Camera.main.transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * sensivityKeyBoard * 5 * Time.deltaTime);//这里进行镜头旋转的时候会出现卡顿的现象。
            }
        }

        //移动

        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * sensivityKeyBoard * Time.deltaTime, 0, 0);
        }
        else if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(0, Input.GetAxis("Vertical") * sensivityKeyBoard * Time.deltaTime, 0);
        }

    }
}
