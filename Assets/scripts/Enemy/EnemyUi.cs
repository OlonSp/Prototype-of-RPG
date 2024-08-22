using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUi : MonoBehaviour
{
    // ������� ��
    [SerializeField] float current_hp;
    // ���� ��� ����� - �����
    [SerializeField] public GameObject target;
    //�������������� �����
    [SerializeField] MovementCharacteristics characteristics;
    //������ ������������ �� ����
    [SerializeField] private float radius = 10f;
    //������ �����
    [SerializeField] private float AttackDist = 1f;
    //������������� ���������
    NavMeshAgent nav;
    //��������� ����� ������ � ������� 
    public float dist;
    //�������� �����
    private Animator anim;
    //����
    public float damage = 100f;
    //���������� ������� ��������� ��� �� ����
    public bool isAlive = true;
    //������� �� ��������
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
        //���� ���� ���� ������ �����������
        if (!isAlive)
            return;

        //������� ���������
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
        else if(dist <= AttackDist) //���� ��������� ����� ������ � ������� ���������� ��������� �� �������
        {
            anim.SetTrigger("Attack");

            nav.enabled = false;
        }
        
        if(current_hp <= 0)
        {
            StartCoroutine(Death());
        }

        

    }

    // ��������� �����
    public void TakeDamage(float damage)
    {
        current_hp -= damage;
    }

    //������
    private IEnumerator Death()
    {
        Debug.Log("Kill");
        isAlive = false;
        anim.SetTrigger("Death");

        //���������� ����� ����� �� �������� ����
        target.GetComponent<PlayerMovement>().coins += Reward;
        PlayerPrefs.SetInt("Coins", target.GetComponent<PlayerMovement>().coins);
        //����������� ����� �� ������
        target.GetComponent<PlayerMovement>().CoinText.text = target.GetComponent<PlayerMovement>().coins.ToString();

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
