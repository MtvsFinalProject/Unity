using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class PlayerEmoji : MonoBehaviourPun, IPunObservable
    {
        public List<GameObject> emojiPrefabList = new List<GameObject>();
        public GameObject stingPrefab;

        public Transform emojiTransform;
        public GameObject emojiButtonPrefab;


        //��� ���� ��
        private Vector3 stingDir;

        PlayerMove playerMove;


        void Start()
        {
            playerMove = GetComponent<PlayerMove>();

            emojiTransform = GameManager.instance.emojiTransform;

            if(photonView.IsMine && GameManager.instance.emojiList.Count != 0)
            {
                for(int i = 0; i < GameManager.instance.emojiList.Count; i++)
                {
                    Destroy(GameManager.instance.emojiList[i].gameObject);
                }
                GameManager.instance.emojiList.Clear();
            }
            if (GetComponent<PhotonView>().IsMine)
            {
                //�̸��� ��ư ����
                for (int i = 0; i < emojiPrefabList.Count; i++)
                {
                    GameObject emoji = Instantiate(emojiButtonPrefab, emojiTransform);
                    EmojiButton emojiBut = emoji.GetComponent<EmojiButton>();
                    emojiBut.EmojiIndex(i);
                    emojiBut.playerEmoji = gameObject.GetComponent<PlayerEmoji>();
                    Image emojiImage = emoji.transform.GetChild(0).gameObject.GetComponent<Image>();
                    emojiImage.sprite = emojiPrefabList[i].GetComponent<SpriteRenderer>().sprite;
                    GameManager.instance.emojiList.Add(emoji);
                }
            }
            if (photonView.IsMine)
            {
                GameManager.instance.stingButton.onClick.AddListener(RPC_OnSting);

            }

        }
        void Update()
        {
            #region ��ǻ�� ���� ǥ��
            /*
            if (GetComponent<PhotonView>().IsMine)
            {
                // ���� �̸��� ����
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    OnEmoji(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    OnEmoji(1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    OnEmoji(2);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    OnEmoji(3);
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    OnEmoji(4);
                }



                //��� �̸��� ����
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    OnString();
                }
            }
            */
            #endregion
        }
        [PunRPC]
        public void OnEmoji(int num)
        {

            GameObject emoji = Instantiate(emojiPrefabList[num]);
            emoji.transform.position = transform.position + (transform.up * 1.5f);

        }
        public void RPC_OnEmoji(int num)
        {
            photonView.RPC(nameof(OnEmoji), RpcTarget.All, num);
        }

        [PunRPC]
        public void OnString()
        {
            if (photonView.IsMine)
            {
                stingDir = GetComponent<PlayerMove>().stingDir;

            }
            else
            {
                stingDir = GetComponent<PlayerMove>().stingDirPun;

            }
            GameObject sting = Instantiate(stingPrefab);
            sting.transform.position = transform.position;
            sting.transform.right = stingDir;

        }

        public void RPC_OnSting()
        {

            photonView.RPC(nameof(OnString), RpcTarget.All);

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
    }

}