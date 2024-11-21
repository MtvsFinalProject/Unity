using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
namespace SW
{
    public class WebSocketManager : MonoBehaviour
    {
        #region �̱���
        static WebSocketManager instance;
        public static WebSocketManager GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "WebSocketManager";
                go.AddComponent<WebSocketManager>();
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
        #endregion

        public string SERVER_ADDRESS { get; } = "ws://125.132.216.190:5544";
        public WebSocket webSocket;
        private void Start()
        {
            try
            {
                webSocket = new WebSocket(SERVER_ADDRESS + "/ws?userId=test1");
                webSocket.OnOpen += (sender, e) =>
                {
                    Debug.Log("������ ����Ǿ����ϴ�.");
                };
                webSocket.OnMessage += Receive;
                webSocket.OnClose += (sender, e) =>
                {
                    Debug.Log("�������� ������ ����Ǿ����ϴ�.");
                };
                webSocket.OnError += (sender, e) =>
                {
                    Debug.LogError("WebSocket ����: " + e.Message);
                };
                if (webSocket == null || !webSocket.IsAlive)
                    webSocket.Connect();
            }
            catch (Exception ex)
            {
                Debug.LogError("WebSocket ���� �� ���� �߻�: " + ex.Message);
            }
        }
        private void Receive(object sender, MessageEventArgs e)
        {
            print(e.Data);
        }
        public void Send()
        {
            try
            {
                if (webSocket != null && webSocket.IsAlive)
                {

                    // ��ü�� JSON ���ڿ��� ��ȯ
                    string jsonMessage = "test";

                    // WebSocket�� ���� ������ ����
                    webSocket.Send(jsonMessage);

                    Debug.Log("���� �޽���: " + jsonMessage);
                }
                else
                {
                    Debug.LogError("������ ������� �ʾҽ��ϴ�.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("�޽��� ���� �� ���� �߻�: " + ex.Message);
            }
        }
        private void OnApplicationQuit()
        {
            DisconncectServer();
        }
        public void DisconncectServer()
        {
            try
            {
                if (webSocket == null)
                    return;

                if (webSocket.IsAlive)
                    webSocket.Close();

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Send();
            }
        }
    }
}