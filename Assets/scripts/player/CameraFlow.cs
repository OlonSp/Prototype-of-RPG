using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFlow : MonoBehaviour
{
    // ���������������� ����
    [Tooltip("���������������� ����")]
    [SerializeField] [Range(0f, 5f)] private float angularSpeed = 1f;
    [Tooltip("����� �� ������� ������ ������")]
    // ����� �� ������� ������ ������
    [SerializeField] private Transform target;

    // ���������� ������������ ����� ����� ��� ����������
    private PlayerMovement controlmode;

    private Vector2 RotateCamera;

    //����� �������� ������
    private float angleY;
    private float rotationX = 0f;
    // ����������� �������� ������ �� ���������
    private float minVert = -45f;
    private float maxVert = 45f;

    // ��������� ������� ������� ��� ������
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
            #region �� ���������� �������
            // ������� �������� ������ �� ���������
            if (Input.GetAxis("Mouse X") > 0) angleY += angularSpeed;
            if (Input.GetAxis("Mouse X") < 0) angleY -= angularSpeed;
            // ������� �������� ������ �� �����������
            rotationX -= Input.GetAxis("Mouse Y") * angularSpeed;
            rotationX = Mathf.Clamp(rotationX, minVert, maxVert);
            
            #endregion
        }
        else if (controlmode.mode == PlayerMovement.ControlMode.Mobile)
        {
            #region ��������� ���������� �������
            if (Input.GetMouseButtonDown(0))
            {
                // ����������� ������� ������� �� ������
                if(Input.mousePosition.x >= Screen.width / 2  && Input.mousePosition.y >= Screen.width / 4)
                {
                    startPos = Input.mousePosition;
                    Debug.Log(startPos);
                }
                    
            }
            else if (Input.GetMouseButton(0))
            {
                // ������ �������� ���������� � ��������������� ������ � ������ ������
                Vector2 mousepos = Input.mousePosition;

                if(mousepos.x >= Screen.width / 2 && Input.mousePosition.y >= Screen.width / 4)
                {
                    // ������� ������� ��� ��������� ��������� ������
                    rotationX = mousepos.y - startPos.y;
                    rotationX = Mathf.Clamp(rotationX, minVert, maxVert);
                    angleY = mousepos.x - startPos.x;
                }
                
            }
            #endregion
        }
        else if (controlmode.mode == PlayerMovement.ControlMode.Gamepad)
        {
            #region ������� ���������� �������
            //������ �������� ������ �� �����������
            rotationX += RotateCamera.y * angularSpeed;
            rotationX = Mathf.Clamp(rotationX, minVert, maxVert);
            //������ �������� ������ �� ���������
            angleY += RotateCamera.x * angularSpeed;
            #endregion
        }

        // �������� ������ �� ������    
        transform.position = target.transform.position;
        //������� ������
        transform.rotation = Quaternion.Euler(rotationX, angleY, 0);
    }

    
}
