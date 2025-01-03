using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.PackageManager.Requests;
using SW;

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
        // Body  ������
        public string body = "";

        // contentType
        public string contentType = "";
        // ��� ���� �� ȣ��Ǵ� �Լ� ���� ����
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
    // GET : �������� �����͸� ��ȸ ��û
    public IEnumerator Get(HttpInfo info)
    {
        string url = info.url;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Token Authorization ��� �߰�
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().token);

            // ������ ��û ������
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
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
            // Token Authorization ��� �߰�
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().token);

            // ������ ��û ������
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
            DoneRequest(webRequest, info);
        }
    }
 
    public IEnumerator Patch(HttpInfo info)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "PATCH"))
        {

            // ��û ���� ���� (byte[] ���·� ����)
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(info.body);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);

            // Token Authorization ��� �߰�
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().token);

            // Content-Type ����
            webRequest.SetRequestHeader("Content-Type", info.contentType);

            // ���� �ޱ� ���� �ٿ�ε� �ڵ鷯 ����
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // ������ ��û ������
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
            DoneRequest(webRequest, info);
        }
    }
  
    public IEnumerator Delete(HttpInfo info)
    {
        string url = info.url;

        // UnityWebRequest.Delete()�� DELETE ��û ����
        using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
        {
            // Token Authorization ��� �߰�
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().token);

            // ������ ��û ������
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
            DoneRequest(webRequest, info);
        }
    }

    // ���� ���ε�(form-data)
    public IEnumerator UploadFileByFormData(HttpInfo info, string fileName)
    {
        // info.data���� ������ ��ġ
        // info.data �� �ִ� ������ byte �迭�� �о����.
        byte[] data = File.ReadAllBytes(info.body);

        // data�� MultipartForm ���� ����
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, fileName, info.contentType));

        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, formData))
        {
            // Token Authorization ��� �߰�
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().token);

            print("��û");
            // ������ ��û ������
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
            DoneRequest(webRequest, info);            
        }

    }
    // ���� ���ε�(form-data)
    public IEnumerator UploadFileByFormDataByte(HttpInfo info, string fileName, byte[] byteData)
    {
        // info.data���� ������ ��ġ
        // info.data �� �ִ� ������ byte �迭�� �о����.
        byte[] data = byteData;

        // data�� MultipartForm ���� ����
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, fileName, info.contentType));

        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, formData))
        {
            // Token Authorization ��� �߰�
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().token);

            print("��û");
            // ������ ��û ������
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
            DoneRequest(webRequest, info);            
        }

    }

    // ���� ���ε�
    public IEnumerator UploadFileByByte(HttpInfo info)
    {
        // info.data���� ������ ��ġ
        // info.data �� �ִ� ������ byte �迭�� �о����.
        byte[] data = File.ReadAllBytes(info.body);

        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "Post"))
        {
            // Token Authorization ��� �߰�
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().token);

            // ���ε� �ϴ� ������
            webRequest.uploadHandler = new UploadHandlerRaw(data);
            webRequest.uploadHandler.contentType = info.contentType;

            // ���� �޴� ������ ����
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // ������ ��û ������
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
            DoneRequest(webRequest, info);
        }
    }

    public IEnumerator DownloadSprite(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(info.url))
        {
            // Token Authorization ��� �߰�
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().token);

            yield return webRequest.SendWebRequest();
            DoneRequest(webRequest, info);
        }
    }

    public IEnumerator DownloadAudio(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(info.url, AudioType.WAV))
        {
            // Token Authorization ��� �߰�
            webRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.GetInstance().token);

            yield return webRequest.SendWebRequest();
            //DownloadHandlerAudioClip handler = webRequest.downloadHandler as DownloadHandlerAudioClip;
            //handler.audioClip �� audiosource�� �����ϰ� �÷���
            DoneRequest(webRequest, info);
        }
    }
    void DoneRequest(UnityWebRequest webRequest, HttpInfo info)
    {
        // ���࿡ ����� �����̶��
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // ���� �� �����͸� ��û�� Ŭ������ ������.
            if (info.onComplete != null)
            {
                info.onComplete(webRequest.downloadHandler);
            }
            // print(webRequest.downloadHandler.text);
        }
        else
        {
            // �׷��� �ʴٸ� (Error ���)
            // Error �� ������ ���
            Debug.LogError("Net Error : " + webRequest.error); 
        }
    }
}
