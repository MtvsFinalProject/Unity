using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static HttpManager;

namespace GH
{


    public class LoginJoinUIManager : MonoBehaviour
    {
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

        public UserInfoData getUserInfo;

        #region Button
        [Header("���� ��ư ����Ʈ")]
        public List<Button> nextButtons;

        [Header("���� ��ư ����Ʈ")]
        public List<Button> backButtons;

        [Header("4. �ߺ� ��ư")]
        public Button checkIDButton;

        [Header("4. ���� Ȯ�� ��ư")]
        public Button checkNumButton;

        [Header("7. ���� �Ϸ� ��ư")]
        public Button joinButton;

        [Header("7. ���� ��ư")]
        public Button menButton;

        [Header("7. ���� ��ư")]
        public Button womenButton;

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

        [Header("6. ���ɻ� ��ư ������")]
        public GameObject interestButtonPrefab;

        [Header("6. ���ɻ� ��ư ������ġ")]
        public Transform interestButtonTransform;

        [Header("6. ���ɻ� ���� �ؽ�Ʈ")]
        public TMP_Text interestText;

        [Header("6. ���õ� ���ɻ� ����Ʈ")]
        public List<string> selectedInterest;

        [Header("6. ���ɻ� ��ųʸ�")]
        private Dictionary<string, GameObject> buttonList = new Dictionary<string, GameObject>();

        [Header("7. ���༺��")]
        public bool gender;

        [Header("��� ��ǲ�ʵ� ����Ʈ")]
        // 0 - �̸�, 1 - �̸���, 2 - PassWord, 3 - �������
        public List<TMP_InputField> joinInfoInfoList;

        [Header("�α��� ��ǲ�ʵ� ����Ʈ")]
        // 0 - �̸���. 1 - ��й�ȣ
        public List<TMP_InputField> loginList;

        [Header("�α��� ��ư")]
        public Button loginButton;

        [Header("��й�ȣ Ʋ�� �ؽ�Ʈ")]
        public GameObject PWCheckText;


        private Color32 selectColor = new Color32(242, 136, 75, 255);
        private Color32 noneSelectColor = new Color32(242, 242, 242, 255);


        public TMP_InputField schoolInputField;

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
            menButton.onClick.AddListener(ClickMen);
            womenButton.onClick.AddListener(ClickWomen);


            //�α��� ��Ʈ
            loginButton.onClick.AddListener(UserLogin);

            PWCheckText.SetActive(false);

        }

        private void Update()
        {
            switch (currentLoginstep)
            {
                case Loginstep.NAME:
                    break;

                case Loginstep.EMAIL:
                    break;

                case Loginstep.PASSWORD:
                    PWCheck();
                    break;

                case Loginstep.INTEREST:
                    break;
            }
            Slider();
        }

        public void NextStep()
        {
            switch (currentLoginstep)
            {
                case Loginstep.NAME:
                    currentJoinInfo.name = joinInfoInfoList[0].text;
                    break;

                case Loginstep.EMAIL:
                    currentJoinInfo.email = joinInfoInfoList[1].text;
                    break;

                case Loginstep.PASSWORD:
                    currentJoinInfo.password = joinInfoInfoList[2].text;
                    break;

                case Loginstep.INTEREST:
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
            if (pWCheckInputField.text.Length > 0 && pWInputField.text == pWCheckInputField.text)
            {
                nextButtons[5].GetComponent<Image>().color = selectColor;
                nextButtons[5].interactable = true;
            }
            else
            {
                nextButtons[5].GetComponent<Image>().color = noneSelectColor;
                nextButtons[5].interactable = false;
            }
        }

        private void InterestButtonCreate()
        {
            for (int i = 0; i < DataManager.instance.interests.Count; i++)
            {
                GameObject interestButton = Instantiate(interestButtonPrefab, interestButtonTransform);
                interestButton.GetComponentInChildren<TMP_Text>().text = DataManager.instance.interests[i];
                buttonList.Add(DataManager.instance.interests[i], interestButton);
            }
        }

        public void InterestSlect(string key, Image image)
        {
            bool test = false;
            interestText.text = "";

            if (selectedInterest.Count > 0)
            {

                // �ߺ� üũ
                for (int i = 0; i < selectedInterest.Count; i++)
                {
                    if (selectedInterest[i] == key)
                    {
                        test = true;

                        image.color = noneSelectColor;
                        selectedInterest.RemoveAt(i);
                        break;
                    }
                    else
                    {
                        test = false;

                    }
                    // interestText.text += "#" + selectedInterest[i] + " ";
                }
                if (!test)
                {
                    image.color = selectColor;

                    if (selectedInterest.Count < 5)
                    {
                        selectedInterest.Add(key);
                    }
                    else
                    {
                        buttonList[selectedInterest[0]].GetComponent<Image>().color = noneSelectColor;
                        selectedInterest.RemoveAt(0);
                        selectedInterest.Add(key);
                    }
                }

                for (int i = 0; i < selectedInterest.Count; i++)
                {
                    interestText.text += "#" + selectedInterest[i] + " ";

                }
            }
            else
            {
                image.color = new Color32(242, 136, 75, 255);
                selectedInterest.Add(key);
                interestText.text += "#" + key + " ";
            }

        }


        //��� �ڵ�
        private void UserJoin()
        {
            currentJoinInfo.birthday = joinInfoInfoList[3].text;

            UserInfo joinInfo = new UserInfo();
            joinInfo.email = currentJoinInfo.email;
            joinInfo.name = currentJoinInfo.name;
            joinInfo.birthday = currentJoinInfo.birthday;
            joinInfo.gender = gender;
            joinInfo.password = currentJoinInfo.password;
            joinInfo.interest = selectedInterest;


            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user";
            info.body = JsonUtility.ToJson(joinInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));

            logins[7].SetActive(false);
            logins[2].SetActive(true);

            currentLoginstep = Loginstep.LOGIN;
        }

        private void UserLogin()
        {
            HttpInfo info = new HttpInfo();
            info.url =  HttpManager.GetInstance().SERVER_ADRESS + "/user/email/" + loginList[0].text;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                print(jsonData);
                //jsonData�� PostInfoArray ������ �ٲ���.
                getUserInfo = JsonUtility.FromJson<UserInfoData>(jsonData);
                print("get : "+getUserInfo);
                LoginCallback();
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));


        }
        private void LoginCallback()
        {
            if (getUserInfo.data.password == loginList[1].text)
            {
                print("����");
                AuthManager.GetInstance().userAuthData = new AuthManager.AuthData(getUserInfo.data);
                //�� �Ѿ��
                DataManager.instance.playerName = getUserInfo.data.name;
                SceneManager.LoadScene(2);
            }
            else
            {
                print("Ʋ��");

                PWCheckText.SetActive(true);
                print("��й�ȣ�� �ٸ��ϴ�");
            }
        }

        private void ClickMen()
        {
            menButton.GetComponent<Image>().color = selectColor;
            womenButton.GetComponent<Image>().color = noneSelectColor;
            gender = true;
        }
        private void ClickWomen()
        {
            menButton.GetComponent<Image>().color = noneSelectColor;
            womenButton.GetComponent<Image>().color = selectColor;
            gender = false;
        }

    }
}