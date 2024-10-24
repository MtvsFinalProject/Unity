using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace MJ
{
    public class UIManager : MonoBehaviour
    {
        #region Button
        [Header("�α��� ��ư")]
        public UnityEngine.UI.Button loginButton;
        public UnityEngine.UI.Button KakaoLoginButton;

        [Header("ȸ������ ��ư")]
        public UnityEngine.UI.Button joinButton;
        public UnityEngine.UI.Button kakaoJoinButton;

        [Header("�ݱ� ��ư")]
        public UnityEngine.UI.Button MapRegisterCloseButton;
        public UnityEngine.UI.Button MapContestCloseButton;

        public UnityEngine.UI.Button kakaoLoginCloseButton;
        public UnityEngine.UI.Button kakaoJoinCloseButton;

        [Header("�ٹ̱� �г� - Skin, Face, Hair, Cloth")]
        public UnityEngine.UI.Button[] decorationEnumButton;


        [Header("�ٹ̱� �г� - ����â(0, 1, 2, 3)")]
        public UnityEngine.UI.Button[] decorationChoiceButton;

        [Header("�� ��� �г� - �� ��� �г� ��ư")]
        public UnityEngine.UI.Button mapRegisterButton;
        #endregion

        #region Panel
        [Header("�α��� �г�")]
        public GameObject loginPanel;
        public GameObject KakaoLoginPanel;

        [Header("ȸ������ �г�")]
        public GameObject JoinPanel;
        public GameObject KakaoJoinPanel;

        [Header("�ٹ̱� �г�")]
        public GameObject DecorationPanel;

        [Header("�÷��̾� �ٹ̱� ������Ʈ")]
        public GameObject PlayerObject;

        [Header("���� �г�")]
        public GameObject guestbookPanel;

        [Header("�� ��� �г�")]
        public GameObject mapRegisterPanel;

        [Header("�� ���׽�Ʈ �г�")]
        public GameObject mapContestPanel;
        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            PlayerDecoration DecorationDT = DecorationPanel.GetComponent<PlayerDecoration>();
            PlayerAnimation AnimationDT = PlayerObject.GetComponent<PlayerAnimation>();
            
            loginButton.onClick.AddListener(OnLoginPanel);
            joinButton.onClick.AddListener(OnJoinPanel);

            KakaoLoginButton.onClick.AddListener(OnKakaoLoginPanel);
            kakaoJoinButton.onClick.AddListener(OnKakaoJoinPanel);

            kakaoLoginCloseButton.onClick.AddListener(OnKakaoLoginClosePanel);
            kakaoJoinCloseButton.onClick.AddListener(OnKakaoJoinClosePanel);

            mapRegisterButton.onClick.AddListener(OnMapRegisterPanel);

            MapRegisterCloseButton.onClick.AddListener(CloseMapRegisterPanel);
            MapRegisterCloseButton.onClick.AddListener(OnMapContestPanel);

            MapContestCloseButton.onClick.AddListener(CloseMapContestPanel);

            for (int i = 0; i < decorationEnumButton.Length; i++)
            {
                int data = i;
                decorationEnumButton[i].onClick.AddListener(() => DecorationDT.SetPlayerDecorationData((DecorationEnum.DECORATION_DATA)data));
            }

            for (int i = 0; i < decorationChoiceButton.Length; i++)
            {
                int data = i;
                decorationChoiceButton[i].onClick.AddListener(() =>
                {
                    DecorationDT.SetPlayerSelectDecorationData(DecorationDT.CurDecorationPanel, data);
                    
                    AnimationDT.ResetDecorationAnimData(DecorationDT.CurDecorationPanel); 
                    AnimationDT.SetDecorationAnimData(DecorationDT.CurDecorationPanel, data);
                });
            }
        }

        public void OnLoginPanel()
        {
            loginPanel.SetActive(true);
            JoinPanel.SetActive(false);
        }

        public void OnJoinPanel()
        {
            loginPanel.SetActive(false);
            JoinPanel.SetActive(true);
        }

        public void OnKakaoLoginPanel()
        {
            KakaoLoginPanel.SetActive(true);
            KakaoJoinPanel.SetActive(false);
        }

        public void OnKakaoJoinPanel()
        {
            KakaoLoginPanel.SetActive(false);
            KakaoJoinPanel.SetActive(true);
        }

        public void OnKakaoJoinClosePanel()
        {
            KakaoJoinPanel.SetActive(false);
        }

        public void OnKakaoLoginClosePanel()
        {
            KakaoLoginPanel.SetActive(false);
        }
        
        public void OnGuestbookPanel()
        {
            guestbookPanel.SetActive(true);
        }

        public void OnMapRegisterPanel()
        {
            mapRegisterPanel.SetActive(true);
            mapContestPanel.SetActive(false);
        }

        public void OnMapContestPanel()
        {
            mapRegisterPanel.SetActive(false);
            mapContestPanel.SetActive(true);
        }

        public void CloseMapRegisterPanel()
        {
            mapRegisterPanel.SetActive(false);
        }

        public void CloseMapContestPanel()
        {
            mapContestPanel.SetActive(false);
        }
    }
}