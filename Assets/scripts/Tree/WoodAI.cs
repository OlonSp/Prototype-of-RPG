using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodAI : MonoBehaviour
{
    //��������� ������
    [SerializeField] public InventoryManager inventory;
    //������������ �� ������
    public float max_HP;
    //������� �� ������
    private float current_HP;
    //���������� �� ������
    public bool isExist = true;
    //�������������� �������� ����������� � ������
    public ItemScriptableObject item;

    private void Start()
    {
        current_HP = max_HP;
    }

    private void Update()
    {
        //���� ������ ���������� ��������� ��� ��
        if(isExist)
            if (current_HP <= 0)
                StartCoroutine(WasFelled());
    }
    //��������� ����� �������
    public void TakeDamage(float damage)
    {
        current_HP -= damage;
    }
    //���� ������ ��������
    private IEnumerator WasFelled()
    {
        //��������� ���� ������ � ���������
        inventory.AddItemToInventory(item, 1);
        Debug.Log("Felled");
        //������ �������� ������������
        isExist = false;
        
        yield return new WaitForSeconds(0.1f);//������������ ��������, ����� �� ��������� ����� �������
        //���������� ������
        Destroy(gameObject);

    }
}
