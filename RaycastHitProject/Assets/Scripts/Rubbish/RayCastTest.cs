using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{
    Vector3 mTargetPos = Vector3.zero;
    GameObject shapeTransform;
    float attackTime;
    Transform priringPoint;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    //void Update()
    //{
    //    Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit mHit;
    //    if (Physics.Raycast(mRay, out mHit))
    //    {
    //        if (mHit.collider.gameObject.tag == "Terrain")
    //        {
    //            mTargetPos = new Vector3(mHit.point.x, mHit.point.y + 0.5f, mHit.point.z);
    //            shapeTransform.transform.LookAt(mTargetPos);
    //        }
    //    }
    //    if (Input.GetMouseButton(0))
    //    {
    //        if (attackTime > 0)
    //        {
    //            attackTime -= Time.deltaTime;
    //        }
    //        if (attackTime < 0)
    //        {
    //            attackTime = 0;
    //        }
    //        if (attackTime == 0)
    //        {
    //            Ray shotRay = new Ray(mTargetPos, priringPoint.position);
    //            Debug.DrawLine(mTargetPos, priringPoint.position, Color.yellow);
    //            RaycastHit shotHit;
    //            if(Physics.Raycast(shotRay,out shotHit))
    //            {
    //                if (shotHit.collider.gameObject.tag == "Enemy")
    //                {
    //                    EnemyAI enemy = (EnemyAI)shotHit.collider.GetComponent("EnemyAI");
    //                    enemy.AddjustCurrentHealth(-10);
    //                }
    //            }
    //            attackTime = coolDown;
    //        }
    //    }
    //}
}
