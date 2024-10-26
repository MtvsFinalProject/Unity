using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerMalpung : MonoBehaviourPun, IPunObservable
{
    //��ǳ��
    public GameObject malpungPanel;
    public TMP_Text malpungText;


    //��ǳ�� �ð�
    public float currtMalpungTime = 5.0f;
    private float maxMalpungTime = 5.0f;

    bool onMalpung = true;
    bool onMalpung_Pun;
    void Start()
    {
        //��ǳ�� ����
        malpungPanel.SetActive(false);

        // ��ǳ �ؽ�Ʈ �ʱ�ȭ
        malpungText.text = "";
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
            stream.SendNext(onMalpung);

        }
        //�׷��� �ʰ� ���� �����͸� �����κ��� �о���� ���¶��
        else if (stream.IsReading)
        {
            //���� �޴� ������� ������ ĳ���� ����� �Ѵ�.
            onMalpung_Pun = (bool)stream.ReceiveNext();
        }
    }

    public void RPC_MalPungText(string value)
    {
        photonView.RPC("MalPungText", RpcTarget.All, value);
    }
}
