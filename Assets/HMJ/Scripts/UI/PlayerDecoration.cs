using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MJ.DecorationEnum;

namespace MJ
{
    public class PlayerDecoration : MonoBehaviour
    {
        /// <summary>
        /// Player �ƹ�Ÿ �ٹ� ����
        /// </summary>
        private int[] decorationData = new int[5];

        /// <summary>
        /// ���Ͽ��� ���� �ε��� �̹��� ������
        /// </summary>
        private Texture2D[,] loadDecorationImage = new Texture2D[4, 6];

        /// <summary>
        /// �ٹ̱� �����Ϳ� ������ �̹���
        /// </summary>
        public RawImage[] decorationImage = new RawImage[3];

        private DecorationEnum.DECORATION_DATA curDecorationPanel;

        public DecorationEnum.DECORATION_DATA CurDecorationPanel
        {
            get { return curDecorationPanel; }
            set { curDecorationPanel = value; }
        }

        public static PlayerDecoration instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static PlayerDecoration GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "PlayerDecoration";
                go.AddComponent<PlayerDecoration>();
            }
            return instance;
        }

        private void Start()
        {

        }

        /// <summary>
        /// Player �ƹ�Ÿ �ٹ� ���� Set
        /// </summary>
        /// <param name="_DATA"></param>
        /// <param name="idx"></param>
        public void SetPlayerDecorationData(DECORATION_DATA _DATA)
        {
            if (_DATA == DECORATION_DATA.DECORATION_DATA_END)
                return;

            curDecorationPanel = _DATA;
            for (int i = 0; i < 5; i++)
            {
                if (loadDecorationImage[(int)_DATA, i])
                    decorationImage[i].texture = loadDecorationImage[(int)_DATA, i];
                else
                    decorationImage[i].texture = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_DATA"></param>
        /// <param name="idx"></param>
        public bool SetPlayerSelectDecorationData(DECORATION_DATA _DATA, int idx)
        {
            if (!loadDecorationImage[(int)_DATA, idx])
                return false;

            decorationData[(int)_DATA] = idx;
            return true;
        }

        /// <summary>
        /// /Image/PlayerDecoration �ִ� ������ Hair, Skin, Face, Cloth�� ���� png ������ ��� ������.
        /// </summary>
        public void LoadDecorationData()
        {

            string ImagePath = "Image/PlayerDecoration/"; // Resources ���� �� ���
            string[] DecorationData = { "Skin", "Face", "Hair", "Cloth" };
            loadDecorationImage = new Texture2D[DecorationData.Length, 10]; // ����: �� �׸񸶴� �ִ� 10���� �̹���

            for (int i = 0; i < DecorationData.Length; i++)
            {
                // Ư�� ��ο��� ��������Ʈ ���ϵ��� ��� �ε�
                Sprite[] sprites = FileManager.Instance.LoadSpritesFromResources(ImagePath + DecorationData[i]);

                for (int j = 0; j < sprites.Length; j++) // Ư�� ������ �̹��� ������ ��������
                {
                    loadDecorationImage[i, j] = sprites[j].texture;
                    Debug.Log("Loaded Sprite: " + ImagePath + DecorationData[i] + "/" + sprites[j].name);
                }
            }
        }

        private void OnEnable()
        {
            curDecorationPanel = DECORATION_DATA.SKIN;
            SetPlayerDecorationData(curDecorationPanel);
        }
    }

}
