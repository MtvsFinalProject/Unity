using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using SW;
using static UnityEngine.Rendering.DebugUI;
using static HttpManager;
using UnityEngine.Networking;

namespace GH
{


    public class PlayerMalpung : MonoBehaviourPun, IPunObservable
    {
        public UserInfoData getUserInfo;

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

            //PlayerNicknameSet();
            RPC_PlayerNicknameSet();
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

        [PunRPC]
        public void PlayerNicknameSet()
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/" + photonView.Owner.NickName;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                print(jsonData);
                //jsonData�� PostInfoArray ������ �ٲ���.
                getUserInfo = JsonUtility.FromJson<UserInfoData>(jsonData);

                playerNameText.text = getUserInfo.data.nickname;
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        

        }

        public void RPC_PlayerNicknameSet()
        {
            photonView.RPC(nameof(PlayerNicknameSet), RpcTarget.All);

        }


    }

}
