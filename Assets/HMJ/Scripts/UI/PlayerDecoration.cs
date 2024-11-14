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

        private void Awake()
        {

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
            string ImagePath = "Assets/Resources/Image/PlayerDecoration/";
            string[] DecorationData = { "Skin", "Face", "Hair", "Cloth" };
            // ������
            for(int i = 0; i < DecorationData.Length; i++)
            {
                FileInfo[] fileInfos = FileManager.Instance.GetFileInfo(ImagePath + DecorationData[i], "png");
                for (int j = 0; j < fileInfos.Length; j++) // Ư�� ������ �̹��� ������ ��������
                {
                    string data = "Image/PlayerDecoration/" + DecorationData[i] + "/" + Path.GetFileNameWithoutExtension(fileInfos[j].Name);
                    loadDecorationImage[i, j] = Resources.LoadAll<Sprite>(data)[0].texture;
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
