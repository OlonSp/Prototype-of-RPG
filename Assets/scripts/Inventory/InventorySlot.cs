using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //�������������� �������� � �����
    public ItemScriptableObject item;
    //��� ����������
    public int amount;
    //������ �� ����
    public bool isEmpty = true;
    //gameObject ������ �����
    public GameObject iconGO;
    //�����
    public GameObject player;
    //���� ��������
    public int cost = 1;
    //����� ������������ ���������� �������� � �����
    public Text itemAmountText;

    private void Awake()
    {
        iconGO = transform.GetChild(0).gameObject;
        itemAmountText = transform.GetChild(1).GetComponent<Text>();
    }
    //��� ���������� ����� ��������� ���������� ��� ������
    public void SetIcon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
    }
    //������� ������� �������
    public void SellItem()
    { 
        //���� ���� �� ������
        if (isEmpty == false)
            if (GetComponentInParent<InventoryManager>().TradeMode)// ���� ������� ����� �����-�������
            {
                GetComponentInParent<InventoryManager>().AddItemToShop(item, 1);//��������� ������� � �������
                amount--;
                if (amount == 0)
                {
                    //������� ��� �� �����
                    item = null;

                    isEmpty = true;
                    iconGO.GetComponent<Image>().sprite = null;
                    iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    itemAmountText.text = "";
                }
                else
                    itemAmountText.text = amount.ToString();
                //����������� ���������� ����� � ������
                player.GetComponent<PlayerMovement>().coins += cost;
                PlayerPrefs.SetInt("Coins", player.GetComponent<PlayerMovement>().coins);
                player.GetComponent<PlayerMovement>().CoinText.text = player.GetComponent<PlayerMovement>().coins.ToString();
            }
    }

    public void BuyItem()
    {
        //���� ���� �� ������
        if (isEmpty == false)
            if (GetComponentInParent<InventoryManager>().TradeMode)// ���� ������� ����� �����-�������
            {
                //��������� ������� � ���������
                GetComponentInParent<InventoryManager>().AddItemToInventory(item, 1);

                amount--;
                if (amount == 0)
                {
                    //������� ��� �� �����
                    item = null;

                    isEmpty = true;
                    iconGO.GetComponent<Image>().sprite = null;
                    iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    itemAmountText.text = "";
                }
                else
                    itemAmountText.text = amount.ToString();

                //��������� ���������� ����� � ������
                player.GetComponent<PlayerMovement>().coins -= cost;
                PlayerPrefs.SetInt("Coins", player.GetComponent<PlayerMovement>().coins);
                player.GetComponent<PlayerMovement>().CoinText.text = player.GetComponent<PlayerMovement>().coins.ToString();
            }
    }
}
