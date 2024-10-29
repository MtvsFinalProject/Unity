using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using TMPro;

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
            roomName = DataManager.instance.playerSchool;
            StartLogin();
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


            // ���������� ���� ���������.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");


           

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
            PhotonNetwork.LoadLevel(2);
            GameManager.instance.CoSpwamPlayer();
        }
    }
}