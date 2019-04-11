using UnityEngine;

public class CameraView : MonoBehaviour
{
    [Header("----- Camera -----")]
    public float mSpeed = 50;
    //视野缩放的速率
    public float viewValue = 50;
    //摄像机移动的速率
    public float moveSpeed = 1f;
    //摄像机旋转的速率
    public float rotationSpeed = 5;
    //越大抬得越高
    public float camUpAngle = 30;
    //越小抬得越低
    public float camDownAngle = -30;
    private float camEuler;

    //俯视位置
    private Vector3 overLook = new Vector3(-32f, 52, -49);
    private Vector3 overEuler = new Vector3(50f, 40, 0);

    private CharacterController contro;

    [Header("----- Robot机器人 -----")]
    //机器人
    public Transform target;
    //跟随
    public float height = 15f;
    public float distance = 3f;
    public Vector3 offestRobot = new Vector3(0, 0.5f, 0);

    float currentRotationAngle;
    float currentHeight;
    float wantedRotationAngle;
    float wantedHeight;



    [Header("----- Player人物视角 -----")]
    //人物控件
    public GameObject player;
    //相机位置
    public Transform personCam;
    //抬头
    public float personUpAngel = 40;
    //低头
    public float personDownAngel = -40;
    private GameObject cameraHandle;
    private GameObject model;
    private float tempEulerx;
    private Vector3 cameraDampVerlocity;

    [Header("----- Mouse鼠标信息 -----")]
    private float deltaX;
    private float deltaY;


    public Vector3 moveDirection = Vector3.zero;

    public Texture2D handTexture;
    public Texture2D rotateTexture;

    private float camAndObjDistance = 20;
    public GameObject obj;
    public GameObject xyzObj;
    public Vector3 lastCameraPosition;
    public Vector3 lastObjScale;





    void Awake()
    {
        contro = GetComponent<CharacterController>();
        //model = player.GetComponent<ActorControl>().model;
        //cameraHandle = personCam.parent.gameObject;
        transform.position = overLook;
        transform.localEulerAngles = overEuler;
        lastCameraPosition = Camera.main.transform.position;
        lastObjScale = xyzObj.transform.localScale;
    }


    private void Update()
    {
        if (Camera.main.transform.position != lastCameraPosition)
        {
            xyzObj.transform.localScale = lastObjScale * Vector3.Distance(Camera.main.transform.position, obj.transform.position) / camAndObjDistance;
            //Debug.Log(Vector3.Distance(Camera.main.transform.position, obj.transform.position) / camAndObjDistance);
        }

    }

    void LateUpdate()
    {
        //鼠标偏移量
        deltaX = Input.GetAxis("Mouse X");
        deltaY = Input.GetAxis("Mouse Y");

        FreelView();
    }

    /// <summary>
    /// 自由视角
    /// </summary>
    private void FreelView()
    {
        //if (UIManager.CheckGuiRaycastObjects()) return;

        //缩放
        viewValue = Mathf.Clamp(transform.position.y, 0.35f, 5);
        Vector3 zoommentp = new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * viewValue);
        zoommentp = transform.TransformDirection(zoommentp);//改为全局坐标
        contro.Move(zoommentp);

        //鼠标控制平移
        if (Input.GetMouseButton(2))
        {
            Cursor.SetCursor(handTexture, Vector2.zero, CursorMode.Auto);
            moveSpeed = Mathf.Clamp((transform.position.y - 5) * 0.1f, 0.25f, 4);
            Vector3 movement = new Vector3(deltaX * -moveSpeed, deltaY * -moveSpeed, 0);
            //将对角移动的速度限制为和沿着轴移动的速度一样
            movement = Vector3.ClampMagnitude(movement, moveSpeed);
            //把movement向量从本地变换为全局坐标 
            movement = transform.TransformDirection(movement);
            contro.Move(movement);

        }

        //键盘控制移动
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(h * mSpeed, 0, v * mSpeed);
        moveDirection = transform.TransformDirection(moveDirection);//将局部坐标系转换成全局坐标系。
        moveDirection *= moveSpeed;
        if (Input.GetKey(KeyCode.Q))
        {
            moveDirection.y += 1000 * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            moveDirection.y -= 1000 * moveSpeed * Time.deltaTime;
        }
        //moveDirection.y -= gravity * Time.deltaTime;
        contro.Move(moveDirection * Time.deltaTime);



