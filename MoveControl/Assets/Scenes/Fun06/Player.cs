using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 链接：http://gad.qq.com/article/detail/286951
/// 1、这里需要用到CharacterController组件。
/// 2、transform.TransformDirection(moveDirection);//将局部坐标系转换成全局坐标系。
/// 3、这里用的Move来进行移动。Move需要加上Rigidbody组件以后才能使用。
/// </summary>
public class Player : MonoBehaviour
{
    CharacterController controller;
    public Vector3 moveDirection = Vector3.zero;
    public float moveSpeed = 5f;
    public float rotateSpeed;

    public float x, fx, y, fy;
    public float gravity = 200.0f;

    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
        rotateSpeed = GlobalSetting.MouseSensitive;
    }

    private void FixedUpdate()
    {

        //移动
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(h, 0, v);
        moveDirection = transform.TransformDirection(moveDirection);//将局部坐标系转换成全局坐标系。
        moveDirection *= moveSpeed;
        if (Input.GetKey(KeyCode.Q))
        {
            moveDirection.y += 100 * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            moveDirection.y -= 100 * moveSpeed * Time.deltaTime;
        }
        //moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        //旋转
        if (Input.GetMouseButton(1))
        {
            x -= Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
            fx = -x;
            fy = -y;
            transform.eulerAngles = new Vector3(-fy, fx, 0);
        }
    }
}


public class GlobalSetting
{
    private static GlobalSetting Instance;
    public static float MouseSensitive = 150f;
    public static float voice = 0.5f;

    public static GlobalSetting GetInstance()
    {
        if (Instance == null)
        {
            Instance = new GlobalSetting();
        }
        return Instance;
    }
}
