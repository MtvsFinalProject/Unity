using GH;
using SW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.Networking;
using static HttpManager;
using static MapRegisterDataUI;
namespace MJ
{
    [Serializable]
    public class ObjectContestInfo
    {
        public int id;
        public int objId; // ������Ʈ ���̵�
        public int x, y; // ��ǥ
        public int rot; // ȸ��
        public bool flip; // ����
        public int mapId; // �ʾ��̵�
        public DataManager.MapType mapType;
    }

    [Serializable]
    public struct MapContestData
    {
        public int id;
        public string title;
        public string description;
        public List<ObjectContestInfo> furnitureList;
        public string previewImageUrl;
        public int likeCount;
        public int viewCount;
        public int userId;
    }

    [Serializable]
    public class MapContestDataList
    {
        public List<MapContestData> response;
    }

    [Serializable]
    public struct DeleteItemData
    {
        public int objectId; // ������Ʈ ���̵�
        public int removedCount; // ������ ������Ʈ ����
    }

    [Serializable]
    public class DeleteItemDataList
    {
        public List<DeleteItemData> response;
    }

    public class MapContestLoader : MonoBehaviour
    {
        public static MapContestLoader instance;
        // Start is called before the first frame update

        private DataManager dataManager;
        private HttpManager httpManager;
        public MapContestDataList mapDatas;
        public MapContestScrollUI mapContestScrollUIComponent;

        public Texture2D[] sprites;
        //public List<Texture2D> sprites = new List<Texture2D>();

        public List<ObjectContestInfo> loadfurnitureList;


        public DeleteItemDataList deleteItemDataLists;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                sprites = new Texture2D[100];
            }
            else
            {
                Destroy(gameObject);
            }

        }

        public static MapContestLoader GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "MapContestLoader";
                go.AddComponent<MapContestLoader>();
            }
            return instance;
        }

        void Start()
        {
            dataManager = DataManager.instance;
            httpManager = HttpManager.GetInstance();
        }

        public void SendMapContestData(string mapRoute, MapRegisterData mapRegisterData)
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/map-contest/upload-image";
            info.contentType = "multipart/form-data";
            info.body = mapRoute; // ���� �̸�
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string fileName = downloadHandler.text.ToString().Substring(39);
                SendMapData(mapRegisterData, fileName);

            };
            StartCoroutine(HttpManager.GetInstance().UploadFileByFormData(info, mapRoute));

        }

        public void SendMapData(MapRegisterData mapRegisterData, string url)
        {
            MapContestData mapContestInfo = new MapContestData();

            mapContestInfo.title = mapRegisterData.title;
            mapContestInfo.description = mapRegisterData.Description;
            mapContestInfo.userId = DataManager.instance.mapId;
            mapContestInfo.previewImageUrl = url;
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/map-contest";
            info.body = JsonUtility.ToJson(mapContestInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };

            StartCoroutine(HttpManager.GetInstance().Post(info));
        }

        public void ReceiveMapImage(string ImageUrl, int idx)
        {
            HttpInfo info = new HttpInfo();
            info.url = ImageUrl;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                DownloadHandlerTexture handler = downloadHandler as DownloadHandlerTexture;
                sprites[idx] = handler.texture;
                //sprites[idx] = sprite;
            };
            StartCoroutine(HttpManager.GetInstance().DownloadSprite(info));
        }
        //public string title;
        //public string description;
        //public int mapId;
        //public string imageUrl;

        public void LoadMapData()
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/map-contest/list";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                mapDatas = JsonUtility.FromJson<MapContestDataList>(downloadHandler.text);
                for (int i = 0; i < sprites.Length; i++)
                    sprites[i] = null;
                Debug.Log("--------------------------------------------------------------------------------");
                for (int i = 0; i < mapDatas.response.Count; i++)
                    ReceiveMapImage(mapDatas.response[i].previewImageUrl, i);
                mapContestScrollUIComponent.LoadMapData();
                Debug.Log("--------------------------------------------------------------------------------");
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }

        public bool LoadSpriteComplete()
        {
            if (mapDatas.response.Count <= 0)
                return false;

            int count = 0;
            for(int i = 0; i < sprites.Count(); i++)
            {
                if (sprites[i] != null)
                    count++;
            }
            if (count == mapDatas.response.Count)
                return true;

            return false;
        }

        public void LoadFurniture()
        {
            SetTile setTile = dataManager.player.GetComponent<SetTile>();
            foreach (ObjectContestInfo info in loadfurnitureList)
            {
                setTile.LoadData(new Vector3Int(info.x, info.y, 0), InventorySystem.GetInstance().GetItemIndex(info.id), info.id);
            }
        }

        public void MapContestEditSave(MapContestData mapContestInfo)
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/map-contest";
            info.body = JsonUtility.ToJson(mapContestInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Patch(info));
        }

        public void MapContestDeleteAllFurniture()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/furniture/list/" + AuthManager.GetInstance().userAuthData.userInfo.id;
            Debug.Log("MapContestDeleteAllFurniture: " + info.url);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                deleteItemDataLists = JsonUtility.FromJson<DeleteItemDataList>(res.text);
                print("res��: " + res);
                print("���ſϷ�");
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));

        }

        // �� ���׽�Ʈ ���� ���� ������ �濡 ��ġ
        public void MapCopyFurniture()
        {
            SetTile setTileComponent = DataManager.instance.player.GetComponent<SetTile>();
            if(setTileComponent)
            {
                foreach (ObjectContestInfo info in loadfurnitureList)
                    setTileComponent.CopyTile(new Vector3Int(info.x, info.y, 0), info.objId);
            }
        }
    }
}

