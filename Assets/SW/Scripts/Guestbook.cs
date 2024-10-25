using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;
namespace SW
{
    public class Guestbook : MonoBehaviour
    {
        [Header("����")]
        public Button closeButton;
        public Button createPanel_onButton;
        public List<RectTransform> contentsRaws = new List<RectTransform>();
        public GameObject contentPrefab;

        [Header("���� �۾���")]
        public RectTransform createPanel;
        public Image createPanelImage;
        public Button createPanel_closeButton;
        public Button[] colorButtons;
        public Button saveButton;
        public TMP_InputField inputField;
        private Color selectedColor = Color.white;

        [Header("���� ����")]
        public RectTransform deletePanel;
        public Button cancelDeleteButton;
        public Button confirmDeleteButton;
        private ContentData deleteSelected;

        // ���� ����Ʈ
        private List<ContentData> contentsList = new List<ContentData>();
        private void Start()
        {
            closeButton.onClick.AddListener(() => { OffPanel(); });
            createPanel_onButton.onClick.AddListener(() => { SetActiveCreatePanel(true); });
            createPanel_closeButton.onClick.AddListener(() => { SetActiveCreatePanel(false); });
            foreach (Button button in colorButtons)
            {
                button.onClick.AddListener(() => { SetColor(button); });
            }
            cancelDeleteButton.onClick.AddListener(() => { OnCancelButtonClick(); });
            confirmDeleteButton.onClick.AddListener(() => { OnConfirmButtonClick(); });
        }
        private void OffPanel()
        {
            gameObject.SetActive(false);
            createPanel.gameObject.SetActive(false);
        }
        private void SetActiveCreatePanel(bool value)
        {
            createPanel.gameObject.SetActive(value);
        }
        private void SetColor(Button _button)
        {
            foreach (Button button in colorButtons)
            {
                if (button == _button) button.transform.GetChild(0).gameObject.SetActive(true);
                else button.transform.GetChild(0).gameObject.SetActive(false);
            }
            Color buttonColor = _button.GetComponent<Image>().color;
            if (buttonColor == Color.white) createPanelImage.color = new Color(0.9529412f, 0.9607844f, 0.9764706f);
            else createPanelImage.color = buttonColor;
            selectedColor = buttonColor;
        }
        public void OnValueChanged(string value)
        {
            if (value.Length == 0) saveButton.interactable = false;
            else saveButton.interactable = true;
        }
        public void OnSaveButtonClick()
        {
            // ����
            ContentData newContent = new ContentData();
            newContent.nickname = "�̱���";
            newContent.content = inputField.text;
            newContent.rgb = ColorToHexRGB(selectedColor);
            newContent.registDate = DateTime.Now.ToString("O");
            contentsList.Insert(0, newContent);
            StartCoroutine(RefreshList());
            // �ݱ�
            inputField.text = "";
            SetActiveCreatePanel(false);
        }
        IEnumerator RefreshList()
        {
            // ����
            foreach (RectTransform raw in contentsRaws)
            {
                print(raw.childCount);
                for (int i = raw.childCount - 1; i >= 0; i--)
                {
                    Destroy(raw.GetChild(i).gameObject);
                }
            }
            yield return null;
            // �߰�
            for (int i = 0; i < contentsList.Count; i++)
            {
                CreateContent(contentsList[i]);
                yield return null;
            }
        }
        private void CreateContent(ContentData contentData)
        {
            RectTransform raw = contentsRaws.OrderBy(rt => rt.rect.height).FirstOrDefault();
            GameObject newContent = Instantiate(contentPrefab, raw);
            Button delBtn = newContent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
            // ���� ��ư
            delBtn.onClick.AddListener(() =>
            {
                deleteSelected = contentData;
                deletePanel.gameObject.SetActive(true);
            });
            newContent.GetComponent<Image>().color = HexToColor(contentData.rgb);
            TMP_Text contentText = newContent.transform.GetChild(0).GetComponent<TMP_Text>();
            contentText.text = contentData.content;
            TMP_Text infoText = newContent.transform.GetChild(1).GetComponent<TMP_Text>();
            infoText.text = "<b>" + contentData.nickname + "</b> <color=#A4A4A4>" + GetElapsedTime(contentData.registDate) + "</color>";
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentText.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(raw);
        }

        private string ColorToHexRGB(Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255);
            int g = Mathf.RoundToInt(color.g * 255);
            int b = Mathf.RoundToInt(color.b * 255);

            return $"#{r:X2}{g:X2}{b:X2}";
        }
        private Color HexToColor(string hex)
        {
            // '#' ���ڰ� ������ ����
            hex = hex.Replace("#", "");

            // 16���� ���ڿ��� ���� RGB�� ��ȯ
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            // 0~255 ������ ���� 0~1�� ��ȯ�ؼ� Color�� ��ȯ (���Ĵ� 1�� ����)
            return new Color(r / 255f, g / 255f, b / 255f, 1f);
        }
        private string GetElapsedTime(string time)
        {
            DateTime registDate = DateTime.Parse(time);

            // ���� �ð��� ��
            DateTime currentTime = DateTime.Now;
            TimeSpan timeDifference = currentTime - registDate;

            // ��� �ð��� ���� �ٸ� �������� ���
            string timeAgo;
            if (timeDifference.TotalMinutes < 1)
            {
                timeAgo = "0m";
            }
            else if (timeDifference.TotalMinutes < 60)
            {
                timeAgo = $"{Math.Floor(timeDifference.TotalMinutes)}m";  // ��
            }
            else if (timeDifference.TotalHours < 24)
            {
                timeAgo = $"{Math.Floor(timeDifference.TotalHours)}h";  // �ð�
            }
            else
            {
                timeAgo = $"{Math.Floor(timeDifference.TotalDays)}d";  // ��
            }

            return timeAgo;
        }
        public void OnDeleteButtonClick(ContentData contentData)
        {
            deleteSelected = contentData;
            deletePanel.gameObject.SetActive(true);
        }
        public void OnCancelButtonClick()
        {
            deleteSelected = null;
            deletePanel.gameObject.SetActive(false);
        }
        public void OnConfirmButtonClick()
        {
            contentsList.Remove(deleteSelected);
            StartCoroutine(RefreshList());
            deleteSelected = null;
            deletePanel.gameObject.SetActive(false);
        }
    }
    public class ContentData
    {
        public int id;
        public string nickname;
        public string content;
        public string rgb;
        public string registDate;
        public int likeCount;
    }
}