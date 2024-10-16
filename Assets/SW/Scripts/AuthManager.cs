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

        [System.Serializable]
        public struct AuthData
        {
            public int id;
            public string state;
            public string accessToken;
        }
        public AuthData UserAuthData { get; private set; }

        private void Start()
        {
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
                    UserAuthData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
                    print(UserAuthData.id);
                    print(UserAuthData.state);
                    print(UserAuthData.accessToken);
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
    }
}
