using GH;
using MJ;
using Ookii.Dialogs;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static HttpManager;
using static InventorySystem;
using static Item;
using static SellSystem;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

public class InventorySystem : MonoBehaviour
{
    // �κ��丮 ���� ��ư
    public Button[] inventoryTypeButton;

    /// <summary>
    /// �κ��丮 �θ� �� �ڽ� ������
    /// </summary>
    public GameObject parentPanel;
    public Item[] itemComponents;
    public GameObject[] childPanel;

    Item choiceItem;

    /// <summary>
    /// ��� ����ü
    /// </summary>
    [Serializable]
    public class ItemData
    {
        public int inventoryId;
        public int itemIdx;
        public string itemName; // ������ �̸�
        public int price; // ����
        public string itemType; // ������ Ÿ��
        public int count; // ������ ����

        public ItemData(int _inventoryId, int _itemIdx, string _itemName, int _price, string _itemType, int _count)
        {
            inventoryId = _inventoryId;
            itemIdx = _itemIdx;
            itemName = _itemName;
            price = _price;
            itemType = _itemType;
            count = _count;
        }
    }

    /*
     
      "inventoryId": 0,
  "itemIdx": 0,
  "count": 0,
  "itemType": "string"

       "itemIdx": 0,
    "itemName": "string",
    "price": 0,
    "itemType": "string",
    "count": 0
     */
    [Serializable]
    public struct ItemDatas
    {
        public List<ItemData> response;
    }

    private static InventorySystem instance;
    public static InventorySystem GetInstance()
    {
        return instance;
    }

    private void Awake()                            
    {
        if (instance == null)
        {
            instance = this;
        }

        StartCoroutine(FirstLoadData());
    }

    // ���� �θ� �ڽĵ��� �־���� �Ѵ�. �̰� ��� �־��ٱ�?
    // ���� �κ��丮�� ���빰�� �ٲ���ٸ�?
    // �׶� �ش� �κ��丮 ������ ������ �����´�.
    // �ش� ������ ������ �����ؼ� �־��ش�.

    //private List<Item>[] itemList = new List<Item>[2];
    private ItemType curInventoryItemType = ItemType.Common;

    private ItemDatas[] loadItemData = new ItemDatas[2];

    public void InitItemList()
    {
        SettingInventory(ItemType.MyClassRoom);
        SettingInventory(ItemType.Common);

        ResetButton();
    }

    public void InitInventoryButtons()
    {
        for (int i = 0; i < inventoryTypeButton.Count(); i++)
        {
            int data = i;
            inventoryTypeButton[i].onClick.AddListener(
                () => ResetButton()
            );
            inventoryTypeButton[i].onClick.AddListener(
            () => SetItemComponent((ItemType)data)
            );
            inventoryTypeButton[i].onClick.AddListener(
            () => SelectButton((ItemType)data)
            );
        }
    }
    private void Start()
    {

    }

    private void Update()
    {
        Debug.Log("���� �κ��丮 ����: " + curInventoryItemType.ToString());
    }
    public ItemType GetCurItemType()
    {
        return curInventoryItemType;
    }

    public void UpdateItemData(string itemType, string itemName, int _Count)
    {
        ItemData searchItem;
        ItemType _itemType = ItemType.Common;
        if (itemType == "MyClassRoom")
            _itemType = ItemType.MyClassRoom;

        searchItem = loadItemData[(int)_itemType].response.Find(x => itemName == x.itemName);
        if(searchItem != null)
            searchItem.count += _Count;

        SetItemComponent(_itemType);
    }

