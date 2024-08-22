using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// вешаем на объект со скриптом компонент charactercontroller
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //Характеристики персонажа
    [SerializeField] private MovementCharacteristics characteristics;
    //камера следящая за ним
    [SerializeField] private new Transform camera;
    // джойстик для мобильного управления
    [SerializeField] private FixedJoystick joystick;
    // Canvas с элементами мобильного управления
    [SerializeField] private GameObject mobilecontrol;
    //бар для отображения текущего здоровья
    [SerializeField] private Image bar;
    //текущее хп
    [SerializeField] private float Current_HP;
    //текст отображающий количество монет
    [SerializeField] public Text CoinText;
    //аватар игрока
    [SerializeField] private GameObject avatar;    
    // аниматор аватара
    public Animator animator;
    
    // значения по осям от элементом управления для расчёта вектора движения
    private float vertical, horizontal;
    // контролер персонажа
    private CharacterController controller;
    // вектор направления движения на контроллере
    private Vector2 direction;
    // вектор направления движения
    private Vector3 MoveDirection;
    // вектор направления взгляда
    private Quaternion look;
    // точка в которую должен смотреть персонаж
    private Vector3 TargetRotate => camera.forward * DIstance_OFFSET_CAMERA;

    //включение состояния спокойствия
    private bool Idle => (direction.x == 0 && direction.y == 0);
    private bool IdleMobile => (joystick.Horizontal == 0 && joystick.Vertical == 0);
    // количество монет
    public int coins;

    // дистанция между игроком и камерой
    private const float DIstance_OFFSET_CAMERA = 5f;
    //переменная отвечающая за то, жив ли игрок
    public bool IsAlive = true;
    //переменная отвечающая за включение неуязвимости у игрока
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

        //сохранение позиции аватара относительно игрока
        avatar.transform.rotation = transform.rotation;
        Vector3 playertransform = transform.position;
        playertransform.y -= 1;
        avatar.transform.position = playertransform;



        //отображение хп игрока на хп-баре
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
        //рассчет гравитации
        MoveDirection.y -= characteristics.gravity * Time.fixedDeltaTime;
    }

    //получение данных о нажатых кнопка передвижения на клавиатуре + мышке и геймпаде
    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    // функция прыжка 
    public void Jump()
    {
        if (controller.isGrounded)
        {
            animator.SetTrigger("Jump");
            MoveDirection.y = characteristics.JumpForce;
        }

    }

    //логика перемещения
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
            #region Мобильное управление
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
            MoveDirection = transform.TransformDirection(horizontal, MoveDirection.y, vertical);
            #endregion

        }
        else
        {
            #region ПК и геймпад управление
            horizontal = direction.x;
            vertical = direction.y;
            MoveDirection = transform.TransformDirection(horizontal, MoveDirection.y, vertical);
            #endregion
        }

        controller.Move(MoveDirection * Speed * Time.deltaTime);
    }

    //вращение персонажа
    private void Rotate()
    {
        //если игрок в состоянии покоя он, не поворачивается за камерой
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

        //// логика вращения за камерой
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

    //получение урона
    public IEnumerator TakeDamage(float damage)
    {
        Current_HP -= damage;
        // включается неуязвимость
        IsImmortal = true;
        // период неуязвимости
        yield return new WaitForSeconds(1);
        IsImmortal = false;

    }

    //Проигрование анимаций движения
    private void PlayAnimation()
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }

    //проигрование анимации смерти и отключение управления
    private void PlayDeath()
    {
        IsAlive = false;
        animator.SetTrigger("Death");
    }

    // энумиратор типа управления
    public enum ControlMode { PC, Mobile, Gamepad };

    public ControlMode mode = ControlMode.PC;

    // функции переключения типа управления
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
