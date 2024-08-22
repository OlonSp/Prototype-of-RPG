using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    //������ ��������� � ��������
    public GameObject UIPanel;
    //������ ���������
    public Transform inventoryPanel;
    //������ ������ � ���������
    public List<InventorySlot> slots = new List<InventorySlot>();
    //������ ��������
    public Transform ShopPanel;
    //������ ������ � ��������
    public List<InventorySlot> shopSlots = new List<InventorySlot>();
    // ������� �� ������
    public bool isOpened;
    // ������� �� ����� �����-�������(��)
    public bool TradeMode = false;
    // ������ ��������� ������ ��
    public Button TradeButton;
    //�����
    public GameObject Player;
    //������
    public GameObject Cam;
    //������
    public GameObject CrossHair;
    
    // Start is called before the first frame update
    private void Awake()
    {
        //���������� ������ ��� ���������� �������
        UIPanel.SetActive(true);
    }

    void Start()
    {
        //���� ���������� ������� �� ����� ��
        //������� ��������
        //������ �������
        TradeButton.image.color = Color.red;
        
        //���������� ������ ������ � ���������
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
                inventoryPanel.GetChild(i).GetComponent<InventorySlot>().player = Player;
            }
        }

        //���������� ������ ������ � ��������
        for (int i = 0; i < ShopPanel.childCount; i++)
        {
            if (ShopPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                shopSlots.Add(ShopPanel.GetChild(i).GetComponent<InventorySlot>());
                ShopPanel.GetChild(i).GetComponent<InventorySlot>().player = Player;
            }
        }

        //���������� ������
        UIPanel.SetActive(false);
    }

    //����� ��� ������ ���� �������
    //���� ��������� ������,
    //��� ����� ������ ����� �������� �������������� ���������� ����� �������� � ����� ������� � �������� � ���������
    //���������� ��������� ������(����)
    public void OnClick()
    {
        //���� ��������� ������ ����� �� ��������
        if (!isOpened)
            return;
        //���������
        float dist = float.MaxValue;
        //��������� ������
        Button choosebutton = null;

        //���������� ���������� ����� ������� ��������� � ��������
        foreach(InventorySlot slot in slots)
        {
            if (Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, slot.GetComponent<RectTransform>().position) < dist)
            {
                dist = Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, slot.GetComponent<RectTransform>().position);
                //�������� ��������� ������
                choosebutton = slot.GetComponent<Button>();
            }
        }

        //���������� ���������� ����� ������� �������� � ��������
        foreach (InventorySlot slot in shopSlots)
        {
            if (Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, slot.GetComponent<RectTransform>().position) < dist)
            {
                dist = Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, slot.GetComponent<RectTransform>().position);
                //�������� ��������� ������
                choosebutton = slot.GetComponent<Button>();
            }
        }


        //���������� ��������� ����� ������� ������ � ��������
        if (Vector3.Distance(CrossHair.GetComponent<RectTransform>().position, TradeButton.GetComponent<RectTransform>().position) < dist)
        {
            choosebutton = TradeButton.GetComponent<Button>();
        }

        // ���������� ��������� ������
        if(choosebutton != null)
            choosebutton.onClick.Invoke();

        Debug.Log(choosebutton.name);

    }

    //����� ����������� � ����������� ��������� 
    public void OpenAndCloseInventory()
    {
        //���� ������, ���������
        if (isOpened)
        {
            //������ ���� ��������
            Cursor.visible = false;
            //������ ��������� ��������
            UIPanel.SetActive(false);
            isOpened = false;
            //��������� ������ ���������� �������
            Player.GetComponent<PlayerMovement>().enabled = true;
            Player.GetComponent<AttackUI>().enabled = true;
            Cam.GetComponent<CameraFlow>().enabled = true;

            //��������� ������ � ������ ��� � ����� ������
            CrossHair.SetActive(false);
            CrossHair.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0.01163405f);

        }
        else
        {
            //������ ���� ������ �������
            Cursor.visible = true;
            //��������� ������ ���������
            UIPanel.SetActive(true);
            isOpened = true;
            //���������� ������ ���������� �������
            Player.GetComponent<PlayerMovement>().enabled = false;
            Player.GetComponent<AttackUI>().enabled = false;
            //���������� ��������
            Player.GetComponent<PlayerMovement>().animator.SetFloat("Horizontal", 0);
            Player.GetComponent<PlayerMovement>().animator.SetFloat("Vertical", 0);
            //���������� ������ ������
            Cam.GetComponent<CameraFlow>().enabled = false;
            // ���� ������� ������� ��� �������� ������
            if (Player.GetComponent<PlayerMovement>().mode == PlayerMovement.ControlMode.Gamepad)
            {   
                CrossHair.SetActive(true);
            }
        }
    }
    //����� ��������� ������� � ���������
    public void AddItemToInventory(ItemScriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)
        {
            // � ����� ��� ������� ���� �������
            if (slot.item == _item)
            {
                // � ��� ���� ����� �������� �� ������������ ����������, ���������� ��� � ����
                if (slot.amount + _amount <= _item.maximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
               
            }
        }
        //���� ��� ����� ��������� ���������� ������� � ������ ��������� ����
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
    //����� ��������� ������� � �������
    public void AddItemToShop(ItemScriptableObject _item, int _amount)
    {

        foreach (InventorySlot slot in shopSlots)
        {
            // ���� � ����� ��� ������� ���� �������
            if (slot.item == _item)
            {
                // � ��� ���� ����� �������� �� ������������ ����������, ���������� ��� � ����
                if (slot.amount + _amount <= _item.maximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
                
            }
        }
        //���� ��� ����� ��������� ���������� ������� � ������ ��������� ����
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
        //���� ����� �� ������� - ��������� ���, � ��������
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
