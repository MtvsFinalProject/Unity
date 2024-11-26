using GH;
using MJ;
using Photon.Pun;
using SW;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static HttpManager;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr instance;
    

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
    }

    void Update()
    {

    }

    public void Login()
    {

        SceneManager.LoadScene(2);

    }
    public void ClassIn(string roomName = null, int mapId = -1)
    {
        DataManager.instance.playerCurrChannel = DataManager.instance.playerName;
        if (roomName == null)
        {
            PhotonNetMgr.instance.roomName = DataManager.instance.playerName;
            DataManager.instance.mapId = AuthManager.GetInstance().userAuthData.userInfo.id;
        }
        else
        {
            PhotonNetMgr.instance.roomName = roomName;
            DataManager.instance.mapId = mapId;
        }
        DataManager.instance.MapTypeState = DataManager.MapType.MyClassroom;

        //DataManager.instance.player = null;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 2;

        SceneUIManager.GetInstance().OnInventoryUI();
        //SceneManager.LoadScene(2);
        //PhotonNetMgr.instance.CreateRoom();
    }

    public void SquareIn()
    {
        DataManager.instance.playerCurrChannel = "������ ����";
        PhotonNetMgr.instance.roomName = "������ ����";
        DataManager.instance.mapId = 0;
        DataManager.instance.MapTypeState = DataManager.MapType.Square;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 3;
        SceneUIManager.GetInstance().OffInventoryUI();
    }

    public void SchoolIn(string roomName = null, int mapId = -1)
    {
        //DataManager.instance.playerCurrChannel = DataManager.instance.playerSchool;
        //PhotonNetMgr.instance.roomName = DataManager.instance.playerSchool;
        if (roomName == null)
        {
            PhotonNetMgr.instance.roomName = AuthManager.GetInstance().userAuthData.userInfo.school.schoolName;
            DataManager.instance.mapId = AuthManager.GetInstance().userAuthData.userInfo.school.id;
        }
        else
        {
            PhotonNetMgr.instance.roomName = roomName;
            DataManager.instance.mapId = mapId;
        }
        DataManager.instance.MapTypeState = DataManager.MapType.School;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 1;
        SceneUIManager.GetInstance().OnInventoryUI();
    }

    public void QuizIn(string quizRoomName)
    {
        DataManager.instance.playerCurrChannel = quizRoomName;
        PhotonNetMgr.instance.roomName = quizRoomName;
        DataManager.instance.mapId = 0;
        DataManager.instance.MapTypeState = DataManager.MapType.Quiz;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 4;

        // ���� �濡 ���� Count ���ؼ� ������ Ŭ���̾�Ʈ�� ����

        PlayerAnimation.GetInstance().SettingAvatar();
        SceneUIManager.GetInstance().OffInventoryUI();
    }

    public void QuizSquareIn()
    {
        DataManager.instance.playerCurrChannel = "���� ����";
        PhotonNetMgr.instance.roomName = "���� ����";
        DataManager.instance.mapId = 0;
        DataManager.instance.MapTypeState = DataManager.MapType.QuizSquare;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 5;
        SceneUIManager.GetInstance().OffInventoryUI();

    }

    public void MapContestMapIn(string roomName)
    {
        PhotonNetMgr.instance.roomName = "�� ���׽�Ʈ: " + roomName;
        DataManager.instance.mapId = 0;
        DataManager.instance.MapTypeState = DataManager.MapType.ContestClassroom;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 6;
        SceneUIManager.GetInstance().OffInventoryUI();
    }
}