        //旋转
        if (Input.GetMouseButton(1))
        {
            Cursor.SetCursor(rotateTexture, Vector2.zero, CursorMode.Auto);
            Vector3 neweuler = transform.eulerAngles;
            neweuler.x += -(deltaY * rotationSpeed);
            neweuler.x = ClampAngle(neweuler.x, 0, 80);
            neweuler.y += (deltaX * rotationSpeed);
            transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, neweuler, ref cameraDampVerlocity, 0.2f);
        }



        #region 按鼠标中键旋转
        //if (Input.GetMouseButton(2))
        //{
        //    moveSpeed = Mathf.Clamp((transform.position.y - 5) * 0.1f, 0.25f, 4);
        //    Vector3 movement = new Vector3(deltaX * -moveSpeed, 0, 0);
        //    movement = Vector3.ClampMagnitude(movement, moveSpeed);
        //    movement = transform.TransformDirection(movement);
        //    contro.Move(movement);
        //    Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        //    Vector3 neweuler = transform.eulerAngles;
        //    neweuler.y += (deltaX * rotationSpeed);
        //    transform.eulerAngles = neweuler;
        //}
        #endregion



        //换鼠标图标
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    /// <summary>
    /// 全景俯视
    /// </summary>
    void OverLook()
    {
        transform.position = Vector3.Lerp(transform.position, overLook, 4 * Time.deltaTime);
        transform.localEulerAngles = overEuler;
        if (Vector3.Distance(overLook, transform.position) < 0.5f)
        {
            //ClientToServer.Instance.viewstate = ClientToServer.ViewState.FreeView;
        }
    }

    /// <summary>
    /// 机器人跟随视角
    /// </summary>
    private void RobotThirdView()
    {
        currentHeight = transform.position.y;
        currentRotationAngle = transform.eulerAngles.y;
        wantedRotationAngle = target.eulerAngles.y;
        wantedHeight = target.position.y;
        if (Input.GetMouseButton(0))
        {
            height = Mathf.Clamp(height + deltaX, 1, 15);
        }
        if (Input.GetMouseButton(1))
        {
            distance = Mathf.Clamp(distance + deltaY, 0.2f, 8f);
        }
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, 3 * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight + height, 2 * Time.deltaTime);
        //相机当前的旋转
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        //相机的实际位置
        Vector3 pos = target.position - currentRotation * Vector3.forward * distance;
        //相机实际位置的高度
        pos.y = currentHeight;
        transform.position = pos;
        transform.LookAt(target.position + offestRobot);
    }

    /// <summary>
    /// 第三人称人物视角
    /// </summary>
    private void PersonView()
    {
        Vector3 temoModelEuler = model.transform.eulerAngles;
        if (Input.GetMouseButton(1))
        {
            player.transform.Rotate(Vector3.up, deltaX * rotationSpeed * 1.2f);
            tempEulerx -= deltaY * rotationSpeed;
            tempEulerx = Mathf.Clamp(tempEulerx, personDownAngel, personUpAngel);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerx, 0, 0);
        }
        model.transform.eulerAngles = temoModelEuler;
        transform.position = Vector3.SmoothDamp(transform.position, personCam.position, ref cameraDampVerlocity, 0.0f);
        transform.eulerAngles = personCam.eulerAngles;
    }

    float ClampAngle(float angle, float minAngle, float maxAgnle)
    {
        if (angle <= -360)
            angle += 360;
        if (angle >= 360)
            angle -= 360;

        return Mathf.Clamp(angle, minAngle, maxAgnle);
    }

    private void OnGUI()
    {
        //Vector3 mousePos = Input.mousePosition;
        //Rect pos = new Rect(mousePos.x - crosshairTexture.width * 0.5f, Screen.height - mousePos.y - crosshairTexture.height * 0.5f, crosshairTexture.width, crosshairTexture.height);
        //GUI.DrawTexture(pos, crosshairTexture);
    }







}