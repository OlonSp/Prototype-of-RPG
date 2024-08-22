using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUI : MonoBehaviour
{
    // точка атаки
    [SerializeField] private Transform attackPoint;
    // радиус атаки
    [SerializeField] private float attackRadius = 0.5f;
    // урон
    [SerializeField] public float attackDamage = 100f;
    // слой в котором наход€тс€ противники
    [SerializeField] private LayerMask enemyLayers;
    // слой разрушаемых объектов
    [SerializeField] private LayerMask destractableLayers;
    // переменна€ определ€юща€ какой сечас тип управлени€
    private PlayerMovement controlmode;
    ////переменна€, котора€ определ€ет в процессе атаки сейчас игрок или нет
    private bool IsAttack = false;
    //переменна€ котора€ определ€ет в откате ли навык
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
    //¬ызов обычной атаки дл€ геймпада и мобилы
    public void SimplePunch()
    {
        if (controlmode.IsAlive)
            if (!IsAttack)
            {
                StartCoroutine(SimpleAttack());
            }
    }
    //¬ызов 1 скила атаки дл€ геймпада и мобилы
    public void SpecialSkill1()
    {
        if (controlmode.IsAlive)
            if (!IsAttack)
                if (!Skill1ColDown)
                    StartCoroutine(Skill1());

    }

    public IEnumerator SimpleAttack()
    {
        //игрок начал атаковать
        IsAttack = true;
        //вызов анимации атаки
        controlmode.animator.SetTrigger("Attack");

        //массив всех задетых противников
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayers);
        
        // каждый задетый противник получает урон
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit");
            enemy.GetComponent<EnemyUi>().TakeDamage(attackDamage);
        }

        //массив всех задетых разрушаемых объектов
        Collider[] hitObject = Physics.OverlapSphere(attackPoint.position, attackRadius, destractableLayers);

        // каждый задетый разрушаемый объект получает урон
        foreach (Collider destractableObject in hitObject)
        {
            Debug.Log("Hit");
            destractableObject.GetComponent<WoodAI>().TakeDamage(attackDamage);
        }

        //в этот промежуток времени игрок не может атаковать
        yield return new WaitForSeconds(1);
        //атака закончилась

        IsAttack = false;
    }

    public IEnumerator Skill1()
    {
        IsAttack = true;
        //включаем откат навыка
        Skill1ColDown = true;
        controlmode.animator.SetTrigger("Skill1");

        //массив всех задетых противников
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayers);

        // каждый задетый противник получает урон
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit");
            enemy.GetComponent<EnemyUi>().TakeDamage(attackDamage*2f);
        }

        //массив всех задетых разрушаемых объектов
        Collider[] hitObject = Physics.OverlapSphere(attackPoint.position, attackRadius, destractableLayers);

        // каждый задетый разрушаемый объект получает урон
        foreach (Collider destractableObject in hitObject)
        {
            Debug.Log("Hit");
            destractableObject.GetComponent<WoodAI>().TakeDamage(attackDamage);
        }

        //в этот промежуток времени игрок не может атаковать
        yield return new WaitForSeconds(1);
        
        //атака закончилась
        IsAttack = false;
        //откат умени€
        yield return new WaitForSeconds(10f);
        //откат закончен
        Skill1ColDown = false;
    }

    // отображение зоны атаки игрока в редакторе 
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
