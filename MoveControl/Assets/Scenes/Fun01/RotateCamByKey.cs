using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 以下两句很奇怪，当合起来使用的时候竟然会产生旋转效果。
/// 长按W键，会出现晃镜头的Bug。
/// </summary>
public class RotateCamByKey : MonoBehaviour
{
    private int mSpeed;//移动速度
    public Transform target;//目标                           

    void Start()
    {
        mSpeed = 10;
    }

    // 上下左右箭头控制摄像机移动
    void Update()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.Rotate(Vector3.up * mSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.up * mSpeed * Time.deltaTime);
            }

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.Rotate(Vector3.down * mSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.down * mSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            //LeftControl为左边的Control键。 
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Camera.main.transform.Rotate(Vector3.left * mSpeed * 2 * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * mSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Camera.main.transform.Rotate(Vector3.right * mSpeed * 2 * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * mSpeed * Time.deltaTime);
            }
        }

        // transform.LookAt(target);//这一句就是让旋转和位移都指向一个中心对象，类似实现目标摄像机的效果。
    }
}



