using UnityEngine;
using System.Collections;



/// <summary>
/// 链接：https://blog.csdn.net/milk713785/article/details/52530487
/// </summary>
public class ControlCamera : MonoBehaviour
{

    private Vector2 lastTouch1Postion = Vector2.zero;
    private Vector2 lastTouch2Postion = Vector2.zero;
    private float lastDistance = 60f;
    private float touchDistance;
    private float touch1Distance;
    private float touch2Distance;
    private float fieldOfView = DefaultFieldOfView;
    public float ScaleDump = 0.5f;
    public float RotateDump = 0.5f;
    public float TranslateDump = 0.5f;

    Vector3 lastMouseButton1Position = Vector3.zero;
    Vector3 lastMouseButton2Position = Vector3.zero;
    Vector3 lastMouseButton3Position = Vector3.zero;
    private Touch touch1;
    private Touch touch2;

    private Camera thisCamera;
    public Camera DefaultCamera;
    public GameObject DefaultCameraPoint;
    public GameObject lookAtObject;
    public bool showLookAtObject = false;

    private static Color Transparent_Color_Half = new Color(1.0f, 1.0f, 1.0f, 0.4f);
    private static Color Transparent_Color_Full = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    private static float DefaultFieldOfView = 60f;

    void Start()
    {
        thisCamera = this.GetComponent<Camera>();
        lookAtObject.GetComponent<SpriteRenderer>().color = Transparent_Color_Full;
        resetCamera();
    }

    private void resetCamera()
    {
        thisCamera.transform.position = DefaultCamera.transform.position;
        thisCamera.transform.rotation = DefaultCamera.transform.rotation;
        lookAtObject.transform.position = DefaultCameraPoint.transform.position;
        lookAtObject.transform.rotation = DefaultCameraPoint.transform.rotation;
        thisCamera.transform.LookAt(lookAtObject.transform);
        lookAtObject.transform.rotation = thisCamera.transform.rotation;
        fieldOfView = DefaultFieldOfView;
        thisCamera.fieldOfView = fieldOfView;
    }

    private void DoShowLookAtObject()
    {
        if (showLookAtObject)
        {
            lookAtObject.GetComponent<SpriteRenderer>().color = Transparent_Color_Half;
        }
        else
        {
            lookAtObject.GetComponent<SpriteRenderer>().color = Transparent_Color_Full;
        }
    }

    private void DoNothing()
    {
        lookAtObject.GetComponent<SpriteRenderer>().color = Transparent_Color_Full;
    }

    private void Rotate(float rangeRight, float rangeUp)
    {
        this.transform.RotateAround(lookAtObject.transform.position, thisCamera.transform.right, RotateDump * rangeRight);
        this.transform.RotateAround(lookAtObject.transform.position, thisCamera.transform.up, RotateDump * rangeUp);
        lookAtObject.transform.rotation = thisCamera.transform.rotation;
    }

    private void ScaleChange(float range)
    {
        fieldOfView += range * ScaleDump;
        fieldOfView = Mathf.Clamp(fieldOfView, 1f, 120f);
        thisCamera.fieldOfView = fieldOfView;
    }

    private void TransformUp(float range)
    {
        lookAtObject.transform.Translate(thisCamera.transform.up * -range * TranslateDump);
        thisCamera.transform.Translate(thisCamera.transform.up * -range * TranslateDump);
    }

    private void TransformRight(float range)
    {
        lookAtObject.transform.Translate(thisCamera.transform.right * range * TranslateDump);
        thisCamera.transform.Translate(thisCamera.transform.right * range * TranslateDump);
    }

