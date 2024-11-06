using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;

namespace GH
{

    public class PhotonChatMgr : MonoBehaviourPun, IChatClientListener
    {
        //Input Chat InputField
        public TMP_InputField inputChat;
        // ChatItem Prefab
        public GameObject chatItemPrefab;

        public Color color;
        public string playerName;

        // ChatItem�� �θ� Transfrom
        public RectTransform contentRectTransform;

        //��ü���� PhotonChat ����� ������ �ִ� ����
        ChatClient chatClient;

        // �Ϲ�ä��ä��
        public string currChannel = "�Ǳ����б�";

        //ê �α� ��
        public GameObject chatLogView;
        bool chatLogOn = false;

        //��ǳ��
        public GameObject malpungPanel;
        public TMP_Text malpungText;
        PlayerMalpung playerMalpung;

        public RectTransform chatMainPanelRecttransform;
        public RectTransform chatPanelRecttransform;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            playerName = DataManager.instance.playerName;
            //
            currChannel = DataManager.instance.playerSchool;
            // ���� ���� �� ȣ��Ǵ� �Լ� ���
            inputChat.onSubmit.AddListener(OnSubmit);

            // photon chat ������ ����
            PhotonChatConnect();


        }

        void Update()
        {
            // ä�� �������� ���� ������ �����ϱ� ���ؼ� ��� ȣ�� ����� �Ѵ�.
            if (chatClient != null)
            {
                chatClient.Service();
            }
            //print(chatClient.PublicChannels[currChannel].Subscribers.Count);

            //�÷��̾��� ��ǳ���� ã�´�.
            if (malpungPanel == null)
            {
                if (DataManager.instance && DataManager.instance.player)
                {
                    playerMalpung = DataManager.instance.player.GetComponent<PlayerMalpung>();
                    malpungPanel = playerMalpung.malpungPanel;
                    malpungText = playerMalpung.malpungText;
                }
            }
        }

        void PhotonChatConnect()
        {
            //���� ���� ��������
            AppSettings photonSettings = PhotonNetwork.PhotonServerSettings.AppSettings;

            // �� ������ ������ ChatAppSettings ����
            ChatAppSettings chatAppSettings = new ChatAppSettings();
            chatAppSettings.AppIdChat = photonSettings.AppIdChat;
            chatAppSettings.AppVersion = photonSettings.AppVersion;
            chatAppSettings.FixedRegion = photonSettings.FixedRegion;
            chatAppSettings.NetworkLogging = photonSettings.NetworkLogging;
            chatAppSettings.Protocol = photonSettings.Protocol;
            chatAppSettings.EnableProtocolFallback = photonSettings.EnableProtocolFallback;
            chatAppSettings.Server = photonSettings.Server;
            chatAppSettings.Port = (ushort)photonSettings.Port;
            chatAppSettings.ProxyServer = photonSettings.ProxyServer;

            // ChatClient ������
            chatClient = new ChatClient(this);
            // �г��� ����
            chatClient.AuthValues = new Photon.Chat.AuthenticationValues(playerName);
            //����õ�
            chatClient.ConnectUsingSettings(chatAppSettings);

        }
        private void OnSubmit(string s)
        {
            ChatLogSeverPost(s);
            // ä��â�� �ƹ��͵� ������ �Լ��� ������.
            if (inputChat.text.Length < 1)
                return;
            // �г����� ���� ���� Color��
            //string nickName;
            //if (playerName == DataManager.instance.playerName)
            //{
            //    nickName = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + playerName + "</color>";
            //}
            //else
            //{
            //    nickName = playerName;
            //}


            //�ӼӸ����� �Ǵ�
            // /w ���̵� �޽����� �ӼӸ�
            string[] text = s.Split(" ", 3);
            string chat = playerName + " : " + s;

            //��ü ä�� ������ ������.
            if (text[0] == "/w")
            {

                chat = playerName + " : " + text[2];
                //�ӼӸ� ������
                chatClient.SendPrivateMessage(text[1], chat);
            }
            else
            {

                // �Ϲ�ä���� ������
                chatClient.PublishMessage(currChannel, chat);
            }

            //��ǳ���� �ؽ�Ʈ�� �ִ´�.
            playerMalpung.RPC_MalPungText(inputChat.text);
            inputChat.text = "";
        }

