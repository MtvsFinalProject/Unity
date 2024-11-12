using GH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr instance;
    [Header("�α��� ��")]
    public TMP_InputField nameInputField;
    public TMP_InputField schoolInputField;
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

    // Update is called once per frame
    void Update()
    {

    }
    public void Login()
    {
        //�� �Ѿ��
        DataManager.instance.playerName = nameInputField.text;
        DataManager.instance.playerSchool = schoolInputField.text;

        SceneManager.LoadScene(2);
    }
    public void ClassIn()
    {
        DataManager.instance.playerCurrChannel = DataManager.instance.playerName;
        PhotonNetMgr.instance.roomName = DataManager.instance.playerName;

        //DataManager.instance.player = null;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 2;
        //SceneManager.LoadScene(2);
        //PhotonNetMgr.instance.CreateRoom();
    }

    public void SquareIn()
    {
        DataManager.instance.playerCurrChannel = "������ ����";
        PhotonNetMgr.instance.roomName = "������ ����";

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 3;
    }

    public void SchoolIn()
    {
        DataManager.instance.playerCurrChannel = DataManager.instance.playerSchool;
        PhotonNetMgr.instance.roomName = DataManager.instance.playerSchool;

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 1;
    }

    public void QuizIn()
    {
        DataManager.instance.playerCurrChannel = "����";
        PhotonNetMgr.instance.roomName = "����";

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 4;
    }

    public void QuizSquareIn()
    {
        DataManager.instance.playerCurrChannel = "���� ����";
        PhotonNetMgr.instance.roomName = "���� ����";

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 5;
    }

    public void MapContestMapIn(string roomName)
    {
        PhotonNetMgr.instance.roomName = "�� ���׽�Ʈ: " + roomName;

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 6;
    }
}
