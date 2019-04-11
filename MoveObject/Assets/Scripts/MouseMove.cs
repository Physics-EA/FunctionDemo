using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 坐标轴
/// </summary>
public class MouseMove : MonoBehaviour
{

    private Material matBackUp;
    public Material hoverMat;    

    public Camera sceneCam;

    public Transform xrot;
    public Transform yrot;
    public Transform zrot;

    Vector3 tpos;
    Vector3 npos;
    Vector3 rot;

   
    void Start()
    {
        matBackUp = this.GetComponent<MeshRenderer>().material;
    }

    void OnMouseEnter()
    {
        this.GetComponent<MeshRenderer>().sharedMaterial = hoverMat;       
    }

    void OnMouseExit()
    {
        this.GetComponent<MeshRenderer>().sharedMaterial = matBackUp;      
    }
 

}
