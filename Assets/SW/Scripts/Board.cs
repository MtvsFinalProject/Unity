using GH;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using SW;
using System.Linq;

public class Board : MonoBehaviour
{
    [Header("�Խñ� ����Ʈ")]
    public GameObject content;
    public GameObject contentPrefab;
    [Header("�ۼ� �г�")]
    public GameObject createPanel;
    public Button saveBoardButton;
    public TMP_InputField titleInputField;
    public TMP_InputField contentInputField;
    [Header("���� �г�")]
    public GameObject contentPanel;
    public GameObject boardContents;
    public TMP_Text titleText;
    public TMP_Text contentText;
    public TMP_Text likeCountText;
    public Button likeButton;
    public TMP_Text comentCountText;
    public TMP_InputField comentInputField;
    public Button saveComentButton;
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    public void SetCreatePanel()
    {
        if (createPanel.activeSelf)
        {
            createPanel.SetActive(false);
            titleInputField.text = "";
            contentInputField.text = "";
            saveBoardButton.interactable = false;
        }
        else
        {
            createPanel.SetActive(true);
        }
    }
    public void CreatePanelContentsChanged()
    {
        print(titleInputField.text.Length + "/" + contentInputField.text.Length);
        if (titleInputField.text.Length == 0 || contentInputField.text.Length == 0)
            saveBoardButton.interactable = false;
        else
            saveBoardButton.interactable = true;
    }
    public void SaveBoard()
    {
        // ���� ��û ���
        BoardPostInfo boardPostInfo = new BoardPostInfo();
        boardPostInfo.title = titleInputField.text;
        boardPostInfo.content = contentInputField.text;
        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/board";
        info.body = JsonUtility.ToJson(boardPostInfo);
        info.contentType = "application/json";
        info.onComplete = (DownloadHandler res) =>
        {
            ToastMessage.OnMessage("����� ��ϵǾ����ϴ�");
            LoadBoardData();
        };
        StartCoroutine(HttpManager.GetInstance().Post(info));
        SetCreatePanel();
        titleInputField.text = "";
        contentInputField.text = "";
    }
    [Serializable]
    private struct BoardPostInfo
    {
        public string title;
        public string content;
    }

    public void SetContentPanel()
    {
        if (contentPanel.activeSelf)
        {
            contentPanel.SetActive(false);
            comentInputField.text = "";
            saveComentButton.interactable = false;
            LoadBoardData();
        }
        else
        {
            contentPanel.SetActive(true);
        }
    }
    public void ComentInputFieldChanged()
    {
        if (comentInputField.text.Length == 0)
            saveComentButton.interactable = false;
        else
            saveComentButton.interactable = true;
    }
    public void SaveComent()
    {
        // ��� ���� ��û ���
        CommentInfo commentInfo = new CommentInfo();
        commentInfo.content = comentInputField.text;
        commentInfo.boardId = loadedContent.id;
        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/comment";
        info.body = JsonUtility.ToJson(commentInfo);
        info.contentType = "application/json";
        info.onComplete = (DownloadHandler res) =>
        {
            ToastMessage.OnMessage("����� ��ϵǾ����ϴ�");
            LoadComentsData(loadedContent.id);
        };
        comentInputField.text = "";
        saveComentButton.interactable = false;
        StartCoroutine(HttpManager.GetInstance().Post(info));
    }
    [Serializable]
    private struct CommentInfo
    {
        public string content;
        public int boardId;
    }

    public GameObject[] sortTabs;
    private bool sortByLike;
    public void SortByLike(bool value)
    {
        sortByLike = value;
        if (value)
        {   // �α��
            sortTabs[0].gameObject.SetActive(false);
            sortTabs[1].gameObject.SetActive(true);
        }
        else
        {   // �ֽż�
            sortTabs[0].gameObject.SetActive(true);
            sortTabs[1].gameObject.SetActive(false);
        }
        LoadBoardData();
    }
    private BoardContent loadedContent;
    public void LoadBoardData()
    {
        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/board/list";
        info.onComplete = (DownloadHandler res) =>
        {
            // ����
            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            // ����
            BoardGetList boardGetList = JsonUtility.FromJson<BoardGetList>("{\"data\":" + res.text + "}");
            if (sortByLike)
            {
                boardGetList.data.Sort((a, b) =>
                {
                    int result = a.likeCount.CompareTo(b.likeCount);
                    if (result == 0)
                        result = a.boardId.CompareTo(b.boardId);
                    return result;
                });
            }
            for (int i = boardGetList.data.Count - 1; i >= 0; i--)
            {
                GameObject newPanel = Instantiate(contentPrefab, content.transform);
                BoardContent comp = newPanel.GetComponent<BoardContent>();
                comp.id = boardGetList.data[i].boardId;
                comp.title = boardGetList.data[i].title;
                comp.content = boardGetList.data[i].content;
                comp.like= boardGetList.data[i].likeCount;
                comp.text.text = comp.title;
                comp.likeCountText.text = comp.like.ToString();
                // ��� ���� ���� �ʿ�\
                //comp.comentCountText.text = comp.
                comp.button.onClick.AddListener(() =>
                {
                    loadedContent = comp;
                    contentPanel.SetActive(true);
                    titleText.text = comp.title;
                    contentText.text = comp.content;
                    likeCountText.text = comp.like.ToString();
                    comentCountText.text = comp.comentCount.ToString();
                    LoadComentsData(comp.id);
                    likeButton.onClick.AddListener(() =>
                    {
                        // ���ƿ� ��ư

                    });
                });
            }
        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }
    public GameObject commentPrefab;
    public void LoadComentsData(int id)
    {
        // ����
        for (int i = 2; i < boardContents.transform.childCount; i++)
        {
            Destroy(boardContents.transform.GetChild(i).gameObject);
        }
        // ����
        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/comment/" + id;
        info.onComplete = (DownloadHandler res) =>
        {
            CommentList data = JsonUtility.FromJson<CommentList>("{\"data\":" + res.text + "}");
            for (int i = 0; i < data.data.Length; i++)
            {
                // ��� ����
                GameObject newPanel = Instantiate(commentPrefab, boardContents.transform);
                newPanel.transform.GetComponentInChildren<TMP_Text>().text = data.data[i].content;
            }
        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }
    [Serializable]
    private struct CommentResInfo
    {
        public int commentId;
        public string content;
        public int boardId;
    }
    [Serializable]
    private struct CommentList
    {
        public CommentResInfo[] data;
    }

    [Serializable]
    private struct BoardGetList
    {
        public List<BoardGetInfo> data;
    }

    [Serializable]
    private struct BoardGetInfo
    {
        public int boardId;
        public string title;
        public string content;
        public int likeCount;
    }
}
