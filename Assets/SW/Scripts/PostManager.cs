using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class PostManager : MonoBehaviour
    {
        private static PostManager instance;
        public static PostManager GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "PostManager";
                go.AddComponent<PostManager>();
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
