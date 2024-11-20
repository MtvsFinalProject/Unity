using MJ;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using SW;

namespace GH
{

    public class PrivateRoom : MonoBehaviourPunCallbacks, IPunObservable
    {
        public List<GameObject> playersList = new List<GameObject>();
        private GameObject passWordPanel;

        private PrivateRoomPanel privateRoomPanel;

        //�� �н�����
        public string roomPassword = "99999";
        //�� �н����� ��ǲ �ʵ�
        private TMP_InputField passWordInputField;

        //�� �н����� �ȳ� �ؽ�Ʈ
        private TMP_Text passWordText;

        // �н����� Ʋ���� �� �ؽ�Ʈ
        private TMP_Text passWordWrongText;

        //�н����� ���� ��ư
        private Button passWordSubmitButton;
        private Button passWordExitButton;

        private BoxCollider2D boxCollider;

        public bool activeRoom = false;

        public GameObject playerMine;

        private GameObject darkSprite;

        public int roomNum;

        // �����̺� �� ����(�ӽ�)
        public bool playerCheck = false;

        Vector3 enterPosition;

        void Start()
        {
            passWordPanel = GameObject.Find("UIManager").GetComponent<SceneUIManager>().privateRoomPanel;
            privateRoomPanel = passWordPanel.GetComponent<PrivateRoomPanel>();

            passWordInputField = privateRoomPanel.passWordInputField;
            passWordText = privateRoomPanel.passWordText;
            passWordWrongText = privateRoomPanel.passWordWrongText;
            passWordSubmitButton = privateRoomPanel.passWordSubmitButton;
            passWordExitButton = privateRoomPanel.passWordExitButton;

            passWordSubmitButton.onClick.AddListener(PassWordCheck);
            passWordExitButton.onClick.AddListener(PassWordExit);
            boxCollider = GetComponent<BoxCollider2D>();
            darkSprite = transform.GetChild(0).gameObject;
           // StartCoroutine(PlayerSuch());
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if (!PhotonNetwork.IsMasterClient) photonView.RPC(nameof(ReqSync), PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        [PunRPC]
        public void ReqSync(int actorNumber)
        {
            photonView.RPC(nameof(ResSync), PhotonNetwork.CurrentRoom.GetPlayer(actorNumber), roomPassword);
        }
        [PunRPC]
        public void ResSync(string password)
        {
            roomPassword = password;
        }

        private void OnUI()
        {
            passWordInputField.text = "";
            print(passWordPanel.name);
            passWordPanel.SetActive(true);

            if (activeRoom == false)
            {
                passWordText.text = "��й�ȣ �������ּ���";
            }
            else
            {
                passWordText.text = "��й�ȣ�� �Է����ּ���";
            }
        }

        [PunRPC]
        public void PassWordUpload(string s)
        {
            PrivateRoomManager.instance.privateRooms[roomNum].roomPassword = s;
        }

        public void PassWordCheck()
        {
            if (playersList.Count == 1)
            {
               
                if (playerCheck)
                {
                    photonView.RPC(nameof(PassWordUpload), RpcTarget.All, passWordInputField.text);
                    if (playerMine.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        passWordPanel.SetActive(false);

                        playerMine.transform.position = gameObject.transform.position - new Vector3(0, 1.5f, 0);
                        playerCheck = false;

                        // ����
                        SceneUIManager.GetInstance().OnVoicePanel();
                    }
                }


            }
            if (activeRoom)
            {
                if (playerMine.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (passWordInputField.text == roomPassword)
                    {
                        passWordPanel.SetActive(false);
                        playerCheck = false;
                        playerMine.transform.position = gameObject.transform.position - new Vector3(0, 1.5f, 0);

                        // ����
                        SceneUIManager.GetInstance().OnVoicePanel();

                    }
                    else
                    {

                        passWordWrongText.gameObject.SetActive(true);

                    }
                }

            }

        }

        public void PassWordExit()
        {
            passWordPanel.SetActive(false);
            if (playerCheck)
            {
                if (playerMine.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    darkSprite.SetActive(false);
                    playerMine.transform.position = gameObject.transform.position - new Vector3(0, 4f, 0);
                    playerMine = null;
                    playerCheck = false;
                    darkSprite.SetActive(false);

                    if (playersList.Count == 1)
                    {
                    }
                    else
                    {

                    }
                }


            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (collision.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    OnUI();
                    darkSprite.SetActive(true);
                    playerMine = collision.gameObject;
                    playerCheck = true;
                    enterPosition = collision.transform.position;
                }
                playersList.Add(collision.gameObject);
                VoiceManager.GetInstance().SettingPlayerList(playersList);
                activeRoom = true;

            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i] == collision.gameObject)
                    {
                        playersList.RemoveAt(i);
                        VoiceManager.GetInstance().SettingPlayerList(playersList);
                        if (collision.gameObject.GetComponent<PhotonView>().IsMine)
                        {
                            darkSprite.SetActive(false);
                            playerMine = null;
                            // ����
                            SceneUIManager.GetInstance().OffVoicePanel();
                            VoiceManager.GetInstance().HeadSetOnOff(false);
                            VoiceManager.GetInstance().MicrophoneOnOff(false);
                        }
                        break;
                    }
                }
                if (playersList.Count == 0)
                {
                    activeRoom = false;
                }

            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (collision.gameObject.GetPhotonView().IsMine)
                {
                    if (playerCheck)
                    {
                        collision.transform.position = enterPosition;
                    }
                }
            }

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
                //���� �޴� ������� ������ ĳ���� ����� �Ѵ�.
            }
        }

        IEnumerator PlayerSuch()
        {
            yield return new WaitForSeconds(2.0f);
            Collider2D[] players = Physics2D.OverlapBoxAll(transform.position, new Vector2(6, 6), 0, 1 << LayerMask.NameToLayer("Player"));
            foreach (Collider2D player in players)
            {
                playersList.Add(player.gameObject);
            }

            if (playersList.Count > 0)
            {
                activeRoom = true;
            }
        }

    }

}