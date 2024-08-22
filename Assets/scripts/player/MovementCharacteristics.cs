using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// кнопка создания формы в меню 
[CreateAssetMenu(fileName = "Characteristics", menuName = "Movement/MovementCharacteristics", order = 1)]
// класс-контейнер, который создаёт форму с характеристиками передвижения(можно изменять)
public class MovementCharacteristics : ScriptableObject
{
    //должен ли курсор быть скрытым на экране
    [SerializeField] private bool _visibleCursor;
    //скорость передвижения
    [SerializeField] private float _movementSpeed = 1f;
    // скорость поворота
    [SerializeField] private float _angularSpeed = 150f;
    // гравитация
    [SerializeField] private float _gravity = 9.8f;
    //Максимальное хп
    [SerializeField] private float _MaxHP;
    //Сила прыжка
    [SerializeField] private float _JumpForce;

    // публичные переменные, для использования в других скриптах 
    public bool VisibleCursor => _visibleCursor;
    public float MovementSpeed => _movementSpeed;
    public float AngularSpeed => _angularSpeed;
    public float gravity => _gravity;
    public float MaxHP => _MaxHP;
    public float JumpForce => _JumpForce;
}
