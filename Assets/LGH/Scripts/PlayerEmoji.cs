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

        void Start()
        {
            //�̸��� ��ư ����
            for(int i = 0; i < emojiPrefabList.Count; i++)
            {
                GameObject emoji = Instantiate(emojiButtonPrefab, emojiTransform);
                EmojiButton emojiBut = emoji.GetComponent<EmojiButton>();
                emojiBut.EmojiIndex(i);
                Image emojiImage =  emoji.transform.GetChild(0).gameObject.GetComponent<Image>();
                emojiImage.sprite = emojiPrefabList[i].GetComponent<SpriteRenderer>().sprite;
            }
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

            // ��� �̸��� ���Ⱚ ����
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                stingDir = transform.right;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                stingDir = -transform.right;

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                stingDir = transform.up;

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                stingDir = -transform.up;

            }

            //��� �̸��� ����
            if (Input.GetKeyDown(KeyCode.Z))
            {
                OnString(stingDir);
            }
        }
        public void OnEmoji(int num)
        {
            GameObject emoji = Instantiate(emojiPrefabList[num]);
            emoji.transform.position = transform.position + transform.up;
        }

        private void OnString(Vector3 dir)
        {
            GameObject sting = Instantiate(stingPrefab);
            sting.transform.position = transform.position;
            sting.transform.right = dir;
        }

    }

}