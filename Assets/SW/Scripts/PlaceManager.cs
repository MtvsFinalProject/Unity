using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class PlaceManager : MonoBehaviour
    {
        HttpManager httpManager;
        private void Start()
        {
            httpManager = HttpManager.GetInstance();
        }
        
        [Serializable]
        public class ObjectInfo
        {
            public int objId; // ������Ʈ ���̵�
            public int x, y; // ��ǥ
            public int rot; // ȸ��
        }
        [Serializable] // ��ġ ��û �Ķ����
        public class SetPlaceInfo : ObjectInfo
        {
            private int userId;
            private int mapId;
            public SetPlaceInfo(ObjectInfo objectInfo)
            {
                objId = objectInfo.objId;
                x = objectInfo.x;
                y = objectInfo.y;
                rot = objectInfo.rot;
                // ���� �ʿ�
                userId = HttpManager.GetInstance().UserId;
                mapId = HttpManager.GetInstance().MapId;
            }
        }

        public void CreatePlace(ObjectInfo objectInfo)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/ground-furniture";
            info.body = JsonUtility.ToJson(new SetPlaceInfo(objectInfo));
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                print("������û�Ϸ�, id : " + res.text);
            };
            StartCoroutine(httpManager.Post(info));
        }
        [Serializable] // ��ġ �ҷ����� ����
        public class GetPlaceResInfo
        {
            public List<GetPlaceInfo> data;
        }
        [Serializable] // �ҷ��� ������Ʈ ����
        public class GetPlaceInfo : ObjectInfo
        {
            public int id;
            public int userId;
            public int mapId;
        }
        public void ReadPlace()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/ground-furniture/map?mapId=" + HttpManager.GetInstance().MapId;
            info.onComplete = (DownloadHandler res) =>
            {
                GetPlaceResInfo dataInfo = JsonUtility.FromJson<GetPlaceResInfo>("{\"data\":" + res.text + "}");
                foreach (GetPlaceInfo info in dataInfo.data)
                {
                    print(info.id + "/" + info.userId + "/" + info.mapId + "/" + info.objId + "/" + info.x + "/" + info.y + "/" + info.rot);
                }
            };
            StartCoroutine(httpManager.Get(info));
        }

        public void DeletePlace(int furnitureId)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/ground-furniture?furnitureId=" + furnitureId;
            info.onComplete = (DownloadHandler res) =>
            {
                print("���ſϷ�");
            };
            StartCoroutine(httpManager.Delete(info));
        }
    }
}
