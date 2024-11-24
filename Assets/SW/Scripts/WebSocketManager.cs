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

        public string SERVER_ADDRESS { get; } = "ws://My-little-school-dev-env.eba-rfqxtdpp.ap-northeast-2.elasticbeanstalk.com";
        /*
        ws://My-little-school-dev-env.eba-rfqxtdpp.ap-northeast-2.elasticbeanstalk.com
        ws://125.132.216.190:5544
         */
        public WebSocket friendWebSocket;
        public FriendsUI friendsUI;
        public void LogIn(int id)
        {
            try
            {
                friendWebSocket = new WebSocket(SERVER_ADDRESS + "/ws?userId=" + id + "&mapId=" + id + "&mapType=MyClassroom");
                friendWebSocket.OnOpen += (sender, e) =>
                {
                    Debug.Log("������ ����Ǿ����ϴ�.");
                };
                friendWebSocket.OnMessage += Receive;
                friendWebSocket.OnClose += (sender, e) =>
                {
                    Debug.Log("�������� ������ ����Ǿ����ϴ�.");
                };
                friendWebSocket.OnError += (sender, e) =>
                {
                    Debug.LogError("WebSocket ����: " + e.Message);
                };
                if (friendWebSocket == null || !friendWebSocket.IsAlive)
                    friendWebSocket.Connect();
            }
            catch (Exception ex)
            {
                Debug.LogError("WebSocket ���� �� ���� �߻�: " + ex.Message);
            }
        }
        private Queue<string> receiveQueue = new Queue<string>();
        private void Receive(object sender, MessageEventArgs e)
        {
            receiveQueue.Enqueue(e.Data);
        }
        private void Update()
        {
            while (receiveQueue.Count != 0)
            {
                string data = receiveQueue.Dequeue();
                print(data);
                GetReceiveType type = JsonUtility.FromJson<GetReceiveType>(data);
                // ģ�� ���
                if (type.type == "FRIEND_LIST")
                {
                    friendsUI.LoadFriendList(data);
                }
                // ���� ��û
                else if (type.type == "PENDING_REQUESTS")
                {
                    friendsUI.LoadFriendRequest(data);
                }
                // ģ�� ���� �ݹ�
                else if (type.type == "ACCEPT_FRIENDSHIP_REQUEST_CALLBACK")
                {
                    if (friendsUI.gameObject.activeSelf) friendsUI.RefreshFriends();
                }
                // ���� ��û
                else if (type.type == "PENDING_REQUESTS_BY_REQUESTER")
                {
                    friendsUI.LoadFriendRequesting(data);
                }
                // ������ ����
                else if (type.type == "ACCEPT_FRIENDSHIP_REQUEST")
                {
                    if (friendsUI.gameObject.activeSelf) friendsUI.RefreshFriends();
                }
                // ģ�� ����
                else if (type.type == "COMPLETE_DELETE_FRIEND_REQUEST")
                {
                    if (friendsUI.gameObject.activeSelf) friendsUI.RefreshFriends();
                }
                // ��û ���
                else if (type.type == "NOTIFICATION_DELETE_FRIEND_REQUEST")
                {
                    if (friendsUI.gameObject.activeSelf) friendsUI.RefreshFriends();
                }
                // ģ�� ��ġ �̵�
                else if (type.type == "ACCEPT_FRIEND_POS_INFO")
                {
                    if (friendsUI.gameObject.activeSelf) friendsUI.RefreshFriends();
                }
                // ģ�� ����
                else if (type.type == "OFFLINE_USER")
                {
                    if (friendsUI.gameObject.activeSelf) friendsUI.RefreshFriends();
                }
            }
        }
        [Serializable]
        private struct GetReceiveType
        {
            public string type;
        }

        public void Send(WebSocket socket, string jsonMessage)
        {
            try
            {
                if (socket != null && socket.IsAlive)
                {
                    // WebSocket�� ���� ������ ����
                    socket.Send(jsonMessage);
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
                if (friendWebSocket == null)
                    return;

                if (friendWebSocket.IsAlive)
                    friendWebSocket.Close();

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        public void RequestFriend(int userId)
        {
            ToastMessage.OnMessage("ģ�� �߰��� ��û�Ͽ����ϴ�");
            Send(friendWebSocket, "{\"type\": \"FRIEND_REQUEST\", \"requesterId\": " + AuthManager.GetInstance().userAuthData.userInfo.id + ", \"receiverId\": " + userId + "}");
        }
    }
}