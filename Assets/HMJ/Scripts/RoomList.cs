using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class RoomList : MonoBehaviourPunCallbacks
{
    Action<List<RoomInfo>> callback = null;
    LoadBalancingClient client = null;

    static RoomList instance;
    public static RoomList GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(client != null)
        {
            client.Service();
        }
    }


    public void GetRoomList(Action<List<RoomInfo>> callback)
    {
        if (client != null && client.Connect())
            return;
        this.callback = callback;
        client = new LoadBalancingClient();
        client.AddCallbackTarget(this);
        client.StateChanged += OnStateChanged;
        client.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
        client.AppVersion = PhotonNetwork.NetworkingClient.AppVersion;
        client.EnableLobbyStatistics = true;

        client.ConnectToRegionMaster(PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion);
    }

    void OnStateChanged(ClientState previousState, ClientState state)
    {
        if(state == ClientState.ConnectedToMasterServer)
            client.OpJoinLobby(null);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("���� Ŭ���̾�Ʈ �� ����Ʈ ������Ʈ");

        if (callback != null)
        {
            callback(roomList);
        }

        // �۾� �� - ���� Ŭ���̾�Ʈ ����
        if (client != null)
            client.Disconnect();
    }
}
