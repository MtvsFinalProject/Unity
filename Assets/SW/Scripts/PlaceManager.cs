using GH;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class PlaceManager : MonoBehaviour
    {
        #region �̱���
        private static PlaceManager instance;
        public static PlaceManager GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "PlaceManager";
                go.AddComponent<PlaceManager>();
            }
            return instance;
        }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            httpManager = HttpManager.GetInstance();
        }
        private HttpManager httpManager;
        #endregion
        [Serializable]
        public class ObjectInfo
        {
            public int objId; // ������Ʈ ���̵�
            public int x, y; // ��ǥ
            public int rot; // ȸ��
            public bool flip; // ����
            public int mapId; // �ʾ��̵�
            public DataManager.MapType mapType;
        }
        [Serializable] // ��ġ ��û �Ķ����
        public class SetPlaceInfo : ObjectInfo
        {
            public SetPlaceInfo(ObjectInfo objectInfo)
            {
                objId = objectInfo.objId;
                x = objectInfo.x;
                y = objectInfo.y;
                rot = objectInfo.rot;
                flip = objectInfo.flip;
                mapId = objectInfo.mapId;
                mapType = objectInfo.mapType;
            }
        }

        public void CreatePlace(ObjectInfo objectInfo, Action<PlaceInfo> callBack)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/furniture";
            info.body = JsonUtility.ToJson(new SetPlaceInfo(objectInfo));
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                print("������û�Ϸ�, id : " + res.text);
                PlaceInfo value = new PlaceInfo(objectInfo);
                value.id = int.Parse(res.text);
                callBack(value);
            };
            StartCoroutine(httpManager.Post(info));
        }
        [Serializable] // ��ġ �ҷ����� ����
        public class GetPlaceResInfo
        {
            public bool success;
            public List<PlaceInfo> response;
            public string error;
        }
        [Serializable] // �ҷ��� ������Ʈ ����
        public class PlaceInfo : ObjectInfo
        {
            public int id;
            public PlaceInfo(ObjectInfo objectInfo)
            {
                objId = objectInfo.objId;
                x = objectInfo.x;
                y = objectInfo.y;
                rot = objectInfo.rot;
                flip = objectInfo.flip;
            }
        }
        public void ReadPlace(int mapId, DataManager.MapType mapType, Action<GetPlaceResInfo> callBack)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/furniture/list/map?mapId=" + (mapId == -1 ? AuthManager.GetInstance().MapId : mapId);
            info.onComplete = (DownloadHandler res) =>
            {
                GetPlaceResInfo dataInfo = JsonUtility.FromJson<GetPlaceResInfo>(res.text);
                foreach (PlaceInfo info in dataInfo.response)
                {
                    print(info.id + "/" + info.objId + "/" + info.x + "/" + info.y + "/" + info.rot);
                }
                callBack(dataInfo);
            };
            StartCoroutine(httpManager.Get(info));
        }

        public void DeletePlace(int furnitureId)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/furniture?furnitureId=" + furnitureId;
            info.onComplete = (DownloadHandler res) =>
            {
                print("���ſϷ�");
            };
            StartCoroutine(httpManager.Delete(info));
        }
    }
}
