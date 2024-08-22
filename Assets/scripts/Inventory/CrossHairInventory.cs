using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CrossHairInventory : MonoBehaviour
{
    //�������� �������� ������� � ��������� 
    public float MovementSpeed = 150f;
    //����������� �������� ������� � ��������� 
    Vector2 direction;

    //������ �������� �����
    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    //������� ������
    void Update()
    {
        GetComponent<RectTransform>().localPosition += new Vector3(direction.x,direction.y) * Time.deltaTime * MovementSpeed;
    }
}
