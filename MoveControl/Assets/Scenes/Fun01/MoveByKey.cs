using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 只有当坐标值为正交值得时候才能使用这个脚本。当摄像机有角度的时候不行。
/// 脚本可以添加在游戏物体上，也可以添加在摄像机上。各有不同的效果。
/// </summary>
public class MoveByKey : MonoBehaviour
{
    private int mSpeed;

    private void Start()
    {
        mSpeed = 10;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.up * mSpeed * Time.deltaTime);//Vector3.up等是沿着自身坐标系进行移动的。
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.down * mSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * mSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * mSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * mSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * mSpeed * Time.deltaTime);
        }
    }
}
