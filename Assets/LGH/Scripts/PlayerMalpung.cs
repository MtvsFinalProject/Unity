using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using GH;

public class PlayerMalpung : MonoBehaviourPun, IPunObservable
{
    //��ǳ��
    public GameObject malpungPanel;
    public TMP_Text malpungText;
    public TMP_Text playerNameText;
    private string playerName;
    private string playerNamePun;

    //��ǳ�� �ð�
    public float currtMalpungTime = 5.0f;
    private float maxMalpungTime = 5.0f;

    bool onMalpung = true;
    //bool onMalpung_Pun;

    void Start()
    {
        //��ǳ�� ����
        malpungPanel.SetActive(false);

        // ��ǳ �ؽ�Ʈ �ʱ�ȭ
        malpungText.text = "";

        //if (photonView.IsMine)
        //{
        //    playerNameText.text = playerName;
        //}
        //else
        //{
        //    playerNameText.text = playerNamePun;
        //}
        playerName = DataManager.instance.playerName;

        Invoke("RPC_NameText", 0.2f);

    }

    // Update is called once per frame
    void Update()
    {

        OnMalpung();
        malpungPanel.SetActive(onMalpung);

        

        

    }
    
    //��ǳ�� �����
    private void OnMalpung()
    {
        currtMalpungTime += Time.deltaTime;
        if (currtMalpungTime < maxMalpungTime)
        {
            onMalpung = true;
        }
        else
        {
            //malpungPanel.SetActive(false);
            onMalpung = false;
        }



    }

    [PunRPC]
    public void MalPungText(string value)
    {
        malpungText.text = value;
        currtMalpungTime = 0;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ���� �����͸� ������ ����(PhotonView.IsMine == true)�ϴ� ���¶��
        if (stream.IsWriting)
        {
            stream.SendNext(playerName);
        }
        //�׷��� �ʰ� ���� �����͸� �����κ��� �о���� ���¶��
        else if (stream.IsReading)
        {
            playerNamePun = (string)stream.ReceiveNext();
        }
    }

    public void RPC_MalPungText(string value)
    {
        photonView.RPC("MalPungText", RpcTarget.All, value);
    }



    [PunRPC]
    public void NameText()
    {
        if (photonView.IsMine)
        {
            playerNameText.text = playerName;
        }
        else
        {
            playerNameText.text = playerNamePun;
        }
    }

    public void RPC_NameText()
    {
        photonView.RPC("NameText", RpcTarget.All);
    }
}
