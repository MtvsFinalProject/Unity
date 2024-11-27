using GH;
using MJ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;
using static Item;

public class SellSystem : MonoBehaviour
{
    // �κ��丮 ���� ��ư
    public Button[] shopTypeButton;

    // ���� �� 
    public Button buyButton;
    public Button CancleButton;

    /// <summary>
    /// �κ��丮 �θ� �� �ڽ� ������
    /// </summary>
    public GameObject parentPanel;
    public Item[] itemComponents;
    public ClickButton[] clickComponents;
    public GameObject[] childPanel;
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

    private static SellSystem instance;
    public static SellSystem GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        clickComponents = new ClickButton[25];
        childPanel = new GameObject[25];

        InitItemList();
        GetItemComponents();
        InitInventoryButtons();
    }

    // ���� �θ� �ڽĵ��� �־���� �Ѵ�. �̰� ��� �־��ٱ�?
    // ���� �κ��丮�� ���빰�� �ٲ���ٸ�?
    // �׶� �ش� �κ��丮 ������ ������ �����´�.
    // �ش� ������ ������ �����ؼ� �־��ش�.

    private List<Item>[] itemList = new List<Item>[2];
    private ItemType curInventoryItemType = ItemType.Common;

    public void InitItemList()
    {
        for (int i = 0; i < itemList.Count(); i++)
            itemList[i] = new List<Item>();
    }

    public void InitInventoryButtons()
    {
        for (int i = 0; i < shopTypeButton.Count(); i++)
        {
            int data = i;
            shopTypeButton[i].onClick.AddListener(
                () => ResetButton()
            );
            shopTypeButton[i].onClick.AddListener(
            () => SetItemComponent((ItemType)data)
            );
            shopTypeButton[i].onClick.AddListener(
            () => SelectButton((ItemType)data)
            );
            shopTypeButton[i].onClick.AddListener(
            () => BuyItemList.GetInstance().ClearBuyData()
            );
        }
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

            print("a : " + wrappedJson);

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
        ResetItemComponents();
        if(_itemType == ItemType.All) // ��� ������ �ε�
        {
            int idx = 0;
            for(int i = 0; i < (int)ItemType.All; i++)
            {
                for (int j = 0; j < itemList[i].Count(); j++)
                {
                    childPanel[idx].SetActive(true);
                    itemComponents[idx++].SetData(itemList[i][j].itemName, itemList[i][j].price, itemList[i][j].itemType.ToString(), itemList[i][j].count, itemList[i][j].prefab);
                }
            }

        }
        else
        {
            for (int i = 0; i < itemList[(int)_itemType].Count(); i++)
            {
                childPanel[i].SetActive(true);
                itemComponents[i].SetData(itemList[(int)_itemType][i].itemName, itemList[(int)_itemType][i].price, itemList[(int)_itemType][i].itemType.ToString(), itemList[(int)_itemType][i].count, itemList[(int)_itemType][i].prefab);
            }
        }

    }

    public void GetItemComponents()
    {
        itemComponents = parentPanel.GetComponentsInChildren<Item>();
        for (int i = 0; i < parentPanel.transform.childCount; i++)
        {
            childPanel[i] = parentPanel.transform.GetChild(i).gameObject;
            clickComponents[i] = childPanel[i].GetComponentInChildren<ClickButton>();
        }
            
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
        SettingInventory(ItemType.Common);
        SettingInventory(ItemType.MyClassRoom);

        ResetButton();
        SelectButton(ItemType.MyClassRoom);
    }


    IEnumerator WaitForConditionToBeTrue(ItemType _itemType)
    {
        GetItemData(_itemType);
        Debug.Log("ItemType: " + _itemType.ToString());
        Debug.Log("Waiting for condition...");
        // ������ true�� �� ������ ���
        yield return new WaitUntil(() => itemList[(int)_itemType].Count > 0);

        SetItemComponent(_itemType);
        Debug.Log("Condition met! Proceeding...");
    }

    public void SettingInventory(ItemType _itemType)
    {
        StartCoroutine(WaitForConditionToBeTrue(_itemType)); // ������
    }

    public void ResetButton()
    {
        for (int i = 0; i < shopTypeButton.Count(); i++)
        {
            shopTypeButton[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void SelectButton(ItemType _itemType)
    {
        shopTypeButton[(int)_itemType].GetComponent<Image>().color = new Color32(242, 106, 27, 255);
    }

    public void ResetClickButtonComponent()
    {
        foreach (ClickButton clickButton in clickComponents)
            clickButton.ResetClick();
    }

    //#region SingleTone
    //private static SellSystem instance;
    //public static SellSystem GetInstance()
    //{
    //    if (instance == null)
    //    {
    //        GameObject go = new GameObject();
    //        go.name = "SellSystem";
    //        go.AddComponent<SellSystem>();
    //    }
    //    return instance;
    //}

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    //#endregion

    //public GameObject itemPrefab;
    //public GameObject parentPanel; // ItemPrefab�� ���� ��ũ�Ѻ��� content
    //private List<GameObject> itemObjects = new List<GameObject>(); // ���� â�� ��� ������Ʈ

    //public List<Item.ItemData> items = new List<Item.ItemData>();
    //public List<Item> itemComponents = new List<Item>();

    //public List<ItemData> choiceItem;

    //public GameObject sellPrefab;
    //public GameObject sellParentPanel;

    //public Button sellButton;
    //public Button cancleButton;

    //public GameObject sellPopupPanel;
    //private FadeOutUI sellFadeOutUI;

    //public TMP_Text allPrice;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    InitItem();
    //    InitButton();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //public void InitButton()
    //{
    //    sellButton.onClick.AddListener(SellItems);
    //    cancleButton.onClick.AddListener(CancleItems);
    //    sellFadeOutUI = sellPopupPanel.GetComponentInChildren<FadeOutUI>();
    //}

    //public void SellItems()
    //{
    //    SettingInventoryItem();
    //    choiceItem.Clear();
    //    UpdateChoiceItemData();
    //    OnSellUI();
    //    ResetShopItemNs();
    //}

    //public void CancleItems()
    //{
    //    choiceItem.Clear();
    //    UpdateChoiceItemData();
    //    ResetShopItemNs();
    //}

    //public void OnSellUI()
    //{
    //    sellFadeOutUI.FadeInOut(0.1f, 1.0f);
    //}

    //public void InitItem()
    //{
    //    foreach (Item.ItemData item in items)
    //    {
    //        GameObject itemGame = Instantiate(itemPrefab, parentPanel.transform);
    //        itemComponents.Add(itemGame.GetComponent<Item>());
    //        itemGame.GetComponent<Item>().SetItemData(item, ItemType.ShopItem);
    //        itemObjects.Add(itemGame);
    //    }

    //}

    //public void SetChoiceItem(ItemData _itemData)
    //{
    //    int idx = ContainItem(_itemData.itemName);
    //    if (idx < 0)
    //    {
    //        ItemData choiceItemdata = new ItemData(_itemData);
    //        choiceItemdata.n = 1;
    //        choiceItem.Add(choiceItemdata);
    //    }
    //    else
    //    {
    //        choiceItem[idx].n++;
    //        choiceItem[idx].price += _itemData.price;
    //    }
    //    UpdateCalculatePrice();
    //}

    //public int ContainItem(string ItemName)
    //{
    //    for(int i = 0; i < choiceItem.Count; i++)
    //    {
    //        if (choiceItem[i].itemName == ItemName)
    //            return i;
    //    }

    //    return -1;
    //}

    //public void UpdateChoiceItemData()
    //{
    //    ResetChoiceItemData();
    //    foreach (ItemData itemData in choiceItem)
    //    {
    //        GameObject itemGame = Instantiate(sellPrefab, sellParentPanel.transform);
    //        itemGame.GetComponent<BuyItem>().SetData(itemData.itemName, itemData.n, itemData.price);
    //    }

    //}

    //public void ResetChoiceItemData()
    //{
    //    foreach (Transform child in sellParentPanel.transform)
    //        Destroy(child.gameObject);
    //}

    //public int GetItemIndex(Item.ItemData _itemData)
    //{
    //    return items.IndexOf(_itemData);
    //}

    //public void ResetShopItemNs()
    //{
    //    // ��ư Ŭ���� Ŭ�� �� ���� - ������ ���� ��
    //    foreach(GameObject item in itemObjects)
    //        item.GetComponentInChildren<ClickButton>().ResetClick();

    //    // ���� ��� ������Ʈ - 0
    //    UpdateCalculatePrice();
    //}

    //public int CalculatePrice()
    //{
    //    int AllPrice = 0;
    //    foreach (ItemData itemData in choiceItem)
    //        AllPrice += itemData.price;
    //    return AllPrice;
    //}

    //public void UpdateCalculatePrice()
    //{
    //    allPrice.text = CalculatePrice().ToString();
    //}

    //// ���Ž� �κ��丮 �������� �������ش�.
    //public void SettingInventoryItem()
    //{
    //    foreach (ItemData itemData in choiceItem)
    //        InventorySystem.GetInstance().AddItem(itemData.n, itemData.itemName);
    //}
}
