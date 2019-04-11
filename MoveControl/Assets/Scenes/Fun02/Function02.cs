
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 1、博客链接：https://www.liangzl.com/get-article-detail-27947.html
/// 2、通过四元数旋转来移动。
/// </summary>
public class Function02 : MonoBehaviour
{
    private int mSpeed;

    private void Awake()
    {
        mSpeed = 10;
    }


    // Update is called once per frame
    void Update()
    {
        HandleKeyboardAction();
    }

    void HandleKeyboardAction()
    {
        var horizontal = Input.GetAxis("Horizontal") * mSpeed * Time.deltaTime;
        var vertical = Input.GetAxis("Vertical") * mSpeed * Time.deltaTime;
        var motion = transform.rotation * new Vector3(horizontal, 0, vertical);//获取XZ平面上的向量，来进行移动。
        var mag = motion.magnitude;
        motion.y = 0;
        transform.position += motion.normalized * mag;
    }
}
