using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFlow : MonoBehaviour
{
    // чувствительность мыши
    [Tooltip("чувствительность мыши")]
    [SerializeField] [Range(0f, 5f)] private float angularSpeed = 1f;
    [Tooltip("игрок за которым следит камера")]
    // игрок за которым следит камера
    [SerializeField] private Transform target;

    // переменна€ определ€юща€ какой сечас тип управлени€
    private PlayerMovement controlmode;

    private Vector2 RotateCamera;

    //уголы поворота камеры
    private float angleY;
    private float rotationX = 0f;
    // ограничение поворота камеры по вертикали
    private float minVert = -45f;
    private float maxVert = 45f;

    // начальна€ позици€ нажати€ дл€ свайпа
    private Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        angleY = transform.position.y;
        controlmode = target.GetComponent<PlayerMovement>();
    }

    public void OnCameraMove(InputAction.CallbackContext context)
    {
        RotateCamera = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlmode.mode == PlayerMovement.ControlMode.PC)
        {
            #region ѕ  управление камерой
            // рассчет поворота камеры по вертикали
            if (Input.GetAxis("Mouse X") > 0) angleY += angularSpeed;
            if (Input.GetAxis("Mouse X") < 0) angleY -= angularSpeed;
            // рассчЄт поворота камеры по горизонтали
            rotationX -= Input.GetAxis("Mouse Y") * angularSpeed;
            rotationX = Mathf.Clamp(rotationX, minVert, maxVert);
            
            #endregion
        }
        else if (controlmode.mode == PlayerMovement.ControlMode.Mobile)
        {
            #region ћобильное управление камерой
            if (Input.GetMouseButtonDown(0))
            {
                // ограничение области нажати€ на экране
                if(Input.mousePosition.x >= Screen.width / 2  && Input.mousePosition.y >= Screen.width / 4)
                {
                    startPos = Input.mousePosition;
                    Debug.Log(startPos);
                }
                    
            }
            else if (Input.GetMouseButton(0))
            {
                // вектор хран€щий информацию о местонахождении пальца в данный момент
                Vector2 mousepos = Input.mousePosition;

                if(mousepos.x >= Screen.width / 2 && Input.mousePosition.y >= Screen.width / 4)
                {
                    // рассчЄт вектора дл€ изменени€ положени€ камеры
                    rotationX = mousepos.y - startPos.y;
                    rotationX = Mathf.Clamp(rotationX, minVert, maxVert);
                    angleY = mousepos.x - startPos.x;
                }
                
            }
            #endregion
        }
        else if (controlmode.mode == PlayerMovement.ControlMode.Gamepad)
        {
            #region √еймпад управление камерой
            //расчЄт поворота камеры по горизонтали
            rotationX += RotateCamera.y * angularSpeed;
            rotationX = Mathf.Clamp(rotationX, minVert, maxVert);
            //расчЄт поворота камеры по вертикали
            angleY += RotateCamera.x * angularSpeed;
            #endregion
        }

        // фиксаци€ камеры на игроке    
        transform.position = target.transform.position;
        //поворот камеры
        transform.rotation = Quaternion.Euler(rotationX, angleY, 0);
    }

    
}
