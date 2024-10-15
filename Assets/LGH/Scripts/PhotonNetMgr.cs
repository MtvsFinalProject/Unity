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
        void Start()
        {
            StartLogin();
        }
        void Update()
        {

        }
        public void StartLogin()
        {
            PhotonNetwork.GameVersion = "1.0.0";
            PhotonNetwork.NickName = nickName;
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnected()
        {
            base.OnConnected();

            print(MethodInfo.GetCurrentMethod().Name + " is call!");
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

            //���� ���� �����.
            RoomOptions roomOpt = new RoomOptions();
            roomOpt.MaxPlayers = 20;
            roomOpt.IsOpen = true;
            roomOpt.IsVisible = true;

            PhotonNetwork.CreateRoom(roomName, roomOpt, TypedLobby.Default);
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();

            //���������� ���� ���������.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");
        }
    }
}