    private void TransformForward(float range)
    {
        lookAtObject.transform.Translate(thisCamera.transform.forward * range * ScaleDump);
        thisCamera.transform.Translate(thisCamera.transform.forward * range * ScaleDump);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 20, Screen.height / 20, Screen.width / 20, Screen.height / 20), "Exit"))
        {
            Application.Quit();
        }
        if (GUI.Button(new Rect(Screen.width * 3 / 20, Screen.height / 20, Screen.width / 20, Screen.height / 20), "Reset"))
        {
            resetCamera();
        }
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.touchCount == 1)
            {
                DoShowLookAtObject();
                touch1 = Input.GetTouch(0);
                if (touch1.phase == TouchPhase.Moved)
                {
                    Rotate((lastTouch1Postion.y - touch1.position.y) / 30f, (lastTouch1Postion.x - touch1.position.x) / 30f);
                }
                else
                {
                    lastTouch1Postion = touch1.position;
                }
            }
            else if (Input.touchCount == 2)
            {
                DoShowLookAtObject();
                touch1 = Input.GetTouch(0);
                touch2 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    lastTouch1Postion = touch1.position;
                    lastTouch2Postion = touch2.position;
                    lastDistance = Vector2.Distance(touch1.position, touch2.position);
                }
                else if (touch1.phase == TouchPhase.Stationary && touch2.phase == TouchPhase.Moved)
                {
                    touch1Distance = Vector2.Distance(touch1.position, lastTouch1Postion);
                    touch2Distance = Vector2.Distance(touch2.position, lastTouch2Postion);
                    touchDistance = Vector2.Distance(touch1.position, touch2.position);
                    if (Mathf.Abs(lastTouch2Postion.x - touch2.position.x) > Mathf.Abs(lastTouch2Postion.y - touch2.position.y))
                    {
                        ScaleChange((lastTouch2Postion.x - touch2.position.x) / 20f);
                    }
                    else
                    {
                        TransformForward((lastTouch2Postion.y - touch2.position.y) / 200f);
                    }
                }
                else if (touch2.phase == TouchPhase.Stationary && touch1.phase == TouchPhase.Moved)
                {
                    if (Mathf.Abs(lastTouch1Postion.x - touch1.position.x) > Mathf.Abs(lastTouch1Postion.y - touch1.position.y))
                    {
                        ScaleChange((lastTouch1Postion.x - touch1.position.x) / 20f);
                    }
                    else
                    {
                        TransformForward((lastTouch1Postion.y - touch1.position.y) / 200f);
                    }
                }
                else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
                {
                    touch1Distance = Vector2.Distance(touch1.position, lastTouch1Postion);
                    touch2Distance = Vector2.Distance(touch2.position, lastTouch2Postion);
                    touchDistance = Vector2.Distance(touch1.position, touch2.position);

                    if (touch1Distance < 0.3f)
                    {
                        if (Mathf.Abs(lastTouch2Postion.x - touch2.position.x) > Mathf.Abs(lastTouch2Postion.y - touch2.position.y))
                        {
                            ScaleChange((lastTouch2Postion.x - touch2.position.x) / 20f);
                        }
                        else
                        {
                            TransformForward((lastTouch2Postion.y - touch2.position.y) / 200f);
                        }
                    }
                    else if (touch2Distance < 0.3f)
                    {
                        if (Mathf.Abs(lastTouch1Postion.x - touch1.position.x) > Mathf.Abs(lastTouch1Postion.y - touch1.position.y))
                        {
                            ScaleChange((lastTouch1Postion.x - touch1.position.x) / 20f);
                        }
                        else
                        {
                            TransformForward((lastTouch1Postion.y - touch1.position.y) / 200f);
                        }
                    }
                    else if (Mathf.Abs(touchDistance - lastDistance) < 1f)
                    {
                        TransformRight(Mathf.Min((lastTouch1Postion.x - touch1.position.x), (lastTouch2Postion.x - touch2.position.x)) / 20f);
                        TransformUp(Mathf.Min((lastTouch1Postion.y - touch1.position.y), (lastTouch2Postion.y - touch2.position.y)) / 20f);
                    }
                    else
                    {
                        ScaleChange((touchDistance - lastDistance) / -5f);
                    }
                    lastTouch1Postion = touch1.position;
                    lastTouch2Postion = touch2.position;
                    lastDistance = touchDistance;
                }
                else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled || touch2.phase == TouchPhase.Canceled)
                {
                    lastTouch1Postion = touch1.position;
                    lastTouch2Postion = touch2.position;
                    lastDistance = Vector2.Distance(touch1.position, touch2.position);
                }
            }
            else
            {
                DoNothing();
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                if (lastMouseButton1Position != Vector3.zero)
                {
                    DoShowLookAtObject();
                    Rotate(0.5f * (Input.mousePosition.y - lastMouseButton1Position.y), 0.5f * (Input.mousePosition.x - lastMouseButton1Position.x));
                }
                lastMouseButton1Position = Input.mousePosition;
            }
            else if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
            {
                if (lastMouseButton2Position != Vector3.zero)
                {
                    DoShowLookAtObject();
                    if (Mathf.Abs(lastMouseButton2Position.x - Input.mousePosition.x) > Mathf.Abs(lastMouseButton2Position.y - Input.mousePosition.y))
                    {
                        touchDistance = lastMouseButton2Position.x - Input.mousePosition.x;
                        ScaleChange(touchDistance / 2f);
                    }
                    else
                    {
                        TransformUp((lastMouseButton2Position.y - Input.mousePosition.y) * 0.1f);
                    }
                }
                lastMouseButton2Position = Input.mousePosition;
            }
            else if (Input.GetMouseButton(2) || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
            {
                if (lastMouseButton3Position != Vector3.zero)
                {
                    DoShowLookAtObject();
                    if (Mathf.Abs(lastMouseButton3Position.x - Input.mousePosition.x) > Mathf.Abs(lastMouseButton3Position.y - Input.mousePosition.y))
                    {
                        TransformRight((lastMouseButton3Position.x - Input.mousePosition.x) * 0.1f);
                    }
                    else
                    {
                        TransformForward((lastMouseButton3Position.y - Input.mousePosition.y) * 0.3f);
                    }
                }
                lastMouseButton3Position = Input.mousePosition;
            }
            else if (Input.mouseScrollDelta.y != 0)
            {
                DoShowLookAtObject();
                TransformForward(Input.mouseScrollDelta.y * -0.8f);
            }
            else
            {
                lastMouseButton1Position = Vector3.zero;
                lastMouseButton2Position = Vector3.zero;
                lastMouseButton3Position = Vector3.zero;
                DoNothing();
            }
        }
    }
}