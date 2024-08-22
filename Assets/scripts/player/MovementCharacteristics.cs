using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �������� ����� � ���� 
[CreateAssetMenu(fileName = "Characteristics", menuName = "Movement/MovementCharacteristics", order = 1)]
// �����-���������, ������� ������ ����� � ���������������� ������������(����� ��������)
public class MovementCharacteristics : ScriptableObject
{
    //������ �� ������ ���� ������� �� ������
    [SerializeField] private bool _visibleCursor;
    //�������� ������������
    [SerializeField] private float _movementSpeed = 1f;
    // �������� ��������
    [SerializeField] private float _angularSpeed = 150f;
    // ����������
    [SerializeField] private float _gravity = 9.8f;
    //������������ ��
    [SerializeField] private float _MaxHP;
    //���� ������
    [SerializeField] private float _JumpForce;

    // ��������� ����������, ��� ������������� � ������ �������� 
    public bool VisibleCursor => _visibleCursor;
    public float MovementSpeed => _movementSpeed;
    public float AngularSpeed => _angularSpeed;
    public float gravity => _gravity;
    public float MaxHP => _MaxHP;
    public float JumpForce => _JumpForce;
}
