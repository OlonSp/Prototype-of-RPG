using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// ������ �� ������ �� �������� ��������� charactercontroller
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //�������������� ���������
    [SerializeField] private MovementCharacteristics characteristics;
    //������ �������� �� ���
    [SerializeField] private new Transform camera;
    // �������� ��� ���������� ����������
    [SerializeField] private FixedJoystick joystick;
    // Canvas � ���������� ���������� ����������
    [SerializeField] private GameObject mobilecontrol;
    //��� ��� ����������� �������� ��������
    [SerializeField] private Image bar;
    //������� ��
    [SerializeField] private float Current_HP;
    //����� ������������ ���������� �����
    [SerializeField] public Text CoinText;
    //������ ������
    [SerializeField] private GameObject avatar;    
    // �������� �������
    public Animator animator;
    
    // �������� �� ���� �� ��������� ���������� ��� ������� ������� ��������
    private float vertical, horizontal;
    // ��������� ���������
    private CharacterController controller;
    // ������ ����������� �������� �� �����������
    private Vector2 direction;
    // ������ ����������� ��������
    private Vector3 MoveDirection;
    // ������ ����������� �������
    private Quaternion look;
    // ����� � ������� ������ �������� ��������
    private Vector3 TargetRotate => camera.forward * DIstance_OFFSET_CAMERA;

    //��������� ��������� �����������
    private bool Idle => (direction.x == 0 && direction.y == 0);
    private bool IdleMobile => (joystick.Horizontal == 0 && joystick.Vertical == 0);
    // ���������� �����
    public int coins;

    // ��������� ����� ������� � �������
    private const float DIstance_OFFSET_CAMERA = 5f;
    //���������� ���������� �� ��, ��� �� �����
    public bool IsAlive = true;
    //���������� ���������� �� ��������� ������������ � ������
    private bool IsImmortal = false;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.visible = characteristics.VisibleCursor;

        Current_HP = characteristics.MaxHP;
        animator = avatar.GetComponent<Animator>();
        coins = PlayerPrefs.GetInt("Coins");
        CoinText.text = coins.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        //���������� ������� ������� ������������ ������
        avatar.transform.rotation = transform.rotation;
        Vector3 playertransform = transform.position;
        playertransform.y -= 1;
        avatar.transform.position = playertransform;



        //����������� �� ������ �� ��-����
        bar.fillAmount = Current_HP / characteristics.MaxHP;
        if (IsAlive)
        {
            Rotate();
            Movement();
            if(Current_HP <= 0)
            {
                PlayDeath();
            }
        }
        
    }

    private void FixedUpdate()
    {
        //������� ����������
        MoveDirection.y -= characteristics.gravity * Time.fixedDeltaTime;
    }

    //��������� ������ � ������� ������ ������������ �� ���������� + ����� � ��������
    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    // ������� ������ 
    public void Jump()
    {
        if (controller.isGrounded)
        {
            animator.SetTrigger("Jump");
            MoveDirection.y = characteristics.JumpForce;
        }

    }

    //������ �����������
    private void Movement()
    {
        float Speed = characteristics.MovementSpeed;
        if (!controller.isGrounded)
        {
            Speed /= 2;
            
        }
        else
        {
            PlayAnimation();
        }
        
        if (mode == ControlMode.Mobile)
        {
            #region ��������� ����������
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
            MoveDirection = transform.TransformDirection(horizontal, MoveDirection.y, vertical);
            #endregion

        }
        else
        {
            #region �� � ������� ����������
            horizontal = direction.x;
            vertical = direction.y;
            MoveDirection = transform.TransformDirection(horizontal, MoveDirection.y, vertical);
            #endregion
        }

        controller.Move(MoveDirection * Speed * Time.deltaTime);
    }

    //�������� ���������
    private void Rotate()
    {
        //���� ����� � ��������� ����� ��, �� �������������� �� �������
        if (mode == ControlMode.PC || mode == ControlMode.Gamepad)
        {
            if (Idle)
            {
                return;
            }
            
        }
        if (mode == ControlMode.Mobile)
        {
            if (IdleMobile) 
            {
                return;
            } 
        }

        //// ������ �������� �� �������
        Vector3 target = TargetRotate;
        target.y = 0f;

        look = Quaternion.LookRotation(target);
        float speed = characteristics.AngularSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, look, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsAlive)
            if(other.tag == "AttackTrigger")
            {
                if(!IsImmortal)
                    StartCoroutine(TakeDamage(other.GetComponentInParent<EnemyUi>().damage));
            }
    }

    //��������� �����
    public IEnumerator TakeDamage(float damage)
    {
        Current_HP -= damage;
        // ���������� ������������
        IsImmortal = true;
        // ������ ������������
        yield return new WaitForSeconds(1);
        IsImmortal = false;

    }

    //������������ �������� ��������
    private void PlayAnimation()
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }

    //������������ �������� ������ � ���������� ����������
    private void PlayDeath()
    {
        IsAlive = false;
        animator.SetTrigger("Death");
    }

    // ���������� ���� ����������
    public enum ControlMode { PC, Mobile, Gamepad };

    public ControlMode mode = ControlMode.PC;

    // ������� ������������ ���� ����������
    public void OnPC()
    {
        if (mode != ControlMode.PC)
        {
            if( mode == ControlMode.Mobile)
                mobilecontrol.SetActive(false);

            mode = ControlMode.PC;
        }
            
    }

    public void OnMobile()
    {
        if (mode != ControlMode.Mobile) mode = ControlMode.Mobile;
        mobilecontrol.SetActive(true);
    }

    public void OnGamepad()
    {
        if (mode != ControlMode.Gamepad)
        {
            if (mode == ControlMode.Mobile)
                mobilecontrol.SetActive(false);

            mode = ControlMode.Gamepad;
        }
            
    }
}
