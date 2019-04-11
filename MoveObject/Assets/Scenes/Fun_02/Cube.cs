using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tizen;

public class Cube : MonoBehaviour
{
    bool isShowTip;
    public bool WindowShow = false;

    // Use this for initialization
    void Start()
    {
        isShowTip = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseEnter()
    {
        isShowTip = true;
    }

    private void OnMouseExit()
    {
        isShowTip = false;
    }

    private void OnGUI()
    {
        if (isShowTip)
        {
            GUIStyle style1 = new GUIStyle();
            style1.fontSize = 30;
            style1.normal.textColor = Color.red;
            GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 400, 50), "Cube", style1);
        }
        if (WindowShow)
        {
            GUI.Window(0, new Rect(30, 30, 200, 100), MyWindow, "Cube");
        }
    }

    void MyWindow(int WindowID)
    {
        GUILayout.Label("你想写入的内容");
    }

    private void OnMouseDown()
    {
        if (WindowShow)
        {
            WindowShow = false;
        }
        else
        {
            WindowShow = true;
        }
    }
}
