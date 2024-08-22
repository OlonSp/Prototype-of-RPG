using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUi : MonoBehaviour
{
    // текущее хп
    [SerializeField] float current_hp;
    // цель дл€ атаки - игрок
    [SerializeField] public GameObject target;
    //характеристики врага
    [SerializeField] MovementCharacteristics characteristics;
    //радиус реагировани€ на цель
    [SerializeField] private float radius = 10f;
    //радиус атаки
    [SerializeField] private float AttackDist = 1f;
    //»скусственный интеллект
    NavMeshAgent nav;
    //дистанци€ между врагом и игроком 
    public float dist;
    //аниматор врага
    private Animator anim;
    //урон
    public float damage = 100f;
    //переменна€ котора€ провер€ет жив ли враг
    public bool isAlive = true;
    //награда за убийство
    public int Reward = 5;


    
    // Start is called before the first frame update
    void Start()
    {
        current_hp = characteristics.MaxHP;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //если враг мЄртв логика отключаетс€
        if (!isAlive)
            return;

        //рассчЄт дистанции
        dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist > radius)
        {
            nav.enabled = false;
            anim.SetTrigger("Idle");
        }
        else if(dist < radius && dist > AttackDist)
        {
            nav.enabled = true;
            nav.SetDestination(target.transform.position);
            anim.SetTrigger("Run");
        }
        else if(dist <= AttackDist) //если дистанци€ между врагом и игроком достаточно маленька€ он атакует
        {
            anim.SetTrigger("Attack");

            nav.enabled = false;
        }
        
        if(current_hp <= 0)
        {
            StartCoroutine(Death());
        }

        

    }

    // получение урона
    public void TakeDamage(float damage)
    {
        current_hp -= damage;
    }

    //смерть
    private IEnumerator Death()
    {
        Debug.Log("Kill");
        isAlive = false;
        anim.SetTrigger("Death");

        //добавление денег игрку за убийство моба
        target.GetComponent<PlayerMovement>().coins += Reward;
        PlayerPrefs.SetInt("Coins", target.GetComponent<PlayerMovement>().coins);
        //отображение денег на экране
        target.GetComponent<PlayerMovement>().CoinText.text = target.GetComponent<PlayerMovement>().coins.ToString();

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
