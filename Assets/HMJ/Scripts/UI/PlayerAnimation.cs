using GH;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using static HttpManager;
using static MapRegisterDataUI;
using static MJ.DecorationEnum;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace MJ
{
    [Serializable]
    public struct AvatarIndexData
    {
        public int userId;
        public List<int> infoList;
    }

    public class PlayerAnimation : MonoBehaviour
    {
        //public static PlayerAnimation instance;

        AvatarIndexData avatarIndexData;

        //private void Awake()
        //{
        //    if (instance == null)
        //    {
        //        instance = this;
        //        DontDestroyOnLoad(gameObject);
        //    }
        //    else
        //    {
        //        Destroy(gameObject);
        //    }

        //}
        private void Start()
        {
        }

        private int[] animatorIndex = new int[(int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END];
        private int[] animMaxIndexData = {3, 5, 4, 4 };
        public Animator[] playerAnimator = new Animator[(int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END];

        public void SetDecorationAnimData(DecorationEnum.DECORATION_DATA decorationData, int idx)
        {
            if (animMaxIndexData[(int)decorationData] <= idx)
                return;
            playerAnimator[(int)decorationData].SetBool("Anim" + (idx + 1).ToString(), true);
            animatorIndex[(int)decorationData] = idx;

            ResetDecorationAnim();
        }

        public void ResetDecorationAnimData(DecorationEnum.DECORATION_DATA decorationData)
        {
            int maxAnimData = animMaxIndexData[(int)decorationData];
            for (int i = 0; i < maxAnimData; i++)
            {
                playerAnimator[(int)decorationData].SetBool("Anim" + (i + 1).ToString(), false);
            }
        }

        public void ResetDecorationAnim()
        {
            for (int i = 0; i < (int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END; i++)
            {
                if(animatorIndex[i] >= 0)
                    playerAnimator[i].Play("Anim" + (animatorIndex[i] + 1).ToString(), 0, 0.0f);
            }
        }

        public void InitPlayerAnimation()
        {
            for (int i = 0; i < (int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END; i++)
            {
                animatorIndex[i] = UnityEngine.Random.Range(0, animMaxIndexData[i]);
                ResetDecorationAnimData((DecorationEnum.DECORATION_DATA)i);
                SetDecorationAnimData((DecorationEnum.DECORATION_DATA)i, animatorIndex[i]);
            }
        }


        private void OnEnable()
        {
        }
        // �ƹ�Ÿ ������ ����
        public void SendAvatarData()
        {
            AvatarIndexData avatarInfoList = new AvatarIndexData();
            avatarInfoList.userId = DataManager.instance.mapId;
            avatarInfoList.infoList = animatorIndex.ToList();

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/avatar";
            info.body = JsonUtility.ToJson(avatarInfoList);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                Debug.Log("�ƹ�Ÿ ������-------------------");
                Debug.Log(downloadHandler.text);
                Debug.Log("--------------------------------");
            };

            StartCoroutine(HttpManager.GetInstance().Post(info));
        }

        public void FetchAvatarData()
        {
            AvatarIndexData avatarInfoList = new AvatarIndexData();
            avatarInfoList.userId = DataManager.instance.mapId;
            avatarInfoList.infoList = animatorIndex.ToList();

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/avatar";
            info.body = JsonUtility.ToJson(avatarInfoList);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Patch(info));
        }

        // �ƹ�Ÿ ������ �ޱ�
        public void GetAvatarData()
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/avatar?userId=" + DataManager.instance.mapId;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                avatarIndexData = JsonUtility.FromJson<AvatarIndexData>(downloadHandler.text);
                Debug.Log("--------------------------------------------------------------------------------");
                Debug.Log("�ƹ�Ÿ ���� ����Ʈ: " + avatarIndexData);
                Debug.Log("--------------------------------------------------------------------------------");
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }
    }

}
