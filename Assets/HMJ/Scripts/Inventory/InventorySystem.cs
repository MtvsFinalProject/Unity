using GH;
using MJ;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;
using static InventorySystem;
using static Item;

public class InventorySystem : MonoBehaviour
{
    /// <summary>
    /// �κ��丮 �θ� �� �ڽ� ������
    /// </summary>
    public GameObject parentPanel;
    public Item[] itemComponents;
    /// <summary>
    /// ��� ����ü
    /// </summary>
    [Serializable]
    public struct ItemData
    {
        public string itemName; // ������ �̸�
        public int price; // ����
        public string itemType; // ������ Ÿ��
        public int count; // ������ ����
    }

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
        InitItemList();
    }

    // ���� �θ� �ڽĵ��� �־���� �Ѵ�. �̰� ��� �־��ٱ�?
    // ���� �κ��丮�� ���빰�� �ٲ���ٸ�?
    // �׶� �ش� �κ��丮 ������ ������ �����´�.
    // �ش� ������ ������ �����ؼ� �־��ش�.

    private List<Item>[] itemList = new List<Item>[(int)Item.ItemType.ItemTypeEnd];
    private ItemType curInventoryItemType = ItemType.Common;

    public void InitItemList()
    {
        for (int i = 0; i < itemList.Count(); i++)
            itemList[i] = new List<Item>();
    }

    private void Start()
    {

    }
    public void FetchItemData()
    {

    }

    public void GetItemData(ItemType _itemType)
    {
        itemList[(int)_itemType].Clear();

        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/inventory/list/" + DataManager.instance.mapId + "/" + _itemType.ToString();
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            Debug.Log("GetItemData: " + downloadHandler.text);
           

            string wrappedJson = "{\"response\":" + downloadHandler.text + "}";
       
            print("a : "+wrappedJson);

            ItemDatas itemData = JsonUtility.FromJson<ItemDatas>(wrappedJson);

            foreach (ItemData itemdata in itemData.response)
            {
                GameObject itemPrefab = LoadItemData(_itemType, itemdata.itemName);
                if (itemPrefab)
                    Debug.Log("itemPrefab - Name: " + itemdata.itemName + ", " + itemdata.count);
                else
                    Debug.Log("Null - itemPrefab  - " + itemdata.itemName);
                itemList[(int)_itemType].Add(new Item(itemdata.itemName, itemdata.price, _itemType, itemdata.count, itemPrefab));
            }
                

        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }

    public void SetItemComponent(ItemType _itemType)
    {
        itemComponents = parentPanel.GetComponentsInChildren<Item>();
        for(int i = 0; i < itemList[(int)_itemType].Count(); i++)
            itemComponents[i].SetData(itemList[(int)_itemType][i].itemName, itemList[(int)_itemType][i].price, itemList[(int)_itemType][i].itemType, itemList[(int)_itemType][i].count, itemList[(int)_itemType][i].prefab);

        //Debug.Log("ItemList: " + itemList[(int)_itemType].Count());
        //for (int i = itemList[(int)_itemType].Count(); i < itemComponents.Count(); i++)
        //    itemComponents[i].gameObject.SetActive(false);
    }

    public GameObject LoadItemData(ItemType _itemType, string _itemName)
    {
        string ItemPath = "Inventory/" + _itemType.ToString() + "/" + _itemName;

        // �ش� �κ��丮 ���� ������Ʈ
        return FileManager.Instance.LoadGameObjectFromResource(ItemPath);

    }

    private void OnEnable()
    {
        SettingInventory(curInventoryItemType);
    }

    IEnumerator WaitForConditionToBeTrue(ItemType _itemType)
    {
        GetItemData(_itemType);
        Debug.Log("Waiting for condition...");
        // ������ true�� �� ������ ���
        yield return new WaitUntil(() => itemList[0].Count + itemList[1].Count > 0);

        SetItemComponent(_itemType);
        Debug.Log("Condition met! Proceeding...");
    }

    public void SettingInventory(ItemType _itemType)
    {
        StartCoroutine(WaitForConditionToBeTrue(_itemType)); // ������ ��
    }

}
