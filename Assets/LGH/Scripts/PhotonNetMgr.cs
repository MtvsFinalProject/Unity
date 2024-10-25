using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;

namespace GH
{
    public class PhotonNetMgr : MonoBehaviourPunCallbacks
    {
        // ���� �г���
        public string nickName;

        //�� �̸�
        public string roomName;

        // �� ����Ʈ�� ������ ����Ʈ
        private List<string> roomNames = new List<string>();
        void Start()
        {
            StartLogin();
        }
        void Update()
        {

        }
        public void StartLogin()
        {
            // ������ ���� ����
            PhotonNetwork.GameVersion = "1.0.0";
            PhotonNetwork.NickName = nickName;
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

       
        }


        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            bool roomCheck = false;

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

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            // ���������� ���� ���������.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");

            // �濡 ������ ģ������ ��� 1�� ������ �̵�����
            //PhotonNetwork.LoadLevel();

        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);

            //�� ���忡 ������ ����
            Debug.LogError(message);
        }
    }
}