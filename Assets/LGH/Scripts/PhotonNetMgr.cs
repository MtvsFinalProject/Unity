using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using MJ;
using SW;

namespace GH
{
    public class PhotonNetMgr : MonoBehaviourPunCallbacks
    {

        //��� �� �̸�
        public TMP_Text topMenuText;

        // ���� �г���
        private string playerName;

        //�� �̸�
        public string roomName;

        // �� ����Ʈ�� ������ ����Ʈ
        private List<string> roomNames = new List<string>();

        public static PhotonNetMgr instance;

        public int sceneNum;

        //�ε� �г�
        public GameObject loadingPanel;
        public Loading loading;

        public SceneUIManager sceneUIManager;

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
            playerName = AuthManager.GetInstance().userAuthData.userInfo.id.ToString();
            roomName = AuthManager.GetInstance().userAuthData.userInfo.id.ToString();
            StartLogin();

            //�ε�ä�� Ȱ��ȭ
            loadingPanel.SetActive(true);
            loading = loadingPanel.GetComponent<Loading>();
        }
        void Update()
        {

        }
        public void StartLogin()
        {
            // ������ ���� ����
            PhotonNetwork.GameVersion = "1.0.0";
            PhotonNetwork.NickName = playerName;
            PhotonNetwork.AutomaticallySyncScene = false;
            // ������ ������ ��û
            PhotonNetwork.ConnectUsingSettings();
        }



        public override void OnConnected()
        {
            base.OnConnected();

            print(MethodInfo.GetCurrentMethod().Name + " is call!");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);

            //  ���� ���� ���
            Debug.LogError("Disconnected from Server - " + cause);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            print(MethodInfo.GetCurrentMethod().Name + " is call!");

            //������ �κ�� ����
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            //���� �κ� ������ �˸���.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");

            //PhotonNetwork.JoinOrCreateRoom()
        }


        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            print(MethodInfo.GetCurrentMethod().Name + " is call!");
            // ���� ����Ʈ�� ����� ������Ʈ
            roomNames.Clear();
            foreach (RoomInfo room in roomList)
            {
                if (!room.RemovedFromList)
                {
                    // �� �̸� ����
                    roomNames.Add(room.Name);
                }
            }

            SuchRoom();
        }
        public void SuchRoom()
        {
            bool roomCheck = false;

            foreach (string roomN in roomNames)
            {
                if (roomN == roomName)
                {

                    JoinRoom();
                    roomCheck = true;
                }
            }

            if (!roomCheck)
            {
                CreateRoom();

            }
        }

        public void CreateRoom()
        {
            //���� ���� �����.
            RoomOptions roomOpt = new RoomOptions();
            roomOpt.MaxPlayers = 20;
            roomOpt.IsOpen = true;
            roomOpt.IsVisible = true;
            roomOpt.CleanupCacheOnLeave = false;
            PhotonNetwork.CreateRoom(roomName, roomOpt, TypedLobby.Default);
        }

        public void JoinRoom()
        {
            //�濡 ����.
            PhotonNetwork.JoinRoom(roomName);

        }



        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();

            //���������� ���� ���������.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");


        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            JoinRoom();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            topMenuText.text = roomName;
            if (topMenuText.text == AuthManager.GetInstance().userAuthData.userInfo.id.ToString())
            {
                topMenuText.text = AuthManager.GetInstance().userAuthData.userInfo.nickname;
            }



            DataManager.instance.playerCurrChannel = roomName;
            PhotonChatMgr.instance.currChannel = roomName;
            PhotonChatMgr.instance.ChatChannelChange();

            // ���������� ���� ���������.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");
            GameObject joyStick = GameObject.Find("Variable Joystick");

            if (joyStick != null)
            {
                joyStick.GetComponent<Joystick>().ResetJoystick();
            }

            PlayerAnimation.GetInstance().SettingAvatar();
            sceneUIManager.MapTutorial();

        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);

            //�� ���忡 ������ ����
            Debug.LogError(message);
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();

            print(MethodInfo.GetCurrentMethod().Name + " is call!");
            //CreateRoom();
            //// �濡 ������ ģ������ ��� 1�� ������ �̵�����
            //�ε�â Ȱ��ȭ
            PhotonNetwork.LoadLevel(sceneNum);
            loadingPanel.SetActive(true);
            StartCoroutine(loading.SceneMove());
            GameManager.instance.CoSpwamPlayer();
            VoiceManager.GetInstance().MoveScene();

        }
        // �г��� ���� �޼���
        //public void ChangeNickname(string newNickname)
        //{
        //    // ���� ���� �÷��̾��� �г��� ����
        //    if (PhotonNetwork.IsMessageQueueRunning)
        //    {
        //        // ���� ��Ʈ��ũ �� �÷��̾� �г��� ����
        //        PhotonNetwork.LocalPlayer.NickName = newNickname;

        //    }
        //}




    }

}