using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //характеристика предмета в слоте
    public ItemScriptableObject item;
    //его количество
    public int amount;
    //пустой ли слот
    public bool isEmpty = true;
    //gameObject иконка слота
    public GameObject iconGO;
    //игрок
    public GameObject player;
    //цена предмета
    public int cost = 1;
    //текст отображающий количество предмета в слоте
    public Text itemAmountText;

    private void Awake()
    {
        iconGO = transform.GetChild(0).gameObject;
        itemAmountText = transform.GetChild(1).GetComponent<Text>();
    }
    //при заполнении слота предметом изменяется его иконка
    public void SetIcon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
    }
    //функция продажы объекта
    public void SellItem()
    { 
        //если слот не пустой
        if (isEmpty == false)
            if (GetComponentInParent<InventoryManager>().TradeMode)// если включён режим купли-продажы
            {
                GetComponentInParent<InventoryManager>().AddItemToShop(item, 1);//добавляем предмет в магазин
                amount--;
                if (amount == 0)
                {
                    //удаляем его из слота
                    item = null;

                    isEmpty = true;
                    iconGO.GetComponent<Image>().sprite = null;
                    iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    itemAmountText.text = "";
                }
                else
                    itemAmountText.text = amount.ToString();
                //увеличиваем количество монет у игрока
                player.GetComponent<PlayerMovement>().coins += cost;
                PlayerPrefs.SetInt("Coins", player.GetComponent<PlayerMovement>().coins);
                player.GetComponent<PlayerMovement>().CoinText.text = player.GetComponent<PlayerMovement>().coins.ToString();
            }
    }

    public void BuyItem()
    {
        //если слот не пустой
        if (isEmpty == false)
            if (GetComponentInParent<InventoryManager>().TradeMode)// если включён режим купли-продажы
            {
                //добавляем предмет в инвентарь
                GetComponentInParent<InventoryManager>().AddItemToInventory(item, 1);

                amount--;
                if (amount == 0)
                {
                    //удаляем его из слота
                    item = null;

                    isEmpty = true;
                    iconGO.GetComponent<Image>().sprite = null;
                    iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    itemAmountText.text = "";
                }
                else
                    itemAmountText.text = amount.ToString();

                //уменьшаем количество монет у игрока
                player.GetComponent<PlayerMovement>().coins -= cost;
                PlayerPrefs.SetInt("Coins", player.GetComponent<PlayerMovement>().coins);
                player.GetComponent<PlayerMovement>().CoinText.text = player.GetComponent<PlayerMovement>().coins.ToString();
            }
    }
}
