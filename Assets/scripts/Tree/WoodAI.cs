using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodAI : MonoBehaviour
{
    //инвентарь игрока
    [SerializeField] public InventoryManager inventory;
    //максимальное хп дерева
    public float max_HP;
    //текущее хп дерева
    private float current_HP;
    //существует ли дерево
    public bool isExist = true;
    //характеристики предмета выпадающего с дерева
    public ItemScriptableObject item;

    private void Start()
    {
        current_HP = max_HP;
    }

    private void Update()
    {
        //если дерево существует мониторим его хп
        if(isExist)
            if (current_HP <= 0)
                StartCoroutine(WasFelled());
    }
    //получение урона деревом
    public void TakeDamage(float damage)
    {
        current_HP -= damage;
    }
    //если дерево срублено
    private IEnumerator WasFelled()
    {
        //добавляем одно бревно в инвентарь
        inventory.AddItemToInventory(item, 1);
        Debug.Log("Felled");
        //дерево перестаёт существовать
        isExist = false;
        
        yield return new WaitForSeconds(0.1f);//обязательная задержка, иначе не спавнятся новые деревья
        //уничтожаем дерево
        Destroy(gameObject);

    }
}
