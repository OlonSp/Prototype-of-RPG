using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    //панель инвенторя и магазина
    public GameObject UIPanel;
    //панель инвенторя
    public Transform inventoryPanel;
    //список слотов в инвентаре
    public List<InventorySlot> slots = new List<InventorySlot>();
    //панель магазина
    public Transform ShopPanel;
    //список слотов в магазине
    public List<InventorySlot> shopSlots = new List<InventorySlot>();
    // открыта ли панель
    public bool isOpened;
    // включён ли режим купли-продажы(кп)
    public bool TradeMode = false;
    // кнопка включения режима кп
    public Button TradeButton;
    //игрок
    public GameObject Player;
    //камера
    public GameObject Cam;
    //прицел
    public GameObject CrossHair;
    
    // Start is called before the first frame update
    private void Awake()
    {
        //активируем панель для компиляции списков
        UIPanel.SetActive(true);
    }

    void Start()
    {
        //цвет показывает включён ли режим кп
        //красный выключен
        //зелёный включён
        TradeButton.image.color = Color.red;
        
        //компиляция списка слотов в инвенторе
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
                inventoryPanel.GetChild(i).GetComponent<InventorySlot>().player = Player;
            }
        }

        //компиляция списка слотов в магазине
        for (int i = 0; i < ShopPanel.childCount; i++)
        {
            if (ShopPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                shopSlots.Add(ShopPanel.GetChild(i).GetComponent<InventorySlot>());
                ShopPanel.GetChild(i).GetComponent<InventorySlot>().player = Player;
            }
        }

        //выключение панели
        UIPanel.SetActive(false);
    }

    //метод для режима игры геймпад
    //если инвентарь открыт,
    //при клике кнопки атаки геймпада рассчитывается расстояние между курсором и всеми слотами и кнопками в инвенторе
    //активирует ближайшую кнопку(слот)
    public void OnClick()
    {
        //если инвентарь закрыт метод не работает
        if (!isOpened)
            return;
        //дистанция
        float dist = float.MaxValue;
        //ближайшая кнопка
        Button choosebutton = null;

        //сравниваем расстояние между слотами инвенторя и курсором
        foreach(InventorySlot slot in slots)
        {
            if (Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, slot.GetComponent<RectTransform>().position) < dist)
            {
                dist = Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, slot.GetComponent<RectTransform>().position);
                //получаем ближайшую кнопку
                choosebutton = slot.GetComponent<Button>();
            }
        }

        //сравниваем расстояние между слотами магазина и курсором
        foreach (InventorySlot slot in shopSlots)
        {
            if (Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, slot.GetComponent<RectTransform>().position) < dist)
            {
                dist = Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, slot.GetComponent<RectTransform>().position);
                //получаем ближайшую кнопку
                choosebutton = slot.GetComponent<Button>();
            }
        }


        //сравниваем растояние между кнопкой трейда и курсором
        if (Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, TradeButton.GetComponent<RectTransform>().position) < dist)
        {
            choosebutton = TradeButton.GetComponent<Button>();
        }

        // активируем ближайшую кнопку
        if(choosebutton != null)
            choosebutton.onClick.Invoke();

        Debug.Log(choosebutton.name);

    }

    //метод открывающий и закрывающий инвентарь 
    public void OpenAndCloseInventory()
    {
        //если открыт, закрываем
        if (isOpened)
        {
            //курсор мыши скрываем
            Cursor.visible = false;
            //панель инвенторя закрывем
            UIPanel.SetActive(false);
            isOpened = false;
            //включение логики управления игроком
            Player.GetComponent<PlayerMovement>().enabled = true;
            Player.GetComponent<AttackUI>().enabled = true;
            Cam.GetComponent<CameraFlow>().enabled = true;

            //отключаем курсор и ставим его в центр экрана
            CrossHair.SetActive(false);
            CrossHair.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0.01163405f);

        }
        else
        {
            //курсор мыши делаем видимым
            Cursor.visible = true;
            //открываем панель инвенторя
            UIPanel.SetActive(true);
            isOpened = true;
            //отключение логики управления игроком
            Player.GetComponent<PlayerMovement>().enabled = false;
            Player.GetComponent<AttackUI>().enabled = false;
            //отключение анимаций
            Player.GetComponent<PlayerMovement>().animator.SetFloat("Horizontal", 0);
            Player.GetComponent<PlayerMovement>().animator.SetFloat("Vertical", 0);
            //отключение логики камеры
            Cam.GetComponent<CameraFlow>().enabled = false;
            // если включён геймпад мод включаем курсор
            if (Player.GetComponent<PlayerMovement>().mode == PlayerMovement.ControlMode.Gamepad)
            {   
                CrossHair.SetActive(true);
            }
        }
    }
    //метод добавляет предмет в инвентарь
    public void AddItemToInventory(ItemScriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)
        {
            // В слоте уже имеется этот предмет
            if (slot.item == _item)
            {
                // и при этом этого предмета не максимальное количество, складываем его в слот
                if (slot.amount + _amount <= _item.maximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
               
            }
        }
        //если все слоты заполнены складываем предмет в первый свободный слот
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmountText.text = slot.amount.ToString();
                break;
            }
        }
    }
    //метод добавляет предмет в магазин
    public void AddItemToShop(ItemScriptableObject _item, int _amount)
    {

        foreach (InventorySlot slot in shopSlots)
        {
            // если в слоте уже имеется этот предмет
            if (slot.item == _item)
            {
                // и при этом этого предмета не максимальное количество, складываем его в слот
                if (slot.amount + _amount <= _item.maximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
                
            }
        }
        //если все слоты заполнены складываем предмет в первый свободный слот
        foreach (InventorySlot slot in shopSlots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmountText.text = slot.amount.ToString();
                break;
            }
        }
    }

    public void TradeModeWORK()
    {
        //если режим кп включён - отключить его, и наоборот
        if (TradeMode)
        {
            TradeMode = !TradeMode;
            TradeButton.image.color = Color.red;
        }
        else
        {
            TradeMode = !TradeMode;
            TradeButton.image.color = Color.green;
        }
    }

}
