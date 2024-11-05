using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;

namespace GH
{


    public class LoginJoinUIManager : MonoBehaviour
    {
        public string iP;
        public string port;
        public enum Loginstep
        {
            START,
            SERVICE,
            LOGIN,
            NAME,
            EMAIL,
            PASSWORD,
            INTEREST,
            PERSONAL
        }

        public Loginstep currentLoginstep;

        public UserInfo currentJoinInfo;

        #region Button
        [Header("���� ��ư ����Ʈ")]
        public List<Button> nextButtons;

        [Header("���� ��ư ����Ʈ")]
        public List<Button> backButtons;

        [Header("4. �ߺ� ��ư")]
        public Button checkIDButton;

        [Header("4. ���� Ȯ�� ��ư")]
        public Button checkNumButton;

        [Header("7. ���� Ȯ�� ��ư")]
        public Button joinButton;

        #endregion

        #region Panel
        [Header("�α��� �г� ����Ʈ")]
        public List<GameObject> logins;

        #endregion

        [Header("�ε��� �����̴�")]
        public Slider indexSlider;

        [Header("4. �ߺ� ���̵� �ؽ�Ʈ")]
        public TMP_Text checkIDText;

        [Header("4. �ߺ� Ȯ�� �ؽ�Ʈ")]
        public TMP_Text checkNumText;

        [Header("5. ��й�ȣ ��ǲ�ʵ�")]
        public TMP_InputField pWInputField;

        [Header("5. ��й�ȣ Ȯ�� ��ǲ�ʵ�")]
        public TMP_InputField pWCheckInputField;

        [Header("6. ���ɻ� ����Ʈ")]
        public List<string> interests;

        [Header("6. ���ɻ� ��ư ������")]
        public GameObject interestButtonPrefab;

        [Header("6. ���ɻ� ��ư ������ġ")]
        public Transform interestButtonTransform;

        [Header("6. ���ɻ� ���� �迭")]
        public Queue<String> interestsQueue = new Queue<String>(5);

        [Header("��� ��ǲ�ʵ� ����Ʈ")]
        // 0 - �̸�, 1 - �̸���, 2 - PassWord, 3 - �������
        public List<TMP_InputField> joinInfoInfoList;


        private void Start()
        {

            // �ʱ� �г� ��Ƽ�� ����
            for (int i = 0; i < logins.Count; i++)
            {
                logins[i].SetActive(false);
            }
            logins[0].SetActive(true);

            // �������� �Ѿ�� ��ư�� �Ҵ�
            for (int i = 0; i < nextButtons.Count; i++)
            {
                nextButtons[i].onClick.AddListener(NextStep);
            }

            //  �ڷΰ��� ��ư�� �Ҵ�
            for (int i = 0; i < backButtons.Count; i++)
            {
                backButtons[i].onClick.AddListener(BackStep);
            }

            //4 ������
            checkNumButton.onClick.AddListener(CheckNum);
            checkNumText.gameObject.SetActive(false);
            checkIDButton.onClick.AddListener(CheckID);
            checkIDText.gameObject.SetActive(false);

            //6 ������
            InterestButtonCreate();

            //7������
            joinButton.onClick.AddListener(UserJoin);
        }

        private void Update()
        {
            Slider();
            PWCheck();
        }

        public void NextStep()
        {
            switch (currentLoginstep)
            {
                case Loginstep.NAME:
                    currentJoinInfo.name = joinInfoInfoList[0].text;
                    break;

                case Loginstep.EMAIL:
                    break;

                case Loginstep.PASSWORD:
                    break;

                case Loginstep.INTEREST:
                    break;
                    
                case Loginstep.PERSONAL:
                    break;

            }

            for (int i = 0; i < logins.Count; i++)
            {

                logins[i].SetActive(false);

            }
            logins[(int)currentLoginstep + 1].SetActive(true);
            currentLoginstep++;
        }
        public void BackStep()
        {
            for (int i = 0; i < logins.Count; i++)
            {

                logins[i].SetActive(false);

            }
            logins[(int)currentLoginstep - 1].SetActive(true);
            currentLoginstep--;
        }

        private void Slider()
        {
            int indexConvert = (int)currentLoginstep - 2;
            float sliderValue = indexConvert * 0.2f;
            indexSlider.value = Mathf.Lerp(indexSlider.value, sliderValue, 0.05f);
        }

        public void CheckNum()
        {
            //���***** ������ȣ Ȯ��
            checkNumText.gameObject.SetActive(true);
        }
        public void CheckID()
        {
            //���***** ���̵� �ߺ� Ȯ��
            checkIDText.gameObject.SetActive(true);
        }

        public void PWCheck()
        {
            if (pWInputField.text == pWCheckInputField.text)
            {
                nextButtons[5].GetComponent<Image>().color = new Color32(242, 136, 75, 255);
                nextButtons[5].interactable = true;
            }
            else
            {
                nextButtons[5].GetComponent<Image>().color = new Color32(242, 242, 242, 255);
                nextButtons[5].interactable = false;
            }
        }

        private void InterestButtonCreate()
        {
            for (int i = 0; i < interests.Count; i++)
            {
                Button interestButton = Instantiate(interestButtonPrefab, interestButtonTransform).GetComponent<Button>();
                interestButton.GetComponentInChildren<TMP_Text>().text = interests[i];
            }
        }



        //��� �ڵ�
        private void UserJoin()
        {
            UserInfo joinInfo = new UserInfo();
            joinInfo.email = "aaa@naver.com";
            joinInfo.name = "�̱���";
            joinInfo.birthday = "20241231";
            joinInfo.gender = true;
            joinInfo.password = "asd123";
            joinInfo.interest = new List<string>() {"����", "��ȭ", "����" };


            HttpInfo info = new HttpInfo();
            info.url = "http://" + iP +":"+port+"/user";
            info.body = JsonUtility.ToJson(joinInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }

    }
}