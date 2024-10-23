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
        public GameObject activatePannel;

        //��� ���� ��
        private Vector3 stingDir;

        PlayerMove playerMove;
        // �г� �� ����
        bool onActivate = true;

        void Start()
        {
            playerMove = GetComponent<PlayerMove>();


            //�̸��� ��ư ����
            for (int i = 0; i < emojiPrefabList.Count; i++)
            {
                GameObject emoji = Instantiate(emojiButtonPrefab, emojiTransform);
                EmojiButton emojiBut = emoji.GetComponent<EmojiButton>();
                emojiBut.EmojiIndex(i);
                Image emojiImage =  emoji.transform.GetChild(0).gameObject.GetComponent<Image>();
                emojiImage.sprite = emojiPrefabList[i].GetComponent<SpriteRenderer>().sprite;
            }

            activatePannel.SetActive(false);
            emojiTransform.gameObject.SetActive(true);
        }
        void Update()
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

        public void ConversionPanel()
        {
            print("aa");
            if (onActivate)
            {
                activatePannel.SetActive(true);
                emojiTransform.gameObject.SetActive(false);
                onActivate = false;
            }
            else
            {
                activatePannel.SetActive(false);
                emojiTransform.gameObject.SetActive(true);
                onActivate = true;

            }
        }
    }

}