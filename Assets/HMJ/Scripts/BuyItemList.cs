using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class BuyItemList : MonoBehaviour
{
    public TMP_Text coinText;

    public Button buyButton;
    public Button CancleButton;

    public GameObject parentPanel;
    public GameObject buyPanel;

    private static BuyItemList instance;
    public static BuyItemList GetInstance()
    {
        return instance;
    }

    class BuyItemData
    {
        public BuyItem buyItemCom;
        public int count;
        public ItemType itemType;

        public BuyItemData(BuyItem _buyItem, int _count, ItemType _itemType)
        {
            buyItemCom = _buyItem;
            count = _count;
            itemType = _itemType;
        }

        public void AddCount()
        {
            count++;
        }
    }
    private Dictionary<string, BuyItemData> buyItemList = new Dictionary<string, BuyItemData>();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        InitButtons();
    }

    public void InitButtons()
    {
        buyButton.onClick.AddListener(PatchInventoryData);
        buyButton.onClick.AddListener(ClearBuyData);

        CancleButton.onClick.AddListener(ClearBuyData);
    }

    public void AddBuyItem(ItemType itemType, string itemName, int price)
    {
        if (buyItemList.ContainsKey(itemName))
        {
            buyItemList[itemName].AddCount();
        }
        else
        {
            // ������ ���� �г� �߰�
            GameObject gameObject = Instantiate(buyPanel, parentPanel.transform);

            // �ش� ������ ���� ���� �� ������ ������ ����
            buyItemList.Add(itemName, new BuyItemData(gameObject.GetComponentInChildren<BuyItem>(), 1, itemType));
        }

        buyItemList[itemName].buyItemCom.SetData(itemName, buyItemList[itemName].count, price * buyItemList[itemName].count);
        SetCalculateText();
    }

    public void ClearItemList()
    {
        buyItemList.Clear();
        foreach (Transform child in parentPanel.transform)
            Destroy(child.gameObject);
    }

    public void ClearBuyData()
    {
        SellSystem.GetInstance().ResetClickButtonComponent();
        ClearItemList();
    }

    public int CalculateBuyItem()
    {
        int calculateCoin = 0;
        foreach (KeyValuePair<string, BuyItemData> objectBuyItem in buyItemList)
        {
            InventorySystem.ItemData itemData = objectBuyItem.Value.buyItemCom.GetData();
            calculateCoin += itemData.price * itemData.count;
        }
        return calculateCoin;
    }

    public void SetCalculateText()
    {
        coinText.text = CalculateBuyItem().ToString();
    }

    public void PatchInventoryData()
    {
        foreach (KeyValuePair<string, BuyItemData> objectBuyItem in buyItemList)
            InventorySystem.GetInstance().PatchItemData(objectBuyItem.Value.itemType, objectBuyItem.Key, objectBuyItem.Value.count);
    }
}
