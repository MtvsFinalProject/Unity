using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class PlayerEmoji : MonoBehaviour
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
                }
            }
            GameManager.instance.stingButton.onClick.AddListener(OnString);

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
        public void OnEmoji(int num)
        {
            GameObject emoji = Instantiate(emojiPrefabList[num]);
            emoji.transform.position = transform.position + transform.up;
        }

        public void OnString( )
        {
            stingDir = playerMove.stingDir;

            GameObject sting = Instantiate(stingPrefab);
            sting.transform.position = transform.position;
            sting.transform.right = stingDir;
        }

    
    }

}