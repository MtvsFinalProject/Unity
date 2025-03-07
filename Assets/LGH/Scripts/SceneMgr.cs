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
        SoundManager.instance.StopBgmSound();
        SoundManager.instance.PlayBgmSound(DataManager.MapType.MyClassroom);

        StartCoroutine(SpawnPlayer());
    }
    public void ClassIn(string roomName = null, int mapId = -1)
    {
        DataManager.instance.playerCurrChannel = AuthManager.GetInstance().userAuthData.userInfo.id.ToString();
        if (roomName == null)
        {
            PhotonNetMgr.instance.roomName = AuthManager.GetInstance().userAuthData.userInfo.id.ToString();
            DataManager.instance.mapId = AuthManager.GetInstance().userAuthData.userInfo.id;
            // 내 방
            SceneUIManager.GetInstance().OnInventoryUI();
            SceneUIManager.GetInstance().OnOtherInventorySettingUI();
        }
        else
        {
            PhotonNetMgr.instance.roomName = roomName;
            DataManager.instance.mapId = mapId;
            SceneUIManager.GetInstance().OffOtherInventorySettingUI();
            SceneUIManager.GetInstance().OffInventoryUI();
            // 다른 사람 방
        }
        DataManager.instance.MapTypeState = DataManager.MapType.MyClassroom;

        //DataManager.instance.player = null;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 2;
        
        //SceneManager.LoadScene(2);
        //PhotonNetMgr.instance.CreateRoom();
    }

    public void SquareIn()
    {
        DataManager.instance.playerCurrChannel = "만남의 광장";
        PhotonNetMgr.instance.roomName = "만남의 광장";
        DataManager.instance.mapId = 0;
        DataManager.instance.MapTypeState = DataManager.MapType.Square;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 3;
        SceneUIManager.GetInstance().OffOtherInventorySettingUI();
        SceneUIManager.GetInstance().OffInventoryUI();
        // GameManager.instance.coSpawnSoccer();
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
        SceneUIManager.GetInstance().OffOtherInventorySettingUI();
    }

    public void QuizIn(string quizRoomName)
    {
        DataManager.instance.playerCurrChannel = quizRoomName;
        PhotonNetMgr.instance.roomName = quizRoomName;
        DataManager.instance.mapId = 0;
        DataManager.instance.MapTypeState = DataManager.MapType.Quiz;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 4;

        // 퀴즈 방에 따라 Count 구해서 마스터 클라이언트로 전달

        PlayerAnimation.GetInstance().SettingAvatar();
        SceneUIManager.GetInstance().OffOtherInventorySettingUI();
        SceneUIManager.GetInstance().OffInventoryUI();
    }

    public void QuizSquareIn()
    {
        DataManager.instance.playerCurrChannel = "퀴즈 광장";
        PhotonNetMgr.instance.roomName = "퀴즈 광장";
        DataManager.instance.mapId = 0;
        DataManager.instance.MapTypeState = DataManager.MapType.QuizSquare;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 5;
        SceneUIManager.GetInstance().OffOtherInventorySettingUI();
        SceneUIManager.GetInstance().OffInventoryUI();
    }

    public void MapContestMapIn(string roomName)
    {
        PhotonNetMgr.instance.roomName = "맵 콘테스트: " + roomName;
        DataManager.instance.mapId = 0;
        DataManager.instance.MapTypeState = DataManager.MapType.ContestClassroom;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 6;
        SceneUIManager.GetInstance().OffOtherInventorySettingUI();
        SceneUIManager.GetInstance().OffInventoryUI();
    }

    IEnumerator SpawnPlayer()
    {
        //GameManager-instance 생성 완료될 때까지 기다린다.
        yield return new WaitUntil(() => { return GameManager.instance; });

        GameManager.instance.CoSpwamPlayer();
    }
}
