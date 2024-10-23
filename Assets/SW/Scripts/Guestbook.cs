using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;
namespace SW
{
    public class Guestbook : MonoBehaviour
    {
        // ����
        public Button closeButton;
        public Button createPanel_onButton;
        public List<RectTransform> contentsRaws = new List<RectTransform>();
        public GameObject contentPrefab;

        // ���� �۾���
        public RectTransform createPanel;
        public Image createPanelImage;
        public Button createPanel_closeButton;
        public Button[] colorButtons;
        public Button saveButton;
        public TMP_InputField inputField;
        private Color selectedColor = Color.white;

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
            // newContent.registDate =
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
                CreateContent(contentsList[i].id, contentsList[i].content, contentsList[i].nickname, DateTime.Now, HexToColor(contentsList[i].rgb));
                yield return null;
            }
        }
        public void CreateContent(int id, string content, string nickname, DateTime time, Color color)
        {
            RectTransform raw = contentsRaws.OrderBy(rt => rt.rect.height).FirstOrDefault();
            GameObject newContent = Instantiate(contentPrefab, raw);
            newContent.GetComponent<Image>().color = color;
            TMP_Text contentText = newContent.GetComponentInChildren<TMP_Text>();
            contentText.text = content;
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentText.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(raw);
        }

        public string ColorToHexRGB(Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255);
            int g = Mathf.RoundToInt(color.g * 255);
            int b = Mathf.RoundToInt(color.b * 255);

            return $"#{r:X2}{g:X2}{b:X2}";
        }
        public static Color HexToColor(string hex)
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