using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using SW;

namespace GH
{


    public class PlayerMalpung : MonoBehaviourPun, IPunObservable
    {
        //��ǳ��
        public GameObject malpungPanel;
        public TMP_Text malpungText;
        public TMP_Text playerNameText;

        //��ǳ�� �ð�
        public float currtMalpungTime = 5.0f;
        private float maxMalpungTime = 5.0f;

        bool onMalpung = true;
        //bool onMalpung_Pun;

        [Header("�Է¸�ǳ")]
        public GameObject inputMalpung;
        public TMP_InputField malpungInputField;


        void Start()
        {
            //��ǳ�� ����
            malpungPanel.SetActive(false);

            // ��ǳ �ؽ�Ʈ �ʱ�ȭ
            malpungText.text = "";
            playerNameText.text = AuthManager.GetInstance().userAuthData.userInfo.name;
        }

        // Update is called once per frame
        void Update()
        {

            OnMalpung();
            malpungPanel.SetActive(onMalpung);
        }
        public void PlayerNameSet()
        {
            playerNameText.text = AuthManager.GetInstance().userAuthData.userInfo.name;
            PhotonNetMgr.instance.ChangeNickname(AuthManager.GetInstance().userAuthData.userInfo.name);

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
            }
            //�׷��� �ʰ� ���� �����͸� �����κ��� �о���� ���¶��
            else if (stream.IsReading)
            {
            }
        }

        public void RPC_MalPungText(string value)
        {
            photonView.RPC(nameof(MalPungText), RpcTarget.All, value);
        }




    }

}
