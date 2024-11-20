using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using SW;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;

namespace GH
{

    public class PhotonChatMgr : MonoBehaviourPun, IChatClientListener
    {
        public enum Loginstep
        {
            All,
            School,
            Private
        }

        public Loginstep currentLogin;

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
        public string currChannel = "All";


        //ê �α� ��
        public GameObject chatLogView;
        bool chatLogOn = false;

        //��ǳ��
        public GameObject malpungPanel;
        public TMP_Text malpungText;
        PlayerMalpung playerMalpung;

        public RectTransform chatMainPanelRecttransform;
        public RectTransform chatPanelRecttransform;

        public List<GameObject> chatList = new List<GameObject>();

        //ä�� ä�� ��ȯ ��ư
        public Button chatChannel;
        private TMP_Text chatChannelText;

        public static PhotonChatMgr instance;
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
        void Start()
        {
            playerName = DataManager.instance.playerName;
            //
            //currChannel = DataManager.instance.playerSchool;
            // ���� ���� �� ȣ��Ǵ� �Լ� ���
            inputChat.onSubmit.AddListener(OnSubmit);

            // photon chat ������ ����
            PhotonChatConnect();

            chatChannel.onClick.AddListener(ChatChannelChange);
            chatChannelText = chatChannel.gameObject.GetComponentInChildren<TMP_Text>();
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
        public void ChatChannelChange()
        {
            currentLogin = currentLogin == Loginstep.All ? Loginstep.School : Loginstep.All;
            switch (currentLogin)
            {
                case Loginstep.All:
                    JoinChatRoom(DataManager.instance.playerCurrChannel);
                    chatChannelText.text = "��ü";
                    print("��ü");

                    break;
                case Loginstep.School:
                    chatChannelText.text = "�б�";
                    print("�б�");
                    JoinChatRoom(AuthManager.GetInstance().userAuthData.userInfo.school.schoolName);
                    break;
                case Loginstep.Private:
                    break;
            }
        }

        void PhotonChatConnect()
        {
            print("Ŀ��Ʈ ����");
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
            print("���� �õ�");

        }

        public void JoinChatRoom(string newRoom)
        {
            //ä�� �α� �ʱ�ȭ
            for (int i = 0; i < chatList.Count; i++)
            {
                Destroy(chatList[i]);
            }

            chatList.Clear();
            if (!string.IsNullOrEmpty(currChannel))
            {
                chatClient.Unsubscribe(new string[] { currChannel });
            }
            chatClient.Disconnect();

            currChannel = newRoom;
            PhotonChatConnect();
            print(newRoom + "�� �������!");
            //chatClient.Subscribe(new string[] { currChannel });
        }
        public enum ChatMode
        {
            Default, AIChatBot
        }
        public ChatMode ChatModeState { get; set; }
        public AIChatBotNPC NPC { get; set; }
        private void OnSubmit(string s)
        {
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

            if (ChatModeState == ChatMode.Default)
            {
                ChatLogSeverPost(s);

                //�ӼӸ����� �Ǵ�
                // /w ���̵� �޽����� �ӼӸ�
                string[] text = s.Split(" ", 2);
                string chat = playerName + "  " + s;

                //�ӼӸ� 
                if (s[0] == '@')
                {
                    chat = text[0] + "  " + text[1];
                    //�ӼӸ� ������
                    chatClient.SendPrivateMessage(text[0].Remove(0, 1), chat);
                    print(text[0].Remove(0, 1) + "���� �ӼӸ�");
                    inputChat.text = "";
                }
                else
                {
                    // �Ϲ�ä���� ������
                    chatClient.PublishMessage(currChannel, chat);
                    //��ǳ���� �ؽ�Ʈ�� �ִ´�.
                    playerMalpung.RPC_MalPungText(inputChat.text);
                    inputChat.text = "";
                }
            }
            else if (ChatModeState == ChatMode.AIChatBot)
            {
                NPC.ReqChat(s);
                playerMalpung.MalPungText(s);
                inputChat.text = "";
            }
        }

        private void CreateChatItem(string chat, Color chatColor)
        {

            //s�� ������ ChatItem�� ������
            GameObject go = Instantiate(chatItemPrefab, contentRectTransform);
            ChatItem chatItem = go.GetComponent<ChatItem>();
            chatList.Add(go);


            // ������ ������Ʈ�� SetText�Լ��� ����
            chatItem.SetText(chat, chatColor);
            //content Size Fitter Ʈ������ ���ΰ�ħ
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        }

        public void DebugReturn(DebugLevel level, string message)
        {
        }
        public void OnChatStateChange(ChatState state)
        {
            Debug.Log($"Chat State Changed: {state}");
        }

        public void OnErrorInfo(ErrorInfo errorInfo)
        {
            Debug.LogError($"Error: {errorInfo.Info}");
        }
        public void OnDisconnected()
        {
            PhotonChatConnect();
            print("����!!!!!!");
        }

        //ä�� ������ ���� �����ϸ� ȣ��Ǵ� �Լ�
        public void OnConnected()
        {
            print("ä�� ���� ���� ����!");
            // Ư�� ä�ο� ����(����)

            chatClient.Subscribe(currChannel, 0, -1, new ChannelCreationOptions() { PublishSubscribers = true });


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
            chatHeight = chatLogOn ? 1000 : 0;
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
            chatLogInfo.senderId = AuthManager.GetInstance().userAuthData.userInfo.id;

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/chat-log";
            info.body = JsonUtility.ToJson(chatLogInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }


        //��ǲâ�� �÷��̾� �̸� �߰��ϱ�
        public void OneToOneChat(string playerName)
        {

            inputChat.text = "@" + playerName + " ";
        }

        // ä�� �Է� On
        public void OnChatInput()
        {
            inputChat.Select();
            TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
        }

        public void PrivateRoomIn(string s)
        {
            JoinChatRoom(s);
            chatChannelText.text = s;
            print(s);
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