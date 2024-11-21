using GH;
using Photon.Pun;
using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;

namespace MJ
{
    public class SceneUIManager : MonoBehaviour
    {
        #region Button
        [Header("�ݱ� ��ư")]
        public UnityEngine.UI.Button MapRegisterCloseButton;
        public UnityEngine.UI.Button MapContestCloseButton;

        [Header("�ƹ�Ÿ �ٹ̱� On ��ư")]
        public Button avatarDecoOpenButton;

        [Header("�ƹ�Ÿ �ٹ̱� Off ��ư")]
        public Button avatarDecoOffButton;

        [Header("�ٹ̱� �г� - Skin, Face, Hair, Cloth")]
        public UnityEngine.UI.Button[] decorationEnumButton;

        [Header("�ٹ̱� �г� - ����â(0, 1, 2, 3)")]
        public UnityEngine.UI.Button[] decorationChoiceButton;

        [Header("�� ��� �г� - �� ��� �г� ��ư")]
        public UnityEngine.UI.Button mapRegisterButton;

        [Header("�� ��� �г� - �� ���׽�Ʈ �г� ��ư")]
        public UnityEngine.UI.Button mapContestButton;

        [Header("�� ��� �г� - �� ��� ���� �г� ��ư")]
        public UnityEngine.UI.Button MapConfirmYesButton;
        public UnityEngine.UI.Button MapConfirmNoButton;

        [Header("�� ��� ���� �г� ����")]
        public UnityEngine.UI.Button MapRegisterSuccessCloseButton;

        [Header("�� �κ��丮 �г� - �� �κ��丮 �г� ��ư")]
        public UnityEngine.UI.Button InventoryButton;

        [Header("�� �κ��丮 �г� - �� �κ��丮 �г� ���� ��ư")]
        public UnityEngine.UI.Button InventoryCloseButton;

        [Header("�� �κ��丮 ���� �г� - �� �κ��丮 ���� �г� ���� ��ư")]
        public UnityEngine.UI.Button InventoryErrorCloseButton;

        [Header("�޴� ��ư")]
        public UnityEngine.UI.Button topMenuButton;

        [Header("ģ��â ��ư")]
        public UnityEngine.UI.Button friendsButton;

        [Header("���� ������ ��ư")]
        public Button myProfileButton;

        [Header("���� ������ ���� ��ư")]
        public Button myProfileEditButton;

        [Header("���� ������ ���� ���� ��ư")]
        public Button myProfileSaveButton;

        [Header("ù �α��� �л� ���� ��ư")]
        public Button studentCheckButton;

        [Header("ù �α��� �л� �ƴ� ���� ��ư")]
        public Button noStudentCheckButton;

        [Header("�б� ���� �Ϸ� ��ư")]
        public Button schoolSaveButton;

        #endregion

        #region Panel
        [Header("�ٹ̱� �г�")]
        public GameObject DecorationPanel;

        [Header("�÷��̾� �ٹ̱� ������Ʈ")]
        public GameObject PlayerObject;

        [Header("���� �г�")]
        public GameObject guestbookPanel;

        [Header("�Խ��� �г�")]
        public GameObject boardPanel;

        [Header("�� ��� �г�")]
        public GameObject mapRegisterPanel;

        [Header("�� ��� ���� �г�")]
        public GameObject mapConfirmPanel;

        [Header("�� ���׽�Ʈ �г�")]
        public GameObject mapContestPanel;

        [Header("�� ��� ���� �г�")]
        public GameObject mapSuccessRegisterPanel;

        [Header("�� �κ��丮 �г�")]
        public GameObject mapInventoryPanel;

        [Header("�� �κ��丮 ���� �г� - ������ ���� ����")]
        public GameObject mapInventoryErrorPanel;

        [Header("�޴� �г�")]
        public GameObject menuPanel;

        [Header("ģ��â �г�")]
        public GameObject friendsPanel;

        [Header("ä�� �г�")]
        public GameObject ChatPanel;

        [Header("���� ������ �г�")]
        public GameObject myProfilePanel;

