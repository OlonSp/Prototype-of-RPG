using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUI : MonoBehaviour
{
    // ����� �����
    [SerializeField] private Transform attackPoint;
    // ������ �����
    [SerializeField] private float attackRadius = 0.5f;
    // ����
    [SerializeField] public float attackDamage = 100f;
    // ���� � ������� ��������� ����������
    [SerializeField] private LayerMask enemyLayers;
    // ���� ����������� ��������
    [SerializeField] private LayerMask destractableLayers;
    // ���������� ������������ ����� ����� ��� ����������
    private PlayerMovement controlmode;
    ////����������, ������� ���������� � �������� ����� ������ ����� ��� ���
    private bool IsAttack = false;
    //���������� ������� ���������� � ������ �� �����
    private bool Skill1ColDown = false;

    private void Start()
    {
        controlmode = GetComponent<PlayerMovement>();
    }
    // Update is called once per frame
    void Update()
    {
        if(controlmode.mode == PlayerMovement.ControlMode.PC)
        {
            if (Input.GetMouseButtonDown(0))
                if(controlmode.IsAlive)
                    if (!IsAttack)
                    {
                        StartCoroutine(SimpleAttack());
                    }
                        
        }
            
    }
    //����� ������� ����� ��� �������� � ������
    public void SimplePunch()
    {
        if (controlmode.IsAlive)
            if (!IsAttack)
            {
                StartCoroutine(SimpleAttack());
            }
    }
    //����� 1 ����� ����� ��� �������� � ������
    public void SpecialSkill1()
    {
        if (controlmode.IsAlive)
            if (!IsAttack)
                if (!Skill1ColDown)
                    StartCoroutine(Skill1());

    }

    public IEnumerator SimpleAttack()
    {
        //����� ����� ���������
        IsAttack = true;
        //����� �������� �����
        controlmode.animator.SetTrigger("Attack");

        //������ ���� ������� �����������
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayers);
        
        // ������ ������� ��������� �������� ����
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit");
            enemy.GetComponent<EnemyUi>().TakeDamage(attackDamage);
        }

        //������ ���� ������� ����������� ��������
        Collider[] hitObject = Physics.OverlapSphere(attackPoint.position, attackRadius, destractableLayers);

        // ������ ������� ����������� ������ �������� ����
        foreach (Collider destractableObject in hitObject)
        {
            Debug.Log("Hit");
            destractableObject.GetComponent<WoodAI>().TakeDamage(attackDamage);
        }

        //� ���� ���������� ������� ����� �� ����� ���������
        yield return new WaitForSeconds(1);
        //����� �����������

        IsAttack = false;
    }

    public IEnumerator Skill1()
    {
        IsAttack = true;
        //�������� ����� ������
        Skill1ColDown = true;
        controlmode.animator.SetTrigger("Skill1");

        //������ ���� ������� �����������
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayers);

        // ������ ������� ��������� �������� ����
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit");
            enemy.GetComponent<EnemyUi>().TakeDamage(attackDamage*2f);
        }

        //������ ���� ������� ����������� ��������
        Collider[] hitObject = Physics.OverlapSphere(attackPoint.position, attackRadius, destractableLayers);

        // ������ ������� ����������� ������ �������� ����
        foreach (Collider destractableObject in hitObject)
        {
            Debug.Log("Hit");
            destractableObject.GetComponent<WoodAI>().TakeDamage(attackDamage);
        }

        //� ���� ���������� ������� ����� �� ����� ���������
        yield return new WaitForSeconds(1);
        
        //����� �����������
        IsAttack = false;
        //����� ������
        yield return new WaitForSeconds(10f);
        //����� ��������
        Skill1ColDown = false;
    }

    // ����������� ���� ����� ������ � ��������� 
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