    public void PatchItemData(ItemType _itemType, string _itemName, int _Count)
    {
        // /list/" + DataManager.instance.mapId + "/" + itemData.itemType;
        ItemData itemData = loadItemData[(int)_itemType].response.Find(x => _itemName == x.itemName);
        itemData.count += _Count;

        //UpdateItemData(itemData.itemType, _itemName, _Count);

        if (null == itemData)
            return;

        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/inventory";
        info.body = JsonUtility.ToJson(itemData);
        info.contentType = "application/json";
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            SetItemComponent(_itemType);
            print(downloadHandler.text);
        };
        StartCoroutine(HttpManager.GetInstance().Patch(info));
    }

    public void GetItemData(ItemType _itemType)
    {
        // itemList[(int)_itemType].Clear();

        ItemDatas itemData;
        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/inventory/list/" + DataManager.instance.mapId + "/" + _itemType.ToString();
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            Debug.Log("GetItemData: " + downloadHandler.text);


            string wrappedJson = "{\"response\":" + downloadHandler.text + "}";

            print("a : " + wrappedJson);

            itemData = JsonUtility.FromJson<ItemDatas>(wrappedJson);
            loadItemData[(int)_itemType] = itemData;

            foreach (ItemData itemdata in loadItemData[(int)_itemType].response)
            {
                GameObject itemPrefab = LoadItemData(_itemType, itemdata.itemName);
                if (itemPrefab)
                    Debug.Log("itemPrefab - Name: " + itemdata.itemName + ", " + itemdata.count);
                else
                    Debug.Log("Null - itemPrefab  - " + itemdata.itemName);
                //itemList[(int)_itemType].Add(new Item(itemdata.itemName, itemdata.price, _itemType, itemdata.count, itemPrefab));
            }


        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }

    public void SetItemComponent(ItemType _itemType)
    {
        ResetItemComponents();
        if (loadItemData[(int)_itemType].response == null || loadItemData[(int)_itemType].response.Count <= 0 || childPanel.Count() <= 0)
            return;
        for (int i = 0; i < loadItemData[(int)_itemType].response.Count(); i++)
        {
            childPanel[i].SetActive(true);
            GameObject itemPrefab = LoadItemData(_itemType, loadItemData[(int)_itemType].response[i].itemName);
            itemComponents[i].SetData(loadItemData[(int)_itemType].response[i].itemName, loadItemData[(int)_itemType].response[i].price, loadItemData[(int)_itemType].response[i].itemType.ToString(), loadItemData[(int)_itemType].response[i].count, itemPrefab);
        }
    }

    public void GetItemComponents()
    {
        itemComponents = parentPanel.GetComponentsInChildren<Item>();
        for (int i = 0; i < parentPanel.transform.childCount; i++)
            childPanel[i] = parentPanel.transform.GetChild(i).gameObject;
    }

    public void ResetItemComponents()
    {
        for (int i = 0; i < childPanel.Count(); i++)
            childPanel[i].SetActive(false);
    }
    public GameObject LoadItemData(ItemType _itemType, string _itemName)
    {
        string ItemPath = "Inventory/" + _itemType.ToString() + "/" + _itemName;

        // �ش� �κ��丮 ���� ������Ʈ
        return FileManager.Instance.LoadGameObjectFromResource(ItemPath);

    }

    private void OnEnable()
    {
        //SettingInventory(ItemType.MyClassRoom);
        //SettingInventory(ItemType.Common);

        SetItemComponent(ItemType.Common);
        //ResetButton();
    }


    IEnumerator WaitForConditionToBeTrue(ItemType _itemType)
    {
        GetItemData(_itemType);
        Debug.Log("ItemType: " + _itemType.ToString());
        Debug.Log("Waiting for condition...");
        // ������ true�� �� ������ ���
        yield return new WaitUntil(() => loadItemData[(int)_itemType].response != null && loadItemData[(int)_itemType].response.Count() > 0);

        SetItemComponent(_itemType);
        Debug.Log("Condition met! Proceeding...");
    }

    IEnumerator FirstLoadData()
    {
        InitItemList();
        GetItemComponents();
        InitInventoryButtons();
        // ������ true�� �� ������ ���
        yield return new WaitUntil(() => (loadItemData[0].response != null && loadItemData[0].response.Count() > 0) && (loadItemData[1].response != null && loadItemData[1].response.Count() > 0));

        gameObject.SetActive(false);
        Debug.Log("Condition met! Proceeding...");
    }

    public void SettingInventory(ItemType _itemType)
    {
        StartCoroutine(WaitForConditionToBeTrue(_itemType)); // ������ ��
    }

    public void ResetButton()
    {
        for (int i = 0; i < inventoryTypeButton.Count(); i++)
        {
            inventoryTypeButton[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void SelectButton(ItemType _itemType)
    {
        inventoryTypeButton[(int)_itemType].GetComponent<Image>().color = new Color32(242, 106, 27, 255);
        curInventoryItemType = _itemType;
    }

    public bool CheckItem()
    {
        if (choiceItem && choiceItem.count > 0)
            return true;
        return false;

    }

    //public void AddItemData(ItemType _itemType, string _itemName, int _itemCount)
    //{
    //    // �ش��ϴ� ������ ������ ������
    //    Item itemData = itemList[(int)_itemType].Find(x => _itemName == x.itemName);
    //    if (itemData)
    //    {
    //        int idx = itemList[(int)_itemType].IndexOf(itemData);
    //        itemData.count += _itemCount;
    //    }
    //}

    //public Item GetItemData(ItemType _itemType, string _itemName)
    //{
    //    return itemList[(int)_itemType].Find(x => _itemName == x.itemName);
    //}

    public void SetChoiceItem(Item itemCom)
    {
        choiceItem = itemCom;
        DataManager.instance.setTileObj = choiceItem.prefab;
    }

    // Ÿ��, �̸�
    // Ÿ�� ��

    // ���� ���� �ִ� â ����
    public GameObject GetItemPrefab(int idx)
    {
        //if (loadItemData[0].response.Count - 1 < idx) // 0��° Ÿ�Ժ��� �� ������
        //{
        //    loadItemData[0].response[idx - loadItemData[0].response.Count]
        //}
        //else
        //{

        //}
        //for(int i = 0; i < (int)ItemType.Common + 1; i++)
        //{
        //    for(int j = 0; j < loadItemData[i].response.Count(); j++)
        //    {

        //    }
        //}

        //if(idx >= 50) // Common
        //{
        //    itemType.to
        //}
        //else // MyClassRoom
        //{

        //}
        //// ������ Ÿ�Կ� ���� 50�� ���̳�����

        //loadItemData[(int)ItemType]
        //string _itemName = loadItemData[(int)curInventoryItemType].response[(int)curInventoryItemType * 100 + idx].itemName;
        //string _itemType = loadItemData.response[idx].itemType;

        //if(_itemType == "MyClassRoom")
        //    return itemList[0].Find(x => _itemName == x.itemName).prefab;
        //else
        //    return itemList[1].Find(x => _itemName == x.itemName).prefab;

        return gameObject;
    }

}
