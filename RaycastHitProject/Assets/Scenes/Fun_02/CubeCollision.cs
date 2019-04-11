using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 链接：https://www.cnblogs.com/zengbinsi/p/zengbinsi_unity3d_004.html
/// </summary>
public class CubeCollision : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        /*销毁这个物体*/
        //Destroy(this.gameObject);
        //Destroy(collision.gameObject);
        //Debug.Log("销毁了这个物体");

        //通过名字
        var name = collision.collider.name;
        Debug.Log("Name is " + "\"" + name + "\"");


        //通过Tag值
        var tag = collision.collider.tag;
        Debug.Log("Tag is " + "\"" + tag + "\"");
    }

    private void OnCollisionExit(Collision collision)
    {

    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("开始接触！");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("持续接触！");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("结束接触！");
    }
}
