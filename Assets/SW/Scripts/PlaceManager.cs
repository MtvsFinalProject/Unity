using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class PlaceManager : MonoBehaviour
    {
        HttpManager httpManager = HttpManager.GetInstance();

        [Serializable]
        public class ObjectInfo
        {
            public int objId; // ������Ʈ ���̵�
            public int x, y; // ��ǥ
            public int rot; // ȸ��
        }
        [Serializable]
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
                userId = 0;
                mapId = 0;
            }
        }

        public void SetPlace(ObjectInfo objectInfo)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/��������Ʈ";
            info.body = JsonUtility.ToJson(new SetPlaceInfo(objectInfo));
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                print("Set��û�Ϸ�");
            };
            StartCoroutine(httpManager.Post(info));
        }
        [Serializable]
        public class GetPlaceReqInfo
        {
            int userId;
            int mapId;
            public GetPlaceReqInfo()
            {
                // ���� �ʿ�
                userId = 0;
                mapId = 0;
            }
        }
        [Serializable]
        public class GetPlaceResInfo
        {
            public List<ObjectInfo> data;
        }
        public void GetPlace()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/��������Ʈ";
            info.body = JsonUtility.ToJson(new GetPlaceReqInfo());
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                GetPlaceResInfo dataInfo = JsonUtility.FromJson<GetPlaceResInfo>(res.text);
                print("Get��û�Ϸ� : " + res.text);
            };
            StartCoroutine(httpManager.Get(info));
        }
    }
}
