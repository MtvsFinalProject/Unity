using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class PhotonChatMgr : MonoBehaviour, IChatClientListener
    {
        //Input Chat InputField
        public InputField inputChat;

        // ChatItem Prefab
        public GameObject chatItemPrefab;

        public Color color;
        public string nick;

        // ChatItem�� �θ� Transfrom
        public RectTransform contentRectTransform;

        //��ü���� PhotonChat ����� ������ �ִ� ����
        ChatClient chatClient;

        // �Ϲ�ä��ä��
        public string currChannel = "��Ÿ";
        void Start()
        {
            // ���� ���� �� ȣ��Ǵ� �Լ� ���
            inputChat.onSubmit.AddListener(OnSbmit);

            // photon chat ������ ����
            PhotonChatConnect();
        }

        void Update()
        {
            // ä�� �������� ���� ������ �����ϱ� ���ؼ� ��� ȣ�� ����� �Ѵ�.
            if(chatClient != null)
            {
                chatClient.Service();
            }
            print(chatClient.PublicChannels[currChannel].Subscribers.Count);

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
            chatClient.AuthValues = new Photon.Chat.AuthenticationValues(nick);
            //����õ�
            chatClient.ConnectUsingSettings(chatAppSettings);

        }
        private void OnSbmit(string s)
        {
            // �г����� ���� ���� Color��
            string nickName = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + nick + "</color>";
           

            //�ӼӸ����� �Ǵ�
            // /w ���̵� �޽����� �ӼӸ�
            string[] text = s.Split(" ", 3);
            string chat = nickName + " : " + s;

            //��ü ä�� ������ ������.
            if (text[0] == "/w")
            {

                chat = nickName + " : " + text[2];
                //�ӼӸ� ������
                chatClient.SendPrivateMessage(text[1], chat);
            }
            else
            {
               
                // �Ϲ�ä���� ������
                chatClient.PublishMessage(currChannel, chat);
            }


            inputChat.text = "";
        }

        private void CreateChatItem(string chat, Color chatColor)
        {

            //s�� ������ ChatItem�� ������
            GameObject go = Instantiate(chatItemPrefab, contentRectTransform);
            ChatItem chatItem = go.GetComponent<ChatItem>();



            // ������ ������Ʈ�� SetText�Լ��� ����
            chatItem.SetText(chat, chatColor);
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
            chatClient.Subscribe(currChannel, 0, -1, new ChannelCreationOptions() { PublishSubscribers = true});
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
                CreateChatItem(messages[i].ToString(), Color.black);
            }
        }

        //�ӼӸ�
        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            //ä�� ������ ���� ��ũ�� �信 ������
            CreateChatItem(message.ToString(), new Color32(255, 0, 255, 255));
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
    }

}