        private void CreateChatItem(string chat, Color chatColor)
        {

            //s�� ������ ChatItem�� ������
            GameObject go = Instantiate(chatItemPrefab, contentRectTransform);
            ChatItem chatItem = go.GetComponent<ChatItem>();



            // ������ ������Ʈ�� SetText�Լ��� ����
            chatItem.SetText(chat, chatColor);
            //content Size Fitter Ʈ������ ���ΰ�ħ
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        }

        public void DebugReturn(DebugLevel level, string message)
        {
        }

        public void OnDisconnected()
        {
        }

        //ä�� ������ ���� �����ϸ� ȣ��Ǵ� �Լ�
        public void OnConnected()
        {
            print("ä�� ���� ���� ����!");
            // Ư�� ä�ο� ����(����)
            chatClient.Subscribe(currChannel, 0, -1, new ChannelCreationOptions() { PublishSubscribers = true });
        }

        public void OnChatStateChange(ChatState state)
        {
        }

        // Ư�� ä�ο� �޽����� ���� �� ȣ��Ǵ� �Լ�
        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                // ä�� ������ ���� ��ũ�� �信 �ø���
                CreateChatItem(messages[i].ToString(), color);
            }
        }

        //�ӼӸ�
        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            //ä�� ������ ���� ��ũ�� �信 ������
            CreateChatItem(message.ToString(), color);
        }

        //ä���� ������ ��
        public void OnSubscribed(string[] channels, bool[] results)
        {
        }

        //ä�ο��� ���� ��
        public void OnUnsubscribed(string[] channels)
        {
        }

        //ģ�� ���� ���°� �ٲ�� ȣ��
        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
        }

        //������ ���� �ִ� ä�ο� ������ �� ȣ��Ǵ� �Լ�
        public void OnUserSubscribed(string channel, string user)
        {
        }

        //������ ���� �ִ� ä�ο��� ������ �� ȣ��Ǵ� �Լ�
        public void OnUserUnsubscribed(string channel, string user)
        {

        }

        //ä�÷α� Ű��
        public void ChatLogActive()
        {
            RectTransform chatRectTransform = chatLogView.GetComponent<RectTransform>();
            float chatHeight = 0;
            // ê�α�â�� ���������� Ű�� ���������� Ų��.
            chatLogOn = chatLogOn ? false : true;

            //ê �α��� ���̷� ê �α׸� Ȱ��ȭ * ��Ƽ�긦 ���� ��ũ��Ʈ�� �������ͼ� ������ ����.
            chatHeight = chatLogOn ? 820 : 0;
            //chatLogView.GetComponentAtIndex<Image>(0).raycastTarget = chatLogOn ? true : false;
            chatRectTransform.GetChild(0).GetComponent<Image>().raycastTarget = chatLogOn ? true : false;
            chatRectTransform.sizeDelta = new Vector2(chatRectTransform.sizeDelta.x, chatHeight);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatMainPanelRecttransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatPanelRecttransform);



        }

        public void ChatLogSeverPost(string s)
        {
            ChatLogInfo chatLogInfo = new ChatLogInfo();
            chatLogInfo.message = s;
            //chatLogInfo.timestamp
            chatLogInfo.channel = currChannel;
            chatLogInfo.chatType = "PRIVATE";
            chatLogInfo.senderId = AuthManager.GetInstance().userAuthData.userInfo.userId;

             HttpInfo info = new HttpInfo();
            info.url = "http://125.132.216.190:5544/chat-log";
            info.body = JsonUtility.ToJson(chatLogInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }
    }

    [Serializable]
    public struct ChatLogInfo
    {
        public int senderId;
        public int receiverId;
        public string message;
        public string timestamp;
        public string channel;
        public string chatType;

    }

}