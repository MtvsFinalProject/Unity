using GH;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class AuthManager : MonoBehaviour
    {
        private static AuthManager instance;
        public static AuthManager GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "AuthManager";
                go.AddComponent<AuthManager>();
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
        }

        //��ū��
        public string accessToken;
        public string refreshToken;

        [System.Serializable]
        public struct AuthData
        {
            

            public UserInfo userInfo;
            public AuthData(UserInfo info)
            {
                userInfo = info;
                DataManager.instance.mapId = info.id;
                //GetInstance().OnlineStatue();
                WebSocketManager.GetInstance().LogIn(info.id);
            }

        }

        public AuthData userAuthData { get; set; }
        public int MapId { get; set; } = 1;

        private void Start()
        {
            return;
            redirectUri = HttpManager.GetInstance().SERVER_ADRESS + "/login/oauth2/code/kakao";
            serverUrl = HttpManager.GetInstance().SERVER_ADRESS + "/kakaoLoginLog";
            OnKakaoLoginButtonClick();
        }

        // Kakao API ���� ����
        private string kakaoLoginUrl = "https://kauth.kakao.com/oauth/authorize";
        private string clientId = "214a201e2f4c682aa352ec136a79189b";  // Kakao REST API Key
        private string redirectUri;  // Spring ������ ���𷺼� URL

        // Spring ���� URL (Authorization Code�� ���� ��������Ʈ)
        private string serverUrl;

        private string clientState;


        // UI ��ư Ŭ�� �� ����Ǵ� �Լ�
        public void OnKakaoLoginButtonClick()
        {
            clientState = GenerateRandomClientId();
            //Debug.Log("Generated Client State: " + clientState);

            // īī�� �α��� �������� �̵��ϴ� URL ����
            string url = $"{kakaoLoginUrl}?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&state={clientState}&prompt=login";

            // �� ���������� īī�� �α��� ������ ����
            Application.OpenURL(url);

            // �� �ܰ迡�� Spring ������ Authorization Code�� �ް� �Ǹ� Unity���� HTTP ��û�� �����ϴ�.
            StartCoroutine(getAccessToken());
        }

        // ���� ���� Ŭ�� �� ����Ǵ� �Լ�
        public void OnDeleteAccountClick()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user?userId=" + userAuthData.userInfo.id;
            info.onComplete = (DownloadHandler res) =>
            {
                Debug.Log(userAuthData.userInfo.nickname + "�� ���� ���� ����");
            };
            StartCoroutine(HttpManager.GetInstance().Delete(info));
        }

        // Spring ������ Authorization Code�� ������ �Լ�
        public IEnumerator getAccessToken()
        {
            string checkUrl = serverUrl + $"?state={clientState}";

            int cnt = 0;

            while (cnt < 500)
            {
                cnt++;
                UnityWebRequest www = UnityWebRequest.Get(checkUrl);
                yield return www.SendWebRequest();

                //Debug.Log("@@@");

                if (www.result == UnityWebRequest.Result.Success && www.downloadHandler.text.Length > 5)
                {
                    Debug.Log("Authorization Code successfully sent to server.");
                    Debug.Log(www.downloadHandler.text);
                    userAuthData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
                   
                    break;
                }

                // �α��� �Ϸ� ��ȣ�� �� ������ ��� (1�� ��� �� ��õ�)
                yield return new WaitForSeconds(0.2f);
            }
        }

        // ���� �ĺ��� ���� �Լ� (UUID �������� ������ ����)
        public string GenerateRandomClientId()
        {
            return Guid.NewGuid().ToString();  // UUID �������� ������ ����
        }
        public void OnlineStatue()
        {
            StopCoroutine(COnlineStatus());
            StartCoroutine(COnlineStatus());
        }
        private IEnumerator COnlineStatus()
        {
            while (true)
            {
                HttpManager.HttpInfo info = new HttpManager.HttpInfo();
                info.url = HttpManager.GetInstance().SERVER_ADRESS + "/schedule/request-online-status";
                info.body = JsonUtility.ToJson(new Schedule());
                info.contentType = "application/json";
                StartCoroutine(HttpManager.GetInstance().Post(info));
                yield return new WaitForSeconds(3);
            }
        }
        private class Schedule
        {
            public int userId;
            public int mapId;
            public string mapType;
            public Schedule()
            {
                userId = GetInstance().userAuthData.userInfo.id;
                mapId = DataManager.instance.mapId;
                mapType = DataManager.instance.MapTypeState.ToString();
            }
        }
    }
}
