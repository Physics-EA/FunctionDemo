using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 链接：https://www.jb51.net/article/156753.htm
/// 1、unity编辑器中按住鼠标右键，在通过控制键盘的wasdqe键可以自由控制视野。
/// 2、粗调和微调。
/// </summary>
public class CameraMove_02 : MonoBehaviour
{
    public static CameraMove_02 Instance = null;

    private Vector3 dirVector3;
    private Vector3 rotaVector3;
    private float parameter = 0.1f;

    private float xspeed = -0.05f;
    private float yspeed = 0.1f;
    private float dis;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        rotaVector3 = transform.localEulerAngles;
        //dis = UIFuc.Instance.Dis;
    }
    private void FixedUpdate()
    {
        //旋转
        if (Input.GetMouseButton(1))
        {
            rotaVector3.y += Input.GetAxis("Horizontal") * yspeed;
            rotaVector3.x += Input.GetAxis("Vertical") * yspeed;
            transform.rotation = Quaternion.Euler(rotaVector3);
        }

        //移动
        dirVector3 = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                dirVector3.z = 10;
            }
            else
            {
                dirVector3.z = 1;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                dirVector3.z = -10;
            }
            else
            {
                dirVector3.z = -1;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                dirVector3.x = -10;
            }
            else
            {
                dirVector3.x = -1;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                dirVector3.x = 10;
            }
            else
            {
                dirVector3.x = 1;
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                dirVector3.y = -10;
            }
            else
            {
                dirVector3.y = -1;
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                dirVector3.y = 10;
            }
            else
            {
                dirVector3.y = 1;
            }
        }

        transform.Translate(dirVector3 * parameter, Space.Self);
        transform.position = Vector3.ClampMagnitude(transform.position, 1000);//限制摄像机移动范围。

    }
}
