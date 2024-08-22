using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//создание характеристик предмета
[CreateAssetMenu(fileName = "ItemScriptableObject", menuName ="Inventory/Item" , order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    //имя объекта
    public string itemName;
    //максимальное количество в слоте
    public int maximumAmount;
    //иконка предмета
    public Sprite icon;
    //описание
    public string itemDescription;
}
