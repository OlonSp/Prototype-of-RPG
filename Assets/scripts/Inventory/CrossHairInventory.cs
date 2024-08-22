using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CrossHairInventory : MonoBehaviour
{
    //скорость движения курсора в инвенторе 
    public float MovementSpeed = 150f;
    //направление движения курсора в инвенторе 
    Vector2 direction;

    //читаем движение стика
    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    //двигаем курсор
    void Update()
    {
        GetComponent<RectTransform>().localPosition += new Vector3(direction.x,direction.y) * Time.deltaTime * MovementSpeed;
    }
}
