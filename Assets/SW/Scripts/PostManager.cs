using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class PostManager : MonoBehaviour
    {
        HttpManager httpManager = HttpManager.GetInstance();
        [Serializable]
        public class PostInfo
        {
            public int userId;
            public int mapId;
            public string nickname;
            public string title;
            public string content;
            public PostInfo()
            {
                // ���� �ʿ�
                userId = 0;
                mapId = 0;
                nickname = "";
            }
        }
        [Serializable]
        public class LoadReqInfo
        {
            int mapId;
            public LoadReqInfo()
            {
                // ���� �ʿ�
                mapId = 0;
            }
        }
        [Serializable]
        public class PostList
        {
            public List<PostInfo> data;
        }
        public void CreatePost(PostInfo postInfo)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/��������Ʈ";
            info.body = JsonUtility.ToJson(postInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                print("�Խ� ��û �Ϸ�");
            };
            StartCoroutine(httpManager.Post(info));
        }
        public void LoadPost()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/��������Ʈ";
            info.body = JsonUtility.ToJson(new LoadReqInfo());
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                PostList list = JsonUtility.FromJson<PostList>(res.text);
                print("�ε��Ϸ� : " + res.text);
            };
            StartCoroutine(httpManager.Get(info));
        }
    }
}
