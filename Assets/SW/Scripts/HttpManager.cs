using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct PostInfo
{
    public int userId;
    public int id;
    public string title;
    public string body;
}
[System.Serializable]
public struct PostInfoArray
{
    public List<PostInfo> data;
}
public class HttpManager : MonoBehaviour
{
    static HttpManager instance;
    public static HttpManager GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            go.name = "HttpManager";
            go.AddComponent<HttpManager>();
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
    public string SERVER_ADRESS { get; } = "http://175.196.249.220:5544";
    /*
    http://My-little-school-dev-env.eba-rfqxtdpp.ap-northeast-2.elasticbeanstalk.com
    http://125.132.216.190:5544
    http://175.196.249.220:5544
    */
    public class HttpInfo
    {
        public string url = "";
        // Body  데이터
        public string body = "";

        // contentType
        public string contentType = "";
        // 통신 성공 후 호출되는 함수 담을 변수
        public Action<DownloadHandler> onComplete;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    // GET : 서버에게 데이터를 조회 요청
    public IEnumerator Get(HttpInfo info)
    {
        string url = info.url;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Token Authorization 헤더 추가
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().accessToken);

            //print("Bearer " + AuthManager.GetInstance().accessToken);
            // 서버에 요청 보내기
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);
        }
    }
    public void GetMethod(HttpInfo info)
    {
        StartCoroutine(Get(info));
    }
    public IEnumerator Post(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, info.body, info.contentType))
        {
            // Token Authorization 헤더 추가
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().accessToken);

            // 서버에 요청 보내기
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);
        }
    }
 
    public IEnumerator Patch(HttpInfo info)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "PATCH"))
        {

            // 요청 본문 설정 (byte[] 형태로 설정)
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(info.body);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);

            // Token Authorization 헤더 추가
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().accessToken);

            // Content-Type 설정
            webRequest.SetRequestHeader("Content-Type", info.contentType);

            // 응답 받기 위한 다운로드 핸들러 설정
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // 서버에 요청 보내기
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);
        }
    }
  
    public IEnumerator Delete(HttpInfo info)
    {
        string url = info.url;

        // UnityWebRequest.Delete()로 DELETE 요청 생성
        using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
        {
            // Token Authorization 헤더 추가
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().accessToken);

            // 서버에 요청 보내기
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);
        }
    }

    // 파일 업로드(form-data)
    public IEnumerator UploadFileByFormData(HttpInfo info, string fileName)
    {
        // info.data에는 파일의 위치
        // info.data 에 있는 파일을 byte 배열로 읽어오자.
        byte[] data = File.ReadAllBytes(info.body);

        // data를 MultipartForm 으로 셋팅
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, fileName, info.contentType));

        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, formData))
        {
            // Token Authorization 헤더 추가
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().accessToken);

            print("요청");
            // 서버에 요청 보내기
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);            
        }

    }
    // 파일 업로드(form-data)
    public IEnumerator UploadFileByFormDataByte(HttpInfo info, string fileName, byte[] byteData)
    {
        // info.data에는 파일의 위치
        // info.data 에 있는 파일을 byte 배열로 읽어오자.
        byte[] data = byteData;

        // data를 MultipartForm 으로 셋팅
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, fileName, info.contentType));

        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, formData))
        {
            // Token Authorization 헤더 추가
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().accessToken);

            print("요청");
            // 서버에 요청 보내기
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);            
        }

    }

    // 파일 업로드
    public IEnumerator UploadFileByByte(HttpInfo info)
    {
        // info.data에는 파일의 위치
        // info.data 에 있는 파일을 byte 배열로 읽어오자.
        byte[] data = File.ReadAllBytes(info.body);

        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "Post"))
        {
            // Token Authorization 헤더 추가
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().accessToken);

            // 업로드 하는 데이터
            webRequest.uploadHandler = new UploadHandlerRaw(data);
            webRequest.uploadHandler.contentType = info.contentType;

            // 응답 받는 데이터 공간
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // 서버에 요청 보내기
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);
        }
    }

    public IEnumerator DownloadSprite(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(info.url))
        {
            // Token Authorization 헤더 추가
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().accessToken);

            yield return webRequest.SendWebRequest();
            DoneRequest(webRequest, info);
        }
    }

    public IEnumerator DownloadAudio(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(info.url, AudioType.WAV))
        {
            // Token Authorization 헤더 추가
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().accessToken);

            yield return webRequest.SendWebRequest();
            //DownloadHandlerAudioClip handler = webRequest.downloadHandler as DownloadHandlerAudioClip;
            //handler.audioClip 을 audiosource에 세팅하고 플레이
            DoneRequest(webRequest, info);
        }
    }

    public TokenData tokenGet;
    void DoneRequest(UnityWebRequest webRequest, HttpInfo info)
    {
        // 만약에 결과가 정상이라면
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // 응답 온 데이터를 요청한 클래스로 보내자.
            if (info.onComplete != null)
            {
                info.onComplete(webRequest.downloadHandler);
            }
            // print(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Net Error : " + webRequest.error); 
            
            if ((int)webRequest.responseCode == 403)
            {
              
                print("토큰을 다시 받아옵니다");

                TokenInfo tokenInfo = new TokenInfo();
                tokenInfo.userEmail = AuthManager.GetInstance().userAuthData.userInfo.email;
                tokenInfo.refreshToken = AuthManager.GetInstance().refreshToken;

                HttpInfo info2 = new HttpInfo();
                info2.url = HttpManager.GetInstance().SERVER_ADRESS + "/auth/refresh";
                info2.body = JsonUtility.ToJson(tokenInfo);
                info2.contentType = "application/json";
                info2.onComplete = (DownloadHandler downloadHandler) =>
                {
                   

                    print(downloadHandler.text);
                    //tokenGet = JsonUtility.FromJson<TokenData>(jsonData);
                    AuthManager.GetInstance().accessToken = downloadHandler.text;
                    //AuthManager.GetInstance().refreshToken = tokenGet.data.refreshToken;

                    
                };
                StartCoroutine(HttpManager.GetInstance().Post(info2));

            }
        }
    }
}