        [Header("���� ������ ���� �г�")]
        public GameObject myProfileEditPanel;

        [Header("�ٸ� �÷��̾� ������ �г�")]
        public GameObject othersProfilePanel;

        [Header("�÷��̾� ��� �г�")]
        public GameObject playerList;

        [Header("ù �α��� �л� ���� �г�")]
        public GameObject firstLoginPanel;

        [Header("ù �α��� �б� ���� �г�")]
        public GameObject firstSchoolPanel;

        [Header("�б� �湮 �г�")]
        public GameObject visitOtherSchoolPanel;
        [Header("�����̺� �� �г�")]
        public GameObject privateRoomPanel;

        [Header("���̽� �г�")]
        public GameObject voicePanel;
        #endregion

        #region SingleTone
        private static SceneUIManager instance;
        public static SceneUIManager GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "SceneUIManager";
                go.AddComponent<SceneUIManager>();
            }
            return instance;
        }

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
        #endregion

        #region ����
        [Header("������ ���� ��ǲ �ڽ���")]
        public TMP_InputField nickNameInputField;
        public TMP_InputField interestInputField;
        public TMP_InputField myMessageInputField;

        [Header("������ ���� �ؽ�Ʈ")]
        public TMP_Text nickNameText;
        public TMP_Text interestText;
        public TMP_Text myMessageText;

        [Header("���ɻ� ��ųʸ�")]
        private Dictionary<string, GameObject> buttonList = new Dictionary<string, GameObject>();

        [Header("���ɻ� ��ư ������")]
        public GameObject interestButtonPrefab;

        [Header("���ɻ� ��ư ������ġ")]
        public RectTransform interestButtonTransform;

        [Header("���õ� ���ɻ� ����Ʈ")]
        public List<string> selectedInterest;

        private Color32 selectColor = new Color32(242, 136, 75, 255);
        private Color32 noneSelectColor = new Color32(242, 242, 242, 255);

        public TMP_Text profileLvNick;
        public TMP_Text profileInterest;
        public TMP_Text profileMyMessage;

        public UserInfo currentuserInfo;

        [Header("�б� �˻� ��ǲ�ʵ�")]
        public TMP_InputField schoolSuch;

        [Header("�б� ��� �� ����Ʈ")]
        public SchoolData schooldata;

        [Header("�б� ��� ��Ӵٿ�")]
        public TMP_Dropdown schoolDropDown;

        [Header("�г� ��Ӵٿ�")]
        public TMP_Dropdown schoolGradeDropDown;

        public List<string> schoolName;
        #endregion

        private void Start()
        {
            myProfileButton.onClick.AddListener(OnOffMyProfile);
            myProfileEditButton.onClick.AddListener(OnMyProfileEdit);
            myProfileSaveButton.onClick.AddListener(OffMyProfileEdit);
            studentCheckButton.onClick.AddListener(ClickStudent);
            noStudentCheckButton.onClick.AddListener(ClickNoStudent);
            schoolSaveButton.onClick.AddListener(SchoolSave);

            firstLoginPanel.SetActive(false);
            firstSchoolPanel.SetActive(false);
            myProfileEditPanel.SetActive(false);
            myProfilePanel.SetActive(false);

            if (mapRegisterButton)
            {
                mapRegisterButton.onClick.AddListener(OnMapRegisterPanel);
            }
            if (MapRegisterCloseButton)
            {
                MapRegisterCloseButton.onClick.AddListener(CloseMapRegisterPanel);
                MapRegisterCloseButton.onClick.AddListener(OnMapContestPanel);
            }

            if (MapContestCloseButton)
                MapContestCloseButton.onClick.AddListener(CloseMapContestPanel);

            // StartCoroutine(coMapInventoryGameObject());
            InventoryButton.onClick.AddListener(OnMapInventoryPanel);
            InventoryCloseButton.onClick.AddListener(CloseMapInventoryPanel);

            if (mapContestButton)
                mapContestButton.onClick.AddListener(OnMapConfirmPanel);

            if (MapConfirmYesButton)
                MapConfirmYesButton.onClick.AddListener(OnMapRegisterPanel);
            if (MapConfirmYesButton)
                MapConfirmYesButton.onClick.AddListener(OffMapConfirmPanel);
            if (MapConfirmNoButton)
                MapConfirmNoButton.onClick.AddListener(OffMapConfirmPanel);

            if (MapRegisterSuccessCloseButton)
                MapRegisterSuccessCloseButton.onClick.AddListener(OffMapSuccessRegisterPanel);

            if (InventoryErrorCloseButton)
                InventoryErrorCloseButton.onClick.AddListener(OffMapInventoryErrorPanel);

            if (avatarDecoOpenButton)
                avatarDecoOpenButton.onClick.AddListener(OnDecorationPanel);

            if (avatarDecoOffButton)
                avatarDecoOffButton.onClick.AddListener(OffDecorationPanel);


            if (AuthManager.GetInstance().userAuthData.userInfo.school.schoolName == "")
            {
                RegisterAvatar();

                firstLoginPanel.SetActive(true);
            }
            else
                GetAvatar();

            InterestButtonCreate();

            SetProfile();
            initDecorationPanel();
            InitOtherPlayerPanel();
            initDecorationPanel();
        }
        private void Update()
        {
            //ProfileEditCount();

            if (firstSchoolPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    print("����");
                    SchoolGet();
                }
                schoolDropDown.onValueChanged.AddListener(delegate { SetSchoolName(schoolDropDown.value); });

            }
            TouchPlayer();
        }
        public void RegisterAvatar()
        {
            PlayerAnimation.GetInstance().PostAvatarData();
        }

        public void GetAvatar()
        {
            PlayerAnimation.GetInstance().GetAvatarData();
        }

        public void initDecorationPanel()
        {
            avatarDecoOpenButton.onClick.AddListener(OnDecorationPanel);
            avatarDecoOffButton.onClick.AddListener(OffDecorationPanel);

            PlayerDecoration DecorationDT = PlayerDecoration.GetInstance();
            DecorationDT.LoadDecorationData();

            PlayerAnimation AnimationDT = PlayerAnimation.GetInstance();


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
                    if (!DecorationDT.SetPlayerSelectDecorationData(DecorationDT.CurDecorationPanel, data))
                        return;
                    AnimationDT.SetDecorationAnimData(DecorationDT.CurDecorationPanel, data);
                });
            }

            // OffDecorationPanel();
        }
        public void OnGuestbookPanel()
        {
            guestbookPanel.SetActive(true);
            guestbookPanel.GetComponent<Guestbook>().LoadGuestbookData();
        }
        public void OnBoardPanel()
        {
            boardPanel.SetActive(true);
            boardPanel.GetComponent<Board>().LoadBoardData();
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
            mapContestButton.gameObject.SetActive(false);
        }

        IEnumerator coMapInventoryGameObject()
        {
            yield return new WaitUntil(() => mapInventoryPanel != null);
        }

        public void OnMapInventoryPanel()
        {
            mapInventoryPanel.SetActive(true);
            InventoryCloseButton.gameObject.SetActive(true);
            InventoryButton.gameObject.SetActive(false);
            ChatPanel.gameObject.SetActive(false);

            if (DataManager.instance.player != null)
            {
                DataManager.instance.player.GetComponent<SetTile>().setMode = true;
            }
        }

        public void CloseMapRegisterPanel()
        {
            mapRegisterPanel.SetActive(false);
        }

        public void CloseMapContestPanel()
        {
            mapContestPanel.SetActive(false);
            mapContestButton.gameObject.SetActive(true);
        }

        public void CloseMapInventoryPanel()
        {
            mapInventoryPanel.SetActive(false);
            InventoryCloseButton.gameObject.SetActive(false);
            InventoryButton.gameObject.SetActive(true);
            ChatPanel.gameObject.SetActive(true);
            if (DataManager.instance.player != null)
            {
                DataManager.instance.player.GetComponent<SetTile>().setMode = false;
            }
        }

        public void OnMapConfirmPanel()
        {
            mapConfirmPanel.SetActive(true);
        }

        public void OffMapConfirmPanel()
        {
            mapConfirmPanel.SetActive(false);
        }

        public void OnMapSuccessRegisterPanel()
        {
            // mapSuccessRegisterPanel.SetActive(true);
        }

        public void OffMapSuccessRegisterPanel()
        {
            mapSuccessRegisterPanel.SetActive(false);
        }

        public void OnMenuButtonClick()
        {
            if (menuPanel.activeSelf)
            {
                iTween.Stop(menuPanel);
                menuPanel.transform.position = new Vector3(267, menuPanel.transform.position.y, 0);
                iTween.MoveTo(menuPanel, iTween.Hash(
                    "x", 0,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutBounce,
                    "oncomplete", nameof(SetMenuPanel),
                    "oncompletetarget", gameObject,
                    "oncompleteparams", false
                ));
            }
            else
            {
                iTween.Stop(menuPanel);
                menuPanel.transform.position = new Vector3(0, menuPanel.transform.position.y, 0);
                SetMenuPanel(true);
                iTween.MoveTo(menuPanel, iTween.Hash(
                    "x", 267,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutBounce
                ));
            }
        }
        public void SetMenuPanel(bool value)
        {
            menuPanel.SetActive(value);
        }

        public void OnFriendsPanel()
        {
            friendsPanel.SetActive(true);
            friendsPanel.GetComponent<FriendsUI>().RefreshFriends();
        }

        public void OffAllMapPanel()
        {
            if (mapContestPanel)
                mapContestPanel.SetActive(false);
            if (mapRegisterPanel)
                mapRegisterPanel.SetActive(false);
            if (mapConfirmPanel)
                mapConfirmPanel.SetActive(false);
            if (mapSuccessRegisterPanel)
                mapSuccessRegisterPanel.SetActive(false);
        }

        public void OnOffMyProfile()
        {
            //��ư���� Ű�� ����
            myProfilePanel.SetActive(!myProfilePanel.activeSelf);

            Image myProfileImage = myProfileButton.GetComponentInChildren<Image>();
            Color32 myprofileColor = myProfilePanel.activeSelf ? new Color32(242, 136, 75, 255) : new Color32(29, 27, 32, 255);
            myProfileImage.color = myprofileColor;

        }

        public void OnDecorationPanel()
        {
            DecorationPanel.SetActive(true);
        }

        public void OffDecorationPanel()
        {
            PlayerAnimation.GetInstance().PatchAvatarData();
            DecorationPanel.SetActive(false);
        }
        

        public void OffMapInventoryErrorPanel()
        {
            mapInventoryErrorPanel.SetActive(false);
        }

        public void OnMapInventoryErrorPanel()
        {
            mapInventoryErrorPanel.SetActive(true);
        }

        public void OnMyProfileEdit()
        {
            myProfileEditPanel.SetActive(true);
            myProfilePanel.SetActive(false);
        }

        public void OffMyProfileEdit()
        {
            myProfileEditPanel.SetActive(false);
            myProfilePanel.SetActive(true);
            ProfileEditSave();
        }
        public void OnVisitOtherSchoolPanel()
        {
            visitOtherSchoolPanel.SetActive(true);
        }

        public void OnVoicePanel()
        {
            voicePanel.SetActive(true);
        }

        public void OffVoicePanel()
        {
            voicePanel.SetActive(false);
        }

        public void ProfileEditCount()
        {
            nickNameText.text = nickNameInputField.text.Length + "/10";
            interestText.text = selectedInterest.Count + "/5";
            myMessageText.text = myMessageInputField.text.Length + "/30";

            if (interestInputField.isFocused)
            {
                interestButtonTransform.gameObject.SetActive(true);
            }
            if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
            {
                Vector2 localPointPos = interestButtonTransform.InverseTransformPoint(Input.mousePosition);
                if (!interestButtonTransform.rect.Contains(localPointPos))
                {
                    interestButtonTransform.gameObject.SetActive(false);
                }
            }
        }
        public void InterestButtonOnOff()
        {
            bool onInterest = false;
            onInterest = interestButtonTransform.gameObject.activeSelf ? false : true;
            interestButtonTransform.gameObject.SetActive(onInterest);
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
            interestInputField.text = "";

            if (selectedInterest.Count > 0)
            {

                // �ߺ� üũ
                for (int i = 0; i < selectedInterest.Count; i++)
                {
                    if (selectedInterest[i] == key)
                    {
                        test = true;
                        print("11");

                        image.color = noneSelectColor;
                        selectedInterest.RemoveAt(i);
                        break;
                    }
                    else
                    {
                        test = false;
                        print("22");

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
                    interestInputField.text += "#" + selectedInterest[i] + " ";

                }
            }
            else
            {
                image.color = new Color32(242, 136, 75, 255);
                selectedInterest.Add(key);
                interestInputField.text += "#" + key + " ";
            }

        }

        private void ProfileEditSave()
        {
            UserInfo joinInfo = AuthManager.GetInstance().userAuthData.userInfo;
            joinInfo.name = nickNameInputField.text;
            joinInfo.interest = selectedInterest;
            joinInfo.statusMesasge = myMessageInputField.text;
            joinInfo.schoolId = 1;

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/profile";
            info.body = JsonUtility.ToJson(joinInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Patch(info));

            currentuserInfo = AuthManager.GetInstance().userAuthData.userInfo;
            currentuserInfo.name = nickNameInputField.text;
            currentuserInfo.interest = selectedInterest;
            currentuserInfo.statusMesasge = myMessageInputField.text;

            AuthManager.GetInstance().userAuthData = new AuthManager.AuthData(currentuserInfo);
            SetProfile();
        }

        private void SetProfile()
        {
            profileInterest.text = "";
            profileMyMessage.text = "";
            UserInfo userInfo = AuthManager.GetInstance().userAuthData.userInfo;
            profileLvNick.text = userInfo.level + " | " + userInfo.name;
            for (int i = 0; i < userInfo.interest.Count; i++)
            {
                profileInterest.text += "#" + userInfo.interest[i] + " ";
            }

            //�޽���
            profileMyMessage.text = userInfo.statusMesasge;
        }

        //ù �α��� �ٻ� ���� ��ư
        private void ClickStudent()
        {
            firstLoginPanel.SetActive(false);
            firstSchoolPanel.SetActive(true);
        }
        private void ClickNoStudent()
        {
            firstLoginPanel.SetActive(false);

        }
        private void SchoolSave()
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/school/add-user?schoolId="
                + schooldata.data[schoolDropDown.value].id + "&userId=" + AuthManager.GetInstance().userAuthData.userInfo.id
                + "&user_grade=" + schoolGradeDropDown.value;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));

            AuthManager.GetInstance().userAuthData.userInfo.school.schoolName = schooldata.data[schoolDropDown.value].schoolName;
            DataManager.instance.playerSchool = schooldata.data[schoolDropDown.value].schoolName;
            firstSchoolPanel.SetActive(false);

            StartCoroutine(CoUserGet());

        }

        private void SchoolGet()
        {
            schooldata.data.Clear();

            schoolName.Clear();

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/school/" + schoolSuch.text;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                print(jsonData);
                //jsonData�� PostInfoArray ������ �ٲ���.
                schooldata = JsonUtility.FromJson<SchoolData>(jsonData);
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));

            StartCoroutine(CoSchoolGet());


            print("�б� ���� �ҷ�����");



        }
        private void SetSchoolName(int option)
        {
            schoolSuch.text = schoolDropDown.options[option].text;
        }
        IEnumerator CoSchoolGet()
        {
            yield return new WaitUntil(() => schooldata.data.Count > 0);

            for (int i = 0; i < schooldata.data.Count; i++)
            {
                schoolName.Add(schooldata.data[i].schoolName);
            }

            schoolDropDown.ClearOptions();
            schoolDropDown.AddOptions(schoolName);

            schoolDropDown.Show();


        }

        IEnumerator CoUserGet()
        {
            yield return new WaitForSeconds(0.5f);



            HttpInfo info2 = new HttpInfo();
            info2.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/email/" + AuthManager.GetInstance().userAuthData.userInfo.email;
            Debug.Log("�α��� url: " + info2.url);
            info2.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                print(jsonData);
                //jsonData�� �����Ϳ� �ٽ� ���� �ٲ���.
                AuthManager.GetInstance().userAuthData = new AuthManager.AuthData(JsonUtility.FromJson<UserInfoData>(jsonData).data);
            };
            StartCoroutine(HttpManager.GetInstance().Get(info2));
        }
        private void TouchPlayer()
        {
            // ��ġ �Է��� �߻����� ���� ó��
            if (Input.touchCount > 0)
            {
                UnityEngine.Touch touch = Input.GetTouch(0);

                // ��ġ�� ���۵Ǿ��� �� ó��
                if (touch.phase == TouchPhase.Began)
                {
                    // UI�� ��ġ�� ����ë���� Ȯ��
                    if (IsPointerOverUIObject(touch.position))
                    {
                        return; // UI�� ��ġ�� ����ë���Ƿ� ���� ������Ʈ���� ��ȣ�ۿ��� ����
                    }
                    // UI�� �ƴ� 2D �ݶ��̴� ������Ʈ ��ġ ó��
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    HandleInteraction(touchPosition);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                // UI�� ��ġ�� ����ë���� Ȯ��
                if (IsPointerOverUIObject(Input.mousePosition))
                {
                    return; // UI�� ��ġ�� ����ë���Ƿ� ���� ������Ʈ���� ��ȣ�ۿ��� ����
                }
                Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                HandleInteraction(clickPosition);
            }
        }
        private void HandleInteraction(Vector2 touchPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Player"));

            if (hit.collider != null)
            {
                //Debug.Log("2D ������Ʈ�� ��ġ�߽��ϴ�: " + hit.collider.gameObject.name);
                // ��ġ�� ������Ʈ�� ���� ó�� ����
                if (hit.collider.gameObject.GetComponent<PhotonView>().IsMine == false)
                {
                    othersProfilePanel.SetActive(true);
                    UserInfo userInfo = hit.collider.gameObject.GetComponent<UserRPC>().userInfo;
                    FriendPanel comp = othersProfilePanel.GetComponent<FriendPanel>();
                    comp.id = userInfo.id;
                    comp.NickNameText.text = userInfo.name;
                    comp.InterestText.text = "#" + String.Join(" #", userInfo.interest);
                    comp.MessageText.text = userInfo.statusMesasge;
                }
            }
            else othersProfilePanel.SetActive(false);
        }
        private bool IsPointerOverUIObject(Vector2 touchPosition)
        {
            // PointerEventData ���� �� ��ġ ��ġ ����
            PointerEventData eventData = new PointerEventData(EventSystem.current) { position = touchPosition };

            // Raycast ����� ������ ����Ʈ ����
            var results = new List<RaycastResult>();

            // EventSystem�� ���� Raycast ����
            EventSystem.current.RaycastAll(eventData, results);

            // Raycast ��� �˻��Ͽ� Default ���̾ �ƴ� UI ��Ҹ� Ȯ��
            foreach (var result in results)
            {
                if (result.gameObject.layer != LayerMask.NameToLayer("Default"))
                {
                    return true; // Default ���̾ �ƴ� UI ��Ұ� ������ true ��ȯ
                }
            }

            // Default ���̾ �����Ǿ��ų�, UI�� ������ false ��ȯ
            return false;
        }
        private void InitOtherPlayerPanel()
        {
            FriendPanel comp = othersProfilePanel.GetComponent<FriendPanel>();
            comp.PassButton.onClick.AddListener(() =>
            {
                othersProfilePanel.SetActive(false);
            });
            comp.RequestButton.onClick.AddListener(() =>
            {
                WebSocketManager.GetInstance().RequestFriend(comp.id);
            });
        }
    }
}