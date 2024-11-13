using MJ;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace GH
{

    public class PrivateRoom : MonoBehaviourPun, IPunObservable
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

        private bool activeRoom = false;

        public GameObject playerMine;

        private GameObject darkSprite;

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

            StartCoroutine(PlayerSuch());

        }

        void Update()
        {

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

        public void PassWordCheck()
        {
            if (!activeRoom)
            {
                if (playersList.Count == 1)
                {

                    roomPassword = passWordInputField.text;
                    if (playerMine.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        passWordPanel.SetActive(false);
                        boxCollider.isTrigger = true;

                        playerMine.transform.position = gameObject.transform.position - new Vector3(0, 1.5f, 0);
                        activeRoom = true;
                    }
                }


            }
            else
            {
                if (passWordInputField.text == roomPassword)
                {
                    boxCollider.isTrigger = true;
                    passWordPanel.SetActive(false);
                }
                else
                {
                    passWordWrongText.gameObject.SetActive(true);
                }

            }

        }

        public void PassWordExit()
        {
            passWordPanel.SetActive(false);

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (collision.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    OnUI();
                    darkSprite.SetActive(true);
                    playerMine = collision.gameObject;

                }
                playersList.Add(collision.gameObject);

            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //  playersList.Add(collision.gameObject);
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
                        if (collision.gameObject.GetComponent<PhotonView>().IsMine)
                        {
                            boxCollider.isTrigger = false;
                            darkSprite.SetActive(false);
                            playerMine = null;
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