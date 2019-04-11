using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 链接：https://blog.csdn.net/qq_30454411/article/details/79810922
/// </summary>
public class OnTriggerXXX : MonoBehaviour
{
    private void Awake()
    {

    }

    private void Update()
    {

    }


    /// <summary>
    /// 这里当勾选上Is Trigger后，则物体会取消掉Collider属性。
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        //Debug.Log("当进入碰撞器");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        //Debug.Log("当退出碰撞器");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        //Debug.Log("当停留在碰撞器中");
    }



    private void OnMouseEnter()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log("sfasdafads");
    }

    private void OnMouseDrag()
    {

    }

    private void OnMouseOver()
    {

    }

    private void OnMouseExit()
    {

    }

    private void OnMouseUp()
    {

    }

    private void OnMouseUpAsButton()
    {

    }


}
