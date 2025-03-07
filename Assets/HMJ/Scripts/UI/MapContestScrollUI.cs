using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MapRegisterDataUI;

public class MapContestScrollUI : ScrollUI
{
    public MapRegisterScrollUI myMapRegisterScrollUIcp;

    bool settingImage = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();

        // 모든 맵 콘테스트 맵 등록시에 닫기
        addItemButton.onClick.AddListener(() => SceneUIManager.GetInstance().OffAllMapPanel());
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public override void AddItem()
    {
        GameObject registerObject = myMapRegisterScrollUIcp.GetRegisterGameObject();
        if(registerObject)
        {
            MapRegisterData mapRegisterData = registerObject.GetComponent<MapRegisterDataUI>().GetRegisterData();

            MapContestLoader.GetInstance().SendMapContestData(CaptureManager.GetInstance().GetCapturePath(), mapRegisterData);
            SceneUIManager.GetInstance().OnMapSuccessRegisterPanel();
        }

    }

    public void LoadMapData()
    {
        StartCoroutine(WaitForConditionToBeTrue());
    }

    public void LoadMapChild()
    {
        ResetData();
        List<MapContestData> respon = MapContestLoader.GetInstance().mapDatas.response;
        for(int i = 0; i < respon.Count; i++)
        {
            GameObject item = Instantiate(prefab, content);
            //item.set
            MJ.MapContestDataUI mapContestDataUI = item.GetComponent<MJ.MapContestDataUI>();

            int count = 0;
            for (; count < 100; count++)
            {
                if (MapContestLoader.GetInstance().sprites[i] == null)
                    break;
            }
            if (count != 0)
            {
                mapContestDataUI.SetRegisterData(respon[i], MapContestLoader.GetInstance().sprites[i]);

                itemlist.Add(item);
                imageList.Add(item.GetComponent<Image>());
            }
        }
        SceneUIManager.GetInstance().OnMapSuccessRegisterPanel();
    }
    public void ResetColor()
    {
        foreach (var image in imageList)
            image.color = Color.white;
    }

    private void OnEnable()
    {
        // 맵 데이터 로드
        MapContestLoader.GetInstance().LoadMapData();
        LoadMapData();
    }

    public void MapDataLoad()
    {
        // 맵 데이터 로드
        MapContestLoader.GetInstance().LoadMapData();
        LoadMapData();
    }

    public void ResetData()
    {
        itemlist.Clear();
        imageList.Clear();
        content.DetachChildren();
    }

    IEnumerator WaitForConditionToBeTrue()
    {
        Debug.Log("Waiting for condition...");
        ResetData();
        // 조건이 true가 될 때까지 대기
        yield return new WaitUntil(() => MapContestLoader.GetInstance().LoadSpriteComplete());

        LoadMapChild();
        Debug.Log("Condition met! Proceeding...");
    }
